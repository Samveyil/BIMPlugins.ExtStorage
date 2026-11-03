using Autodesk.Revit.DB;
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
        /// <remarks>The element must have a category: for elements without one, such as some internal elements, the method throws <see cref="System.NullReferenceException"/>.</remarks>
        /// <returns>The built-in category of the element, or <see cref="Autodesk.Revit.DB.BuiltInCategory.INVALID"/> if the category is not built-in.</returns>
        public static BuiltInCategory GetBuiltInCategory(this Element element)
        {
            if (element.Category.Id.GetValue() < 0)
            {
                return (BuiltInCategory)element.Category.Id.GetValue();
            }
            
            return BuiltInCategory.INVALID;
        }


        /// <summary>Retrieves the element's solid.</summary>
        /// <param name="element">The element.</param>
        /// <param name="options">User preferences for parsing of geometry. Passing <see langword="null"/> will return a default Options object.</param>
        /// <param name="isSymbolGeometry"><see langword="true"/> if computes the geometric representation of the symbol which generates this instance; otherwise, of the instance.</param>
        /// <returns>The element's solid. This return may be <see langword="null"/> if there is no matching geometry.</returns>
        public static Solid ToSolid(this Element element, Options options = null, bool isSymbolGeometry = false)
        {
            options ??= new Options() { DetailLevel = ViewDetailLevel.Fine };

            GeometryElement geomElem = element.get_Geometry(options);
            foreach (GeometryObject geomObj in geomElem)
            {
                if (geomObj is GeometryInstance geomInst)
                {
                    var solid = (isSymbolGeometry ? geomInst.GetSymbolGeometry() : geomInst.GetInstanceGeometry())
                        .OfType<Solid>()
                        .OrderByDescending(s => s.Volume)
                        .ThenByDescending(s => s.SurfaceArea)
                        .FirstOrDefault();
                    
                    if (solid != null)
                    {
                        if (solid.Volume > 0)
                            return solid;
                        else if (element.GetBuiltInCategory() == BuiltInCategory.OST_DetailComponents && solid.SurfaceArea > 0)
                            return solid;
                    }
                }
                else if (geomObj is Solid solid && solid.Volume > 0)
                {
                    return solid;
                }
            }

            return null;
        }

        /// <summary>A list of points to retrieve from the line.</summary>
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

        /// <summary>Retrieves the location line of the element.</summary>
        /// <remarks>The element must be located by a straight line, as walls or beams are. Otherwise the method throws an exception.</remarks>
        /// <returns>The location line of the element.</returns>
        public static Line ToLine(this Element element)
        {
            return (element.Location as LocationCurve).Curve.To<Line>();
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
        /// <returns>The matching parameter.</returns>
        public static Parameter ToParameter(this Element element, ElementId parameterId)
        {
            return element.Parameters.Cast<Parameter>().FirstOrDefault(p => p.Id.ToString() == parameterId.ToString());
        }
    }
}