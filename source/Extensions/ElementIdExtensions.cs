using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for <see cref="Autodesk.Revit.DB.ElementId"/>: getting elements, id values and parameter filters.</summary>
    public static class ElementIdExtensions
    {
        /// <summary>Gets the Element.</summary>
        /// <param name="id">The element id.</param>
        /// <param name="doc">The owning document. Passing <see langword="null"/> will return the active document</param>
        /// <returns>The element.</returns>
        public static Element ToElement(this ElementId id, Document doc = null)
        {
            doc ??= RevitAPI.Document;
            return doc.GetElement(id);
        }

        /// <summary>Gets the Element.</summary>
        /// <typeparam name="T">The element type to return (must inherit from Element).</typeparam>
        /// <param name="id">The element id.</param>
        /// <param name="doc">The owning document. Passing <see langword="null"/> will return the active document</param>
        /// <returns>The element cast to type <typeparamref name="T"/>.</returns>
        public static T ToElement<T>(this ElementId id, Document doc = null) where T : Element => (T)id.ToElement(doc);


#if !R2024_OR_GREATER
        /// <summary>Provides the value of the element id as an integer.</summary>
        /// <returns>The id value. Before Revit 2024 the type is <see cref="int"/>, since Revit 2024 it is <see cref="long"/>.</returns>
        public static int GetValue(this ElementId id)
        {
            return id.IntegerValue;
        }
#else
        /// <summary>Provides the value of the element id as a 64-bit integer.</summary>
        /// <returns>The id value. Before Revit 2024 the type is <see cref="int"/>, since Revit 2024 it is <see cref="long"/>.</returns>
        public static long GetValue(this ElementId id)
        {
            return id.Value;
        }
#endif


        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether double-precision values from the document equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <remarks>The value is in Revit internal units: feet for lengths, radians for angles.</remarks>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, double value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value, double.Epsilon);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether integer values from the document equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, int value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether ElementId values from the document equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateEqualsFilter(this ElementId parameterId, ElementId value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateEqualsRule(parameterId, value);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether string values from the document do not equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotEqualsFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether double-precision values from the document do not equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <remarks>The value is in Revit internal units: feet for lengths, radians for angles.</remarks>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotEqualsFilter(this ElementId parameterId, double value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, value, double.Epsilon);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether integer values from the document do not equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotEqualsFilter(this ElementId parameterId, int value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, value);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether ElementId values from the document do not equal a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotEqualsFilter(this ElementId parameterId, ElementId value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(parameterId, value);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether double-precision values from the document are greater than or equal to a certain value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied value against which values from the document will be compared.</param>
        /// <remarks>The value is in Revit internal units: feet for lengths, radians for angles.</remarks>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateGreaterOrEqualFilter(this ElementId parameterId, double value)
        {
            var filterRule = ParameterFilterRuleFactory.CreateGreaterOrEqualRule(parameterId, value, double.Epsilon);
            return new ElementParameterFilter(filterRule);
        }

#if R2020_OR_GREATER
        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether an element's parameter has a value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateHasValueFilter(this ElementId parameterId)
        {
            var filterRule = ParameterFilterRuleFactory.CreateHasValueParameterRule(parameterId);
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether an element's parameter does not have a value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateHasNoValueFilter(this ElementId parameterId)
        {
            var filterRule = ParameterFilterRuleFactory.CreateHasNoValueParameterRule(parameterId);
            return new ElementParameterFilter(filterRule);
        }
#endif

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document contain a certain string value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied string value for which values from the document will be searched.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateContainsFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateContainsRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateContainsRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document do not contain a certain string value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied string value for which values from the document will be searched.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotContainsFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateNotContainsRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateNotContainsRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document begin with a certain string value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied string value for which values from the document will be searched.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateBeginsWithFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateBeginsWithRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateBeginsWithRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document do not begin with a certain string value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied string value for which values from the document will be searched.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotBeginsWithFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateNotBeginsWithRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateNotBeginsWithRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document end with a certain string value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied string value for which values from the document will be searched.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateEndsWithFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateEndsWithRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateEndsWithRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }

        /// <summary>
        /// Constructs a new instance of an ElementParameterFilter from a filter rule that determines whether strings from the document do not end with a certain string value.
        /// </summary>
        /// <param name="parameterId">The id of the parameter, for example <c>new ElementId(BuiltInParameter.ALL_MODEL_MARK)</c> or the id of a <see cref="Autodesk.Revit.DB.SharedParameterElement"/>.</param>
        /// <param name="value">The user-supplied string value for which values from the document will be searched.</param>
        /// <returns>The parameter filter. Pass it to <c>ToElements</c> or a <see cref="Autodesk.Revit.DB.FilteredElementCollector"/>.</returns>
        public static ElementParameterFilter CreateNotEndsWithFilter(this ElementId parameterId, string value)
        {
#if !R2023_OR_GREATER
            var filterRule = ParameterFilterRuleFactory.CreateNotEndsWithRule(parameterId, value, false);
#else
            var filterRule = ParameterFilterRuleFactory.CreateNotEndsWithRule(parameterId, value);
#endif
            return new ElementParameterFilter(filterRule);
        }
    }
}