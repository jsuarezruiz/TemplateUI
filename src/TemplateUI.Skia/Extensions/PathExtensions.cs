using SkiaSharp;

namespace TemplateUI.Skia.Extensions
{
    public static class PathExtensions
    {
        public static SKPath ArcTo(this SKPath path, float radius, SKPoint finalPoint)
        {
            path.ArcTo(
                new SKPoint(radius, radius),
                0,
                SKPathArcSize.Small, SKPathDirection.Clockwise,
                finalPoint);

            return path;
        }
    }
}