using Autodesk.Revit.DB;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for <see cref="Autodesk.Revit.DB.BindingMap"/>: finding and binding project parameters.</summary>
    public static class BindingMapExtensions
    {
        /// <summary>Retrieves the definition which has the given name.</summary>
        /// <param name="bindingMap">The parameter bindings of the document, <c>doc.ParameterBindings</c>.</param>
        /// <param name="paramName">The name of the definition to be retrieved.</param>
        /// <returns>The matching definition. This return may be <see langword="null"/> if there is no matching definition.</returns>
        public static InternalDefinition ToDefinition(this BindingMap bindingMap, string paramName)
        {
            var iter = bindingMap.ForwardIterator();
            while (iter.MoveNext())
            {
                var def = iter.Key as InternalDefinition;

                if (def.Name == paramName)
                {
                    return def;
                }
            }

            return null;
        }

#if !R2022_OR_GREATER
        /// <summary>Creates a new parameter binding between a shared parameter and a set of categories in a specified group.</summary>
        /// <param name="bindingMap">The parameter bindings of the document, <c>doc.ParameterBindings</c>.</param>
        /// <param name="doc">The document</param>
        /// <param name="paramName">The name of the shared parameter.</param>
        /// <param name="paramGuid">The guid of the shared parameter.</param>
        /// <param name="categories">The list of categories to which the parameter should be bound.</param>
        /// <param name="isInstance"><see langword="true"/> if an InstanceBinding; otherwise, TypeBinding.</param>
        /// <param name="parameterGroup">The GroupID of the parameter definition, or INVALID if the parameter is not to be associated with any predefined group.</param>
        /// <remarks>
        /// The shared parameter is taken from the shared parameter file currently set in Revit.
        /// If the parameter is already bound, the missing categories are added to the binding; <paramref name="isInstance"/> and the group are ignored.
        /// The method starts its own transactions, so it must not be called inside an open transaction.
        /// </remarks>
        /// <returns><see langword="true"/> if the parameter is bound; <see langword="false"/> if it was not found in the shared parameter file or the binding failed.</returns>
        public static bool InsertParameter(this BindingMap bindingMap, Document doc, string paramName, Guid paramGuid, List<Category> categories, bool isInstance=true, BuiltInParameterGroup parameterGroup=BuiltInParameterGroup.INVALID)
#else
        /// <summary>Creates a new parameter binding between a shared parameter and a set of categories in a specified group.</summary>
        /// <param name="bindingMap">The parameter bindings of the document, <c>doc.ParameterBindings</c>.</param>
        /// <param name="doc">The document</param>
        /// <param name="paramName">The name of the shared parameter.</param>
        /// <param name="paramGuid">The guid of the shared parameter.</param>
        /// <param name="categories">The list of categories to which the parameter should be bound.</param>
        /// <param name="isInstance"><see langword="true"/> if an InstanceBinding; otherwise, TypeBinding.</param>
        /// <param name="groupTypeId">The identifier of the parameter definition's parameter group, or empty if the parameter is not to be associated with any predefined group.</param>
        /// <remarks>
        /// The shared parameter is taken from the shared parameter file currently set in Revit.
        /// If the parameter is already bound, the missing categories are added to the binding; <paramref name="isInstance"/> and the group are ignored.
        /// The method starts its own transactions, so it must not be called inside an open transaction.
        /// </remarks>
        /// <returns><see langword="true"/> if the parameter is bound; <see langword="false"/> if it was not found in the shared parameter file or the binding failed.</returns>
        public static bool InsertParameter(this BindingMap bindingMap, Document doc, string paramName, Guid paramGuid, List<Category> categories, bool isInstance = true, ForgeTypeId groupTypeId=null)
#endif
        {
            var hasParameter = false;

            var iter = bindingMap.ForwardIterator();
            while (iter.MoveNext())
            {
                var bind = iter.Current as ElementBinding;
                var def = iter.Key as InternalDefinition;

                if (def.Name == paramName && def.Id.ToElement() is SharedParameterElement sharedParameter && sharedParameter.GuidValue == paramGuid)
                {
                    bool insertCategories = false;
                    foreach (var category in categories)
                    {
                        if (!bind.Categories.Contains(category))
                        {
                            bind.Categories.Insert(category);
                            insertCategories = true;
                        }
                    }

                    if (insertCategories)
                    {
                        using (Transaction t = new Transaction(doc, "Загрузка параметра"))
                        {
                            t.Start();

                            bindingMap.ReInsert(def, bind);

                            t.Commit();
                        }
                    }  

                    hasParameter = true;
                    break;
                }
            }

            if (!hasParameter)
            {
                var extDef = ParameterMethods.FindExternalDefinition(paramName, paramGuid);
                if (extDef == null)
                {
                    return false;
                }

                var catSet = new CategorySet();
                foreach (var category in categories)
                    catSet.Insert(category);
                
                Binding binding = isInstance ? new InstanceBinding(catSet) : new TypeBinding(catSet);

                bool result;
                using (Transaction t = new Transaction(doc, "Загрузка параметра"))
                {
                    t.Start();
#if !R2022_OR_GREATER
                    result = bindingMap.Insert(extDef, binding, parameterGroup);
#else
                    result = bindingMap.Insert(extDef, binding, groupTypeId ?? new ForgeTypeId(string.Empty));
#endif
                    t.Commit();
                }

                return result;
            }

            return true;
        }
    }
}