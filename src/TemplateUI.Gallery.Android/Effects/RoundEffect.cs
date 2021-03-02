using System;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(TemplateUI.Gallery.Droid.Effects.RoundEffect), nameof(TemplateUI.Gallery.Droid.Effects.RoundEffect))]
namespace TemplateUI.Gallery.Droid.Effects
{
    public class RoundEffect : PlatformEffect
    {
        ViewOutlineProvider originalProvider;
        Android.Views.View effectTarget;

        protected override void OnAttached()
        {
            try
            {
                effectTarget = Control ?? Container;
                originalProvider = effectTarget.OutlineProvider;
                effectTarget.OutlineProvider = new CornerRadiusOutlineProvider(Element);
                effectTarget.ClipToOutline = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to set corner radius: {ex.Message}");
            }
        }

        protected override void OnDetached()
        {
            if (effectTarget != null)
            {
                effectTarget.OutlineProvider = originalProvider;
                effectTarget.ClipToOutline = false;
            }
        }
    }
}