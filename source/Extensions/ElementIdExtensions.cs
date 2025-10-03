using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ElementIdExtensions
    {
        public static Element ToElement(this ElementId id, Document doc = null)
        {
            doc ??= RevitAPI.Document;
            return doc.GetElement(id);
        }
        public static T ToElement<T>(this ElementId id, Document doc = null) where T : Element
        {
            doc ??= RevitAPI.Document;
            return (T)doc.GetElement(id);
        }

#if !R2024_OR_GREATER
        public static int GetValue(this ElementId id)
        {
            return id.IntegerValue;
        }
#else
        public static long GetValue(this ElementId id)
        {
            return id.Value;
        }
#endif

        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, double value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value, double.Epsilon);
            return new ElementParameterFilter(filterRule);
        }
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, int value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value);
            return new ElementParameterFilter(filterRule);
        }
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, ElementId value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value);
            return new ElementParameterFilter(filterRule);
        }

        public static ElementParameterFilter CreateGreaterOrEqualFilter(this ElementId parameterId, double value)
        {
            ParameterValueProvider provider = new ParameterValueProvider(parameterId);
            var evaluator = new FilterNumericGreaterOrEqual();

            FilterRule rule = new FilterDoubleRule(
                provider,
                evaluator,
                value,
                double.Epsilon
            );

            return new ElementParameterFilter(rule);
        }

#if R2020_OR_GREATER
        public static ElementParameterFilter CreateHasValueFilter(this ElementId parameterId)
        {
            var filterRule = ParameterFilterRuleFactory.CreateHasValueParameterRule(parameterId);
            return new ElementParameterFilter(filterRule);
        }
#endif
        public static ElementParameterFilter CreateContainsFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateContainsRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateContainsRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }
    }
}