using Autodesk.Revit.DB;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class BindingMapExtensions
    {
        /// <summary>Retrieves the definition which has the given name.</summary>
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

        /// <summary>Creates a new parameter binding between a shared parameter and a set of categories in a specified group.</summary>
        /// <param name="paramName">The name of the shared parameter.</param>
        /// <param name="paramGuid">The guid of the shared parameter.</param>
        /// <param name="categories">The list of categories to which the parameter should be bound.</param>
        /// <param name="isInstance">True if an InstanceBinding; otherwise, TypeBinding.</param>
        /// <param name="parameterGroup">The GroupID of the parameter definition, or INVALID if the parameter is not to be associated with any predefined group.</param>
        /// <remarks>Note if a shared parameter exists this method only changes the set of categories.</remarks>
#if !R2022_OR_GREATER
        public static bool InsertParameter(this BindingMap bindingMap, string paramName, Guid paramGuid, List<Category> categories, bool isInstance=true, BuiltInParameterGroup parameterGroup=BuiltInParameterGroup.INVALID)
#else
        /// <summary>Creates a new parameter binding between a shared parameter and a set of categories in a specified group.</summary>
        /// <param name="paramName">The name of the shared parameter.</param>
        /// <param name="paramGuid">The guid of the shared parameter.</param>
        /// <param name="categories">The list of categories to which the parameter should be bound.</param>
        /// <param name="isInstance">True if an InstanceBinding; otherwise, TypeBinding.</param>
        /// <param name="groupTypeId">The identifier of the parameter definition's parameter group, or empty if the parameter is not to be associated with any predefined group.</param>
        /// <remarks>Note if a shared parameter exist this method only change the set of categories.</remarks>
        public static bool InsertParameter(this BindingMap bindingMap, string paramName, Guid paramGuid, List<Category> categories, bool isInstance = true, ForgeTypeId groupTypeId=null)
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
                        using (Transaction t = new Transaction(RevitAPI.Document, "Загрузка параметра"))
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
                using (Transaction t = new Transaction(RevitAPI.Document, "Загрузка параметра"))
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