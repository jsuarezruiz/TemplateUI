using SkiaSharp;
using Xamarin.Forms.Shapes;

namespace TemplateUI.Skia.Extensions
{
    public static class TransformExtensions
    {
        public static SKMatrix ToSKMatrix(this Transform transform)
        {
            SKMatrix skMatrix = SKMatrix.CreateIdentity();

            if (transform == null)
                return skMatrix;

            Matrix matrix = transform.Value;

            skMatrix.Values = new float[] {
                (float)matrix.M11,
                (float)matrix.M21,
                matrix.OffsetX.ToScaledPixel(),
                (float)matrix.M12,
                (float)matrix.M22,
                matrix.OffsetY.ToScaledPixel(),
                0,
                0,
                1 };

            return skMatrix;
        }
    }
}