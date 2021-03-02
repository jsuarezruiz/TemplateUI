﻿using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(OpacityGradientLayout), typeof(OpacityGradientLayoutRenderer))]

namespace TemplateUI.Gallery.iOS.Renderers
{
    public class OpacityGradientLayoutRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetNeedsDisplay();
        }

        private CAGradientLayer gradientLayer;

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            OpacityGradientLayout layout = (OpacityGradientLayout)Element;

            if (gradientLayer == null)
            {
                gradientLayer = new CAGradientLayer();
            }

            switch (layout.Mode)
            {
                default:
                case GradientColorStackMode.ToRight:
                    gradientLayer.StartPoint = new CGPoint(0, 0.5);
                    gradientLayer.EndPoint = new CGPoint(1, 0.5);
                    break;
                case GradientColorStackMode.ToLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 0.5);
                    gradientLayer.EndPoint = new CGPoint(0, 0.5);
                    break;
                case GradientColorStackMode.ToTop:
                    gradientLayer.StartPoint = new CGPoint(0.5, 0);
                    gradientLayer.EndPoint = new CGPoint(0.5, 1);
                    break;
                case GradientColorStackMode.ToBottom:
                    gradientLayer.StartPoint = new CGPoint(0.5, 1);
                    gradientLayer.EndPoint = new CGPoint(0.5, 0);
                    break;
                case GradientColorStackMode.ToTopLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 0);
                    gradientLayer.EndPoint = new CGPoint(0, 1);
                    break;
                case GradientColorStackMode.ToTopRight:
                    gradientLayer.StartPoint = new CGPoint(0, 1);
                    gradientLayer.EndPoint = new CGPoint(1, 0);
                    break;
                case GradientColorStackMode.ToBottomLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 1);
                    gradientLayer.EndPoint = new CGPoint(0, 0);
                    break;
                case GradientColorStackMode.ToBottomRight:
                    gradientLayer.StartPoint = new CGPoint(0, 0);
                    gradientLayer.EndPoint = new CGPoint(1, 1);
                    break;
            }

            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] {
                    Color.White.ToCGColor(),
                    layout.SelectedColor.ToCGColor()
                };

            var maskLayer = new CAGradientLayer();
            var lightZero = Color.FromHsla(0d, 1d, 0, 0.0d).ToCGColor();
            var light100 = Color.FromHsla(0d, 1d, 0, 1.0d).ToCGColor();
            CGColor[] maskedColors =
            {
                lightZero,
                light100
            };
            maskLayer.Colors = maskedColors;
            maskLayer.StartPoint = new CGPoint(x: 0.5, y: 0);
            maskLayer.EndPoint = new CGPoint(x: 0.5, y: 1);
            maskLayer.Frame = rect;

            /**
             * LAYER 0: Controls
             * LAYER 1: MaskLayer
             * LAYER 2: GradientLayer
             */
            if (NativeView.Layer.Sublayers.Length == 1)
            {
                NativeView.Layer.InsertSublayer(gradientLayer, 0);
            }

            if (NativeView.Layer.Sublayers.Length == 2)
            {
                NativeView.Layer.InsertSublayer(maskLayer, 1);
            }

            if (NativeView.Layer.Sublayers.Length == 3)
            {
                NativeView.Layer.ReplaceSublayer(NativeView.Layer.Sublayers[1], maskLayer);
            }
        }
    }
}
