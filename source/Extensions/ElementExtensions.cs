using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for <see cref="Autodesk.Revit.DB.Element"/>: type, category, geometry, location and parameters.</summary>
    public static class ElementExtensions
    {
        /// <summary>Returns the element's type.</summary>
        /// <returns>The element's type.</returns>
        public static ElementType ToElementType(this Element element)
        {
            return element.GetTypeId().ToElement<ElementType>(element.Document);
        }

        /// <summary>Returns the element's type.</summary>
        /// <returns>The element's type.</returns>
        public static T ToElementType<T>(this Element element) where T : ElementType
        {
            return element.GetTypeId().ToElement<T>(element.Document);
        }


        /// <summary>Retrieves the category of the element.</summary>
        /// <returns>
        /// The built-in category of the element, or <see cref="Autodesk.Revit.DB.BuiltInCategory.INVALID"/> if the element
        /// has no category or its category is not built-in.
        /// </returns>
        public static BuiltInCategory GetBuiltInCategory(this Element element)
        {
            var categoryId = element.Category?.Id;
            if (categoryId != null && categoryId.GetValue() < 0)
            {
                return (BuiltInCategory)categoryId.GetValue();
            }

            return BuiltInCategory.INVALID;
        }


        /// <summary>Retrieves the element's solid.</summary>
        /// <param name="element">The element.</param>
        /// <param name="options">User preferences for parsing of geometry. Passing <see langword="null"/> will return a default Options object.</param>
        /// <param name="isSymbolGeometry"><see langword="true"/> if computes the geometric representation of the symbol which generates this instance; otherwise, of the instance.</param>
        /// <returns>
        /// The largest solid of the element, the one of the greatest volume, from its own geometry and from the family geometry alike.
        /// A detail component may give a flat solid with no volume. <see langword="null"/> if there is no matching geometry.
        /// </returns>
        public static Solid ToSolid(this Element element, Options options = null, bool isSymbolGeometry = false)
        {
            options ??= new Options() { DetailLevel = ViewDetailLevel.Fine };

            GeometryElement geomElem = element.get_Geometry(options);
            if (geomElem == null)
                return null;

            var solids = new List<Solid>();
            foreach (GeometryObject geomObj in geomElem)
            {
                if (geomObj is GeometryInstance geomInst)
                    solids.AddRange((isSymbolGeometry ? geomInst.GetSymbolGeometry() : geomInst.GetInstanceGeometry()).OfType<Solid>());
                else if (geomObj is Solid solid)
                    solids.Add(solid);
            }

            var largest = solids
                .OrderByDescending(s => s.Volume)
                .ThenByDescending(s => s.SurfaceArea)
                .FirstOrDefault();

            if (largest == null)
                return null;

            if (largest.Volume > 0)
                return largest;

            return element.GetBuiltInCategory() == BuiltInCategory.OST_DetailComponents && largest.SurfaceArea > 0
                ? largest
                : null;
        }

        /// <summary>Retrieves the location line of the element.</summary>
        /// <returns>The location line of the element, or <see langword="null"/> if it is not located by a straight line, as walls or beams are.</returns>
        public static Line ToLine(this Element element)
        {
            return (element.Location as LocationCurve)?.Curve as Line;
        }

        /// <summary>Retrieves the location point of the element.</summary>
        /// <param name="element">The element.</param>
        /// <param name="type">Line's point to retrieve.</param>
        /// <remarks>For elements located by a point, <paramref name="type"/> is ignored and the location point is returned.</remarks>
        /// <returns>The point, or <see langword="null"/> if the element is located neither by a point nor by a straight line.</returns>
        public static XYZ ToPoint(this Element element, LocationType type = LocationType.Origin)
        {
            return element.Location switch
            {
                LocationPoint locationPoint => locationPoint.Point,
                LocationCurve { Curve: Line line } => type switch
                {
                    LocationType.Origin => line.Origin,
                    LocationType.Direction => line.Direction,
                    LocationType.StartPoint => line.GetEndPoint(0),
                    _ => line.GetEndPoint(1)
                },
                _ => null
            };
        }


        /// <summary>Retrieves the parameter from the element via the given name.</summary>
        /// <param name="element">The element.</param>
        /// <param name="parameterName">The name of the parameter to be retrieved.</param>
        /// <returns>The matching parameter.</returns>
        public static Parameter ToParameter(this Element element, string parameterName)
        {
            var parameters = element.GetParameters(parameterName);

            Parameter sharedParameter = null;

            foreach (var parameter in parameters)
            {
                if ((parameter.Definition as InternalDefinition).BuiltInParameter != BuiltInParameter.INVALID)
                    return parameter;

                if (sharedParameter == null && parameter.IsShared)
                    sharedParameter = parameter;
            }

            return sharedParameter ?? element.LookupParameter(parameterName);
        }

        /// <summary>Retrieves the parameter from the element via the given id.</summary>
        /// <param name="element">The element.</param>
        /// <param name="parameterId">The id of the parameter to be retrieved.</param>
        /// <returns>The matching parameter, or <see langword="null"/> if the element has no such parameter.</returns>
        public static Parameter ToParameter(this Element element, ElementId parameterId)
        {
            // Built-in and shared parameters are found directly; only other parameters are looked for among all of them.
            var id = parameterId.GetValue();
            if (id < 0)
                return element.get_Parameter((BuiltInParameter)id);

            if (parameterId.ToElement(element.Document) is SharedParameterElement sharedParameter)
                return element.get_Parameter(sharedParameter.GuidValue);

            return element.Parameters.Cast<Parameter>().FirstOrDefault(p => p.Id.Equals(parameterId));
        }
    }

    /// <summary>The point to retrieve from a location line, for <see cref="ElementExtensions.ToPoint"/>.</summary>
    public enum LocationType
    {
        /// <summary>The origin of the line.</summary>
        Origin,
        /// <summary>The direction of the line: a unit vector, not a point.</summary>
        Direction,
        /// <summary>The start point of the line.</summary>
        StartPoint,
        /// <summary>The end point of the line.</summary>
        EndPoint
    }
}