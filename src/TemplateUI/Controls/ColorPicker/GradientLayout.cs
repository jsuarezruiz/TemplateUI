using System;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class GradientLayout : AbsoluteLayout
    {
        public static readonly BindableProperty ColorsListProperty =
            BindableProperty.Create(nameof(ColorsList), typeof(string), typeof(GradientLayout), "");
        public string ColorsList { get
            {
                return (string)GetValue(ColorsListProperty);
            }
            set
            {
                SetValue(ColorsListProperty, value);
            }
        }

        public Color[] Colors
        {
            get
            {
                string[] hex = ColorsList.Split(',');
                Color[] colors = new Color[hex.Length];

                for (int i = 0; i < hex.Length; i++)
                {
                    colors[i] = Color.FromHex(hex[i].Trim());
                }

                return colors;
            }
        }

        public GradientColorStackMode Mode { get; set; }
    }
}
