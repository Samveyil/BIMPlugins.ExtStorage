using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class GeometryExtensions
    {
        private const double Tolerance = 1e-9;

        /// <summary>Creates an instance of a point with a new X coordinate</summary>
        /// <param name="x">New X coordinate</param>
        public static XYZ SetX(this XYZ point, double x) => new XYZ(x, point.Y, point.Z);

        /// <summary>Creates an instance of a point with a new Y coordinate</summary>
        /// <param name="y">New Y coordinate</param>
        public static XYZ SetY(this XYZ point, double y) => new XYZ(point.X, y, point.Z);

        /// <summary>Creates an instance of a point with a new Z coordinate</summary>
        /// <param name="z">New Z coordinate</param>
        public static XYZ SetZ(this XYZ point, double z) => new XYZ(point.X, point.Y, z);


        /// <summary>Creates an instance of a line with a new X coordinate</summary>
        /// <param name="x">New X coordinate</param>
        /// <returns>The new bound line</returns>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentsInconsistentException">
        ///     Curve length is too small for Revit's tolerance (as identified by Application.ShortCurveTolerance)
        /// </exception>
        public static Line SetCoordinateX(this Line line, double x)
        {
            var endPoint0 = line.GetEndPoint(0);
            var endPoint1 = line.GetEndPoint(1);
            return Line.CreateBound(endPoint0.SetX(x), endPoint1.SetX(x));
        }

        /// <summary>Creates an instance of a line with a new Y coordinate</summary>
        /// <param name="y">New Y coordinate</param>
        /// <returns>The new bound line</returns>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentsInconsistentException">
        ///     Curve length is too small for Revit's tolerance (as identified by Application.ShortCurveTolerance)
        /// </exception>
        public static Line SetCoordinateY(this Line line, double y)
        {
            var endPoint0 = line.GetEndPoint(0);
            var endPoint1 = line.GetEndPoint(1);
            return Line.CreateBound(endPoint0.SetY(y), endPoint1.SetY(y));
        }

        /// <summary>Creates an instance of a line with a new Z coordinate</summary>
        /// <param name="z">New Z coordinate</param>
        /// <returns>The new bound line</returns>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentsInconsistentException">
        ///     Curve length is too small for Revit's tolerance (as identified by Application.ShortCurveTolerance)
        /// </exception>
        public static Line SetCoordinateZ(this Line line, double z)
        {
            var endPoint0 = line.GetEndPoint(0);
            var endPoint1 = line.GetEndPoint(1);
            return Line.CreateBound(endPoint0.SetZ(z), endPoint1.SetZ(z));
        }


        /// <summary> Determines whether the specified point is contained within this bounding box</summary>
        /// <param name="point">The <see cref="Autodesk.Revit.DB.XYZ" /> point to check for containment within this bounding box</param>
        /// <returns><see langword="true"/> if the specified point is within the bounds of this bounding box</returns>
        public static bool Contains(this BoundingBoxXYZ bbox, XYZ point) => bbox.Contains(point, false);

        /// <summary>
        ///     Determines whether the specified point is contained within this bounding box
        /// </summary>
        /// <param name="point">The <see cref="Autodesk.Revit.DB.XYZ" /> point to check for containment within this bounding box</param>
        /// <param name="strict"><see langword="true"/> if the point needs to be fully on the inside of this bounding box. A point coinciding with the box border will be considered 'outside'.</param>
        /// <returns><see langword="true"/> if the specified point is within the bounds of this bounding box</returns>
        public static bool Contains(this BoundingBoxXYZ bbox, XYZ point, bool strict)
        {
            if (!bbox.Transform.IsIdentity)
            {
                point = bbox.Transform.Inverse.OfPoint(point);
            }

            var insideX = strict
                ? point.X > bbox.Min.X + Tolerance && point.X < bbox.Max.X - Tolerance
                : point.X >= bbox.Min.X - Tolerance && point.X <= bbox.Max.X + Tolerance;

            var insideY = strict
                ? point.Y > bbox.Min.Y + Tolerance && point.Y < bbox.Max.Y - Tolerance
                : point.Y >= bbox.Min.Y - Tolerance && point.Y <= bbox.Max.Y + Tolerance;

            var insideZ = strict
                ? point.Z > bbox.Min.Z + Tolerance && point.Z < bbox.Max.Z - Tolerance
                : point.Z >= bbox.Min.Z - Tolerance && point.Z <= bbox.Max.Z + Tolerance;

            return insideX && insideY && insideZ;
        }

        /// <summary>
        ///     Determines whether this bounding box contains another bounding box
        /// </summary>
        /// <param name="other">The <see cref="Autodesk.Revit.DB.BoundingBoxXYZ" /> instance to compare with this bounding box</param>
        /// <returns><see langword="true"/> if this bounding box contains the other bounding box</returns>
        public static bool Contains(this BoundingBoxXYZ bbox, BoundingBoxXYZ other) => bbox.Contains(other, false);

        /// <summary>
        ///     Determines whether this bounding box contains another bounding box
        /// </summary>
        /// <param name="other">The <see cref="Autodesk.Revit.DB.BoundingBoxXYZ" /> instance to compare with this bounding box</param>
        /// <param name="strict"><see langword="true"/> if the other box needs to be fully on the inside of this bounding box. Coincident boxes will be considered 'outside'.</param>
        /// <returns><see langword="true"/> if this bounding box contains the other bounding box</returns>
        public static bool Contains(this BoundingBoxXYZ bbox, BoundingBoxXYZ other, bool strict)
        {
            var boxMin = bbox.Transform.IsIdentity ? bbox.Min : bbox.Transform.OfPoint(bbox.Min);
            var boxMax = bbox.Transform.IsIdentity ? bbox.Max : bbox.Transform.OfPoint(bbox.Max);
            var otherMin = other.Transform.IsIdentity ? other.Min : other.Transform.OfPoint(other.Min);
            var otherMax = other.Transform.IsIdentity ? other.Max : other.Transform.OfPoint(other.Max);

            var insideX = strict
                ? otherMin.X > boxMin.X + Tolerance && otherMax.X < boxMax.X - Tolerance
                : otherMin.X >= boxMin.X - Tolerance && otherMax.X <= boxMax.X + Tolerance;

            var insideY = strict
                ? otherMin.Y > boxMin.Y + Tolerance && otherMax.Y < boxMax.Y - Tolerance
                : otherMin.Y >= boxMin.Y - Tolerance && otherMax.Y <= boxMax.Y + Tolerance;

            var insideZ = strict
                ? otherMin.Z > boxMin.Z + Tolerance && otherMax.Z < boxMax.Z - Tolerance
                : otherMin.Z >= boxMin.Z - Tolerance && otherMax.Z <= boxMax.Z + Tolerance;

            return insideX && insideY && insideZ;
        }

        /// <summary>
        ///     Determines whether this bounding box overlaps with another bounding box
        /// </summary>
        /// <param name="other">The <see cref="Autodesk.Revit.DB.BoundingBoxXYZ" /> instance to compare with this bounding box</param>
        /// <returns><see langword="true"/> if the two bounding boxes have at least one common point</returns>
        public static bool Overlaps(this BoundingBoxXYZ bbox, BoundingBoxXYZ other)
        {
            var boxMin = bbox.Transform.IsIdentity ? bbox.Min : bbox.Transform.OfPoint(bbox.Min);
            var boxMax = bbox.Transform.IsIdentity ? bbox.Max : bbox.Transform.OfPoint(bbox.Max);
            var otherMin = other.Transform.IsIdentity ? other.Min : other.Transform.OfPoint(other.Min);
            var otherMax = other.Transform.IsIdentity ? other.Max : other.Transform.OfPoint(other.Max);

            var overlapX = !(boxMax.X < otherMin.X - Tolerance || boxMin.X > otherMax.X + Tolerance);
            var overlapY = !(boxMax.Y < otherMin.Y - Tolerance || boxMin.Y > otherMax.Y + Tolerance);
            var overlapZ = !(boxMax.Z < otherMin.Z - Tolerance || boxMin.Z > otherMax.Z + Tolerance);

            return overlapX && overlapY && overlapZ;
        }
    }
}