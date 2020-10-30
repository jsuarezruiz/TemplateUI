using System;
namespace TemplateUI.Helpers
{
    public class ColorNumberHelper
    {
        readonly static int maxRGB = 255;
        readonly static int maxHue = 359;
        readonly static int maxSaturation = 100;
        readonly static int maxLuminosity = 100;
        readonly static double maxInterValue = 1.0d;

        private ColorNumberHelper()
        {
            // UTILITY
        }

        public static double FromSourceToTargetRGB(double sourceValue)
        {
            return sourceValue * maxRGB;
        }

        public static double FromSourceToTargetHue(double sourceValue)
        {
            return sourceValue * maxHue;
        }

        public static double FromSourceToTargetSaturation(double sourceValue)
        {
            return sourceValue * maxSaturation;
        }

        public static double FromSourceToTargetLuminosity(double sourceValue)
        {
            return sourceValue * maxLuminosity;
        }

        public static double FromTargetToSourceRGB(double targetValue)
        {
            targetValue = targetValue > maxRGB ? maxRGB : targetValue;
            return maxInterValue / maxRGB * targetValue;
        }

        public static double FromTargetToSourceHue(double targetValue)
        {
            targetValue = targetValue > maxHue ? maxHue : targetValue;
            return maxInterValue / maxHue * targetValue;
        }

        public static double FromTargetToSourceSaturation(double targetValue)
        {
            targetValue = targetValue > maxSaturation ? maxSaturation : targetValue;
            return maxInterValue / maxSaturation * targetValue;
        }

        public static double FromTargetToSourceLuminosity(double targetValue)
        {
            targetValue = targetValue > maxLuminosity ? maxLuminosity : targetValue;
            return maxInterValue / maxLuminosity * targetValue;
        }

        public static double MaxSaturationFromLuminosity(double luminosity)
        {
            if (luminosity <= maxLuminosity / 2)
            {
                return maxSaturation;
            }

            double saturationSubtrahend = (luminosity - maxLuminosity / 2) / (maxLuminosity - maxLuminosity / 2) * maxSaturation;
            return maxSaturation - saturationSubtrahend;
        }

        public static double MaxLuminosityFromSaturation(double saturation)
        {
            return -0.5 * saturation + maxLuminosity;
        }
    }
}
