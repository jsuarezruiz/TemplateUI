using System;
using Xamarin.Forms;

namespace TemplateUI.Skia.Extensions
{
    public static class UnitExtensions
    {
        public static int ToScaledPixel(this float dp)
        {
            return (int)Math.Round(dp * Device.Info.ScalingFactor);
        }

        public static int ToScaledPixel(this double dp)
        {
            return (int)Math.Round(dp * Device.Info.ScalingFactor);
        }
    }
}