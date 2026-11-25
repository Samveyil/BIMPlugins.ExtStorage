using Autodesk.Revit.DB;
using System;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for points, lines and bounding boxes.</summary>
    public static class GeometryExtensions
    {
        private const double Tolerance = 1e-9;

        /// <summary>Creates an instance of a point with a new X coordinate.</summary>
        /// <param name="point">The source point.</param>
        /// <param name="x">New X coordinate.</param>
        /// <returns>A new point. The original point is not changed.</returns>
        public static XYZ SetX(this XYZ point, double x) => new XYZ(x, point.Y, point.Z);

        /// <summary>Creates an instance of a point with a new Y coordinate.</summary>
        /// <param name="point">The source point.</param>
        /// <param name="y">New Y coordinate.</param>
        /// <returns>A new point. The original point is not changed.</returns>
        public static XYZ SetY(this XYZ point, double y) => new XYZ(point.X, y, point.Z);

        /// <summary>Creates an instance of a point with a new Z coordinate.</summary>
        /// <param name="point">The source point.</param>
        /// <param name="z">New Z coordinate.</param>
        /// <returns>A new point. The original point is not changed.</returns>
        public static XYZ SetZ(this XYZ point, double z) => new XYZ(point.X, point.Y, z);


        /// <summary>Creates an instance of a line with a new X coordinate.</summary>
        /// <param name="line">The source line.</param>
        /// <param name="x">New X coordinate.</param>
        /// <returns>The new bound line.</returns>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentsInconsistentException">
        ///     Curve length is too small for Revit's tolerance (as identified by Application.ShortCurveTolerance).
        /// </exception>
        public static Line SetCoordinateX(this Line line, double x)
        {
            var endPoint0 = line.GetEndPoint(0);
            var endPoint1 = line.GetEndPoint(1);
            return Line.CreateBound(endPoint0.SetX(x), endPoint1.SetX(x));
        }

        /// <summary>Creates an instance of a line with a new Y coordinate.</summary>
        /// <param name="line">The source line.</param>
        /// <param name="y">New Y coordinate.</param>
        /// <returns>The new bound line.</returns>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentsInconsistentException">
        ///     Curve length is too small for Revit's tolerance (as identified by Application.ShortCurveTolerance).
        /// </exception>
        public static Line SetCoordinateY(this Line line, double y)
        {
            var endPoint0 = line.GetEndPoint(0);
            var endPoint1 = line.GetEndPoint(1);
            return Line.CreateBound(endPoint0.SetY(y), endPoint1.SetY(y));
        }

        /// <summary>Creates an instance of a line with a new Z coordinate.</summary>
        /// <param name="line">The source line.</param>
        /// <param name="z">New Z coordinate.</param>
        /// <returns>The new bound line.</returns>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentsInconsistentException">
        ///     Curve length is too small for Revit's tolerance (as identified by Application.ShortCurveTolerance).
        /// </exception>
        public static Line SetCoordinateZ(this Line line, double z)
        {
            var endPoint0 = line.GetEndPoint(0);
            var endPoint1 = line.GetEndPoint(1);
            return Line.CreateBound(endPoint0.SetZ(z), endPoint1.SetZ(z));
        }


        /// <summary> Determines whether the specified point is contained within this bounding box.</summary>
        /// <param name="bbox">The bounding box.</param>
        /// <param name="point">The <see cref="Autodesk.Revit.DB.XYZ" /> point to check for containment within this bounding box.</param>
        /// <returns><see langword="true"/> if the specified point is within the bounds of this bounding box.</returns>
        public static bool Contains(this BoundingBoxXYZ bbox, XYZ point) => bbox.Contains(point, false);

        /// <summary>
        ///     Determines whether the specified point is contained within this bounding box.
        /// </summary>
        /// <param name="bbox">The bounding box.</param>
        /// <param name="point">The <see cref="Autodesk.Revit.DB.XYZ" /> point to check for containment within this bounding box.</param>
        /// <param name="strict"><see langword="true"/> if the point needs to be fully on the inside of this bounding box. A point coinciding with the box border will be considered 'outside'.</param>
        /// <returns><see langword="true"/> if the specified point is within the bounds of this bounding box.</returns>
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
        ///     Determines whether this bounding box contains another bounding box.
        /// </summary>
        /// <param name="bbox">The bounding box.</param>
        /// <param name="other">The <see cref="Autodesk.Revit.DB.BoundingBoxXYZ" /> instance to compare with this bounding box.</param>
        /// <returns><see langword="true"/> if this bounding box contains the other bounding box.</returns>
        public static bool Contains(this BoundingBoxXYZ bbox, BoundingBoxXYZ other) => bbox.Contains(other, false);

        /// <summary>
        ///     Determines whether this bounding box contains another bounding box.
        /// </summary>
        /// <param name="bbox">The bounding box.</param>
        /// <param name="other">The <see cref="Autodesk.Revit.DB.BoundingBoxXYZ" /> instance to compare with this bounding box.</param>
        /// <param name="strict"><see langword="true"/> if the other box needs to be fully on the inside of this bounding box. Coincident boxes will be considered 'outside'.</param>
        /// <returns><see langword="true"/> if this bounding box contains the other bounding box.</returns>
        /// <remarks>Both boxes may have a transform: the other box is contained when all its corners are.</remarks>
        public static bool Contains(this BoundingBoxXYZ bbox, BoundingBoxXYZ other, bool strict)
        {
            foreach (var corner in Corners(other))
            {
                if (!bbox.Contains(corner, strict))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     Determines whether this bounding box overlaps with another bounding box.
        /// </summary>
        /// <param name="bbox">The bounding box.</param>
        /// <param name="other">The <see cref="Autodesk.Revit.DB.BoundingBoxXYZ" /> instance to compare with this bounding box.</param>
        /// <returns><see langword="true"/> if the two bounding boxes have at least one common point.</returns>
        /// <remarks>Boxes with a transform, rotated ones included, are compared exactly.</remarks>
        public static bool Overlaps(this BoundingBoxXYZ bbox, BoundingBoxXYZ other)
        {
            // Separating axis test: two boxes do not overlap if and only if their corners, projected onto one of these axes,
            // fall into two separate intervals. The axes are the 3 edges of each box and the 9 cross products of them.
            var cornersA = Corners(bbox);
            var cornersB = Corners(other);
            var axesA = Axes(bbox);
            var axesB = Axes(other);

            var axes = axesA
                .Concat(axesB)
                .Concat(axesA.SelectMany(a => axesB.Select(b => a.CrossProduct(b))))
                .Where(axis => axis.GetLength() > Tolerance)
                .Select(axis => axis.Normalize());

            foreach (var axis in axes)
            {
                Project(cornersA, axis, out var minA, out var maxA);
                Project(cornersB, axis, out var minB, out var maxB);

                if (maxA < minB - Tolerance || minA > maxB + Tolerance)
                    return false;
            }

            return true;
        }

        // The directions of the edges of the box in model coordinates.
        private static XYZ[] Axes(BoundingBoxXYZ box)
        {
            var transform = box.Transform;
            return [transform.BasisX, transform.BasisY, transform.BasisZ];
        }

        private static void Project(XYZ[] corners, XYZ axis, out double min, out double max)
        {
            min = double.MaxValue;
            max = double.MinValue;
            foreach (var corner in corners)
            {
                var value = corner.DotProduct(axis);
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
        }

        // The 8 corners of the box in model coordinates. Transforming only Min and Max is not enough:
        // after a rotation they are no longer the lowest and the highest corners.
        private static XYZ[] Corners(BoundingBoxXYZ box)
        {
            var min = box.Min;
            var max = box.Max;
            var corners = new[]
            {
                new XYZ(min.X, min.Y, min.Z), new XYZ(max.X, min.Y, min.Z),
                new XYZ(min.X, max.Y, min.Z), new XYZ(max.X, max.Y, min.Z),
                new XYZ(min.X, min.Y, max.Z), new XYZ(max.X, min.Y, max.Z),
                new XYZ(min.X, max.Y, max.Z), new XYZ(max.X, max.Y, max.Z)
            };

            if (!box.Transform.IsIdentity)
            {
                for (var i = 0; i < corners.Length; i++)
                    corners[i] = box.Transform.OfPoint(corners[i]);
            }

            return corners;
        }
    }
}