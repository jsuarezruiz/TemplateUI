using System.Collections.Generic;
using Xamarin.Forms.Shapes;
using SkiaSharp;
using Xamarin.Forms;
using FormsRectangle = Xamarin.Forms.Rectangle;

namespace TemplateUI.Skia.Extensions
{
    public static class GeometryExtensions
    {
        public static SKPath ToSKPath(this Geometry geometry)
        {
            SKPath path = new SKPath();

            if (geometry is LineGeometry)
            {
                LineGeometry lineGeometry = geometry as LineGeometry;

                path.MoveTo(
                    lineGeometry.StartPoint.X.ToScaledPixel(),
                    lineGeometry.StartPoint.Y.ToScaledPixel());

                path.LineTo(
                    lineGeometry.EndPoint.X.ToScaledPixel(),
                    lineGeometry.EndPoint.Y.ToScaledPixel());
            }
            else if (geometry is RectangleGeometry)
            {
                FormsRectangle rect = (geometry as RectangleGeometry).Rect;

                path.AddRect(new SKRect(
                    rect.Left.ToScaledPixel(),
                    rect.Top.ToScaledPixel(),
                    rect.Right.ToScaledPixel(),
                    rect.Bottom.ToScaledPixel()),
                    SKPathDirection.Clockwise);
            }
            else if (geometry is EllipseGeometry)
            {
                EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;

                path.AddOval(new SKRect(
                    (ellipseGeometry.Center.X - ellipseGeometry.RadiusX).ToScaledPixel(),
                    (ellipseGeometry.Center.Y - ellipseGeometry.RadiusY).ToScaledPixel(),
                    (ellipseGeometry.Center.X + ellipseGeometry.RadiusX).ToScaledPixel(),
                    (ellipseGeometry.Center.Y + ellipseGeometry.RadiusY).ToScaledPixel()),
                    SKPathDirection.Clockwise);
            }
            else if (geometry is GeometryGroup)
            {
                GeometryGroup geometryGroup = geometry as GeometryGroup;

                path.FillType = geometryGroup.FillRule == FillRule.Nonzero ? SKPathFillType.Winding : SKPathFillType.EvenOdd;

                foreach (Geometry child in geometryGroup.Children)
                {
                    SKPath childPath = child.ToSKPath();
                    path.AddPath(childPath);
                }
            }
            else if (geometry is PathGeometry)
            {
                PathGeometry pathGeometry = geometry as PathGeometry;

                path.FillType = pathGeometry.FillRule == FillRule.Nonzero ? SKPathFillType.Winding : SKPathFillType.EvenOdd;

                foreach (PathFigure pathFigure in pathGeometry.Figures)
                {
                    path.MoveTo(
                        pathFigure.StartPoint.X.ToScaledPixel(),
                        pathFigure.StartPoint.Y.ToScaledPixel());

                    Point lastPoint = pathFigure.StartPoint;

                    foreach (PathSegment pathSegment in pathFigure.Segments)
                    {
                        // LineSegment
                        if (pathSegment is LineSegment)
                        {
                            LineSegment lineSegment = pathSegment as LineSegment;

                            path.LineTo(
                                lineSegment.Point.X.ToScaledPixel(),
                                lineSegment.Point.Y.ToScaledPixel());
                            lastPoint = lineSegment.Point;
                        }
                        // PolylineSegment
                        else if (pathSegment is PolyLineSegment)
                        {
                            PolyLineSegment polylineSegment = pathSegment as PolyLineSegment;
                            PointCollection points = polylineSegment.Points;

                            for (int i = 0; i < points.Count; i++)
                            {
                                path.LineTo(
                                    points[i].X.ToScaledPixel(),
                                    points[i].Y.ToScaledPixel());
                            }
                            lastPoint = points[points.Count - 1];
                        }
                        // BezierSegment
                        else if (pathSegment is BezierSegment)
                        {
                            BezierSegment bezierSegment = pathSegment as BezierSegment;

                            path.CubicTo(
                                bezierSegment.Point1.X.ToScaledPixel(), bezierSegment.Point1.Y.ToScaledPixel(),
                                bezierSegment.Point2.X.ToScaledPixel(), bezierSegment.Point2.Y.ToScaledPixel(),
                                bezierSegment.Point3.X.ToScaledPixel(), bezierSegment.Point3.Y.ToScaledPixel());

                            lastPoint = bezierSegment.Point3;
                        }
                        // PolyBezierSegment
                        else if (pathSegment is PolyBezierSegment)
                        {
                            PolyBezierSegment polyBezierSegment = pathSegment as PolyBezierSegment;
                            PointCollection points = polyBezierSegment.Points;

                            for (int i = 0; i < points.Count; i += 3)
                            {
                                path.CubicTo(
                                    points[i + 0].X.ToScaledPixel(), points[i + 0].Y.ToScaledPixel(),
                                    points[i + 1].X.ToScaledPixel(), points[i + 1].Y.ToScaledPixel(),
                                    points[i + 2].X.ToScaledPixel(), points[i + 2].Y.ToScaledPixel());
                            }

                            lastPoint = points[points.Count - 1];
                        }
                        // QuadraticBezierSegment
                        else if (pathSegment is QuadraticBezierSegment)
                        {
                            QuadraticBezierSegment bezierSegment = pathSegment as QuadraticBezierSegment;

                            path.QuadTo(
                                bezierSegment.Point1.X.ToScaledPixel(), bezierSegment.Point1.Y.ToScaledPixel(),
                                bezierSegment.Point2.X.ToScaledPixel(), bezierSegment.Point2.Y.ToScaledPixel());

                            lastPoint = bezierSegment.Point2;
                        }
                        // PolyQuadraticBezierSegment
                        else if (pathSegment is PolyQuadraticBezierSegment)
                        {
                            PolyQuadraticBezierSegment polyBezierSegment = pathSegment as PolyQuadraticBezierSegment;
                            PointCollection points = polyBezierSegment.Points;

                            for (int i = 0; i < points.Count; i += 2)
                            {
                                path.QuadTo(
                                    points[i + 0].X.ToScaledPixel(), points[i + 0].Y.ToScaledPixel(),
                                    points[i + 1].X.ToScaledPixel(), points[i + 1].Y.ToScaledPixel());
                            }

                            lastPoint = points[points.Count - 1];
                        }
                        // ArcSegment
                        else if (pathSegment is ArcSegment)
                        {
                            ArcSegment arcSegment = pathSegment as ArcSegment;

                            List<Point> points = new List<Point>();

                            GeometryHelper.FlattenArc(points,
                                lastPoint,
                                arcSegment.Point,
                                arcSegment.Size.Width,
                                arcSegment.Size.Height,
                                arcSegment.RotationAngle,
                                arcSegment.IsLargeArc,
                                arcSegment.SweepDirection == SweepDirection.CounterClockwise,
                                1);

                            for (int i = 0; i < points.Count; i++)
                            {
                                path.LineTo(
                                    points[i].X.ToScaledPixel(),
                                    points[i].Y.ToScaledPixel());
                            }

                            if (points.Count > 0)
                                lastPoint = points[points.Count - 1];
                        }
                    }

                    if (pathFigure.IsClosed)
                        path.Close();
                }
            }

            return path;
        }
    }
}