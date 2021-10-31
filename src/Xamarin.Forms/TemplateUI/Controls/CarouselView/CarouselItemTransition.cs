using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class CarouselItemTransition : ICarouselItemTransition
    {
        protected uint AnimationLength { get; } = 250;
        protected Easing AnimationEasing { get; } = Easing.SinInOut;

        public Task OnSelectionChanging(SelectionChangingArgs args)
        {
            if (args.CurrentView != null && Math.Abs(args.Offset) < args.Parent.Width)
                args.CurrentView.TranslationX = args.Offset;

            var nextTabTranslationX = Math.Sign((int)args.Direction) * args.Parent.Width + args.Offset;

            if (args.NextView != null && Math.Abs(nextTabTranslationX) < args.Parent.Width)
                args.NextView.TranslationX = nextTabTranslationX;

            return Task.FromResult(true);
        }

        public Task OnSelectionChanged(SelectionChangedArgs args)
        {
            if (args.Status == SelectionChanged.Reset)
            {
                var tcs = new TaskCompletionSource<bool>();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Animation resetAnimation = new Animation();

                    var animationPercentLength = AnimationLength;

                    if (args.CurrentView != null)
                        resetAnimation.Add(0, 1, new Animation(v => args.CurrentView.TranslationX = v, args.CurrentView.TranslationX, 0));

                    if (args.NextView != null)
                    {
                        resetAnimation.Add(0, 1, new Animation(v => args.NextView.TranslationX = v, args.NextView.TranslationX, Math.Sign((int)args.Direction) * args.Parent.Width));
                        animationPercentLength = (uint)(AnimationLength * (args.Parent.Width - Math.Abs(args.NextView.TranslationX)) / args.Parent.Width);
                    }

                    resetAnimation.Commit(args.Parent, nameof(OnSelectionChanged), length: animationPercentLength, easing: AnimationEasing,
                        finished: (v, t) => tcs.SetResult(true));
                });

                return tcs.Task;
            }

            if (args.Status == SelectionChanged.Completed)
            {
                var tcs = new TaskCompletionSource<bool>();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Animation completeAnimation = new Animation();

                    var animationPercentLength = AnimationLength;

                    if (args.CurrentView != null)
                    {
                        completeAnimation.Add(0, 1, new Animation(v => args.CurrentView.TranslationX = v, args.CurrentView.TranslationX, -Math.Sign((int)args.Direction) * args.Parent.Width));
                        animationPercentLength = (uint)(AnimationLength * (args.Parent.Width - Math.Abs(args.CurrentView.TranslationX)) / args.Parent.Width);
                    }

                    if (args.NextView != null)
                        completeAnimation.Add(0, 1, new Animation(v => args.NextView.TranslationX = v, args.NextView.TranslationX, 0));

                    completeAnimation.Commit(args.Parent, nameof(OnSelectionChanged), length: animationPercentLength, easing: AnimationEasing,
                        finished: (v, t) => tcs.SetResult(true));
                });

                return tcs.Task;
            }

            return Task.FromResult(true);
        }
    }
}