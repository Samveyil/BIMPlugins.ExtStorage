using Autodesk.Revit.DB;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ElementExtensions
    {
        public static ElementType ToElementType(this Element element, Document doc = null)
        {
            doc ??= RevitAPI.Document;
            return element.GetTypeId().ToElement<ElementType>(doc);
        }
        public static T ToElementType<T>(this Element element, Document doc = null) where T : ElementType
        {
            doc ??= RevitAPI.Document;
            return element.GetTypeId().ToElement<T>(doc);
        }

        public static BuiltInCategory GetBuiltInCategory(this Element element)
        {
            if (element.Category.Id.GetValue() < 0)
            {
                return (BuiltInCategory)element.Category.Id.GetValue();
            }
            
            return BuiltInCategory.INVALID;
        }
        
        public static Solid ToSolid(this Element element, Options options = null)
        {
            options ??= new Options() { DetailLevel = ViewDetailLevel.Fine };

            GeometryElement geomElem = element.get_Geometry(options);
            foreach (GeometryObject geomObj in geomElem)
            {
                if (geomObj is GeometryInstance geomInst)
                {
                    foreach (var geometry in geomInst.GetInstanceGeometry())
                    {
                        if (geometry is Solid solid && solid.Volume > 0)
                        {
                            return solid;
                        }
                    }
                }
                else if (geomObj is Solid solid && solid.Volume > 0)
                {
                    return solid;
                }
            }

            return null;
        }

        public enum LocationType { Direction, StartPoint, EndPoint }
        public static XYZ ToLocationCoordinates(this Element element, LocationType type = LocationType.Direction)
        {
            return element.Location switch
            {
                LocationPoint locationPoint => locationPoint.Point,
                LocationCurve { Curve: Line line } => type switch
                {
                    LocationType.Direction => line.Direction,
                    LocationType.StartPoint => line.GetEndPoint(0),
                    _ => line.GetEndPoint(1)
                },
                _ => null
            };
        }

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
        public static Parameter ToParameter(this Element element, ElementId parameterId)
        {
            return element.Parameters.Cast<Parameter>().FirstOrDefault(p => p.Id.ToString() == parameterId.ToString());
        }
    }
}