using Autodesk.Revit.DB;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class BindingMapExtensions
    {
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

#if !R2025_OR_GREATER
        public static bool InsertParameter(this BindingMap bindingMap, string paramName, Guid paramGuid, List<Category> categories, bool isInstance=true, BuiltInParameterGroup parameterGroup=BuiltInParameterGroup.INVALID)
#else
        public static bool InsertParameter(this BindingMap bindingMap, string paramName, Guid paramGuid, List<Category> categories, bool isInstance = true)
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
#if !R2025_OR_GREATER
                    result = bindingMap.Insert(extDef, binding, parameterGroup);
#else
                    result = bindingMap.Insert(extDef, binding);
#endif
                    t.Commit();
                }

                return result;
            }

            return true;
        }
    }
}