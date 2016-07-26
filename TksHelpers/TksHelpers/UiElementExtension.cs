using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;

namespace TksHelpers
{
    public static class UiElementExtension
    {
        private static readonly Action EmptyDelegate = delegate { };

        /// <summary>
        /// Permet de rafraichir un élement graphique
        /// </summary>
        /// <param name="uiElement">L'élement graphique à rafraichir</param>
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        /// <summary>
        /// Execute fading using DoubleAnimation
        /// </summary>
        /// <param name="uiElement">this uielement</param>
        /// <param name="opacityValue">Opacity to have</param>
        /// <param name="startTime">Le delai avant le commencement de l'animation</param>
        /// <param name="duration">La lapse de temps durant lequel l'animation se déroule</param>
        public static void Fade(this UIElement uiElement, double opacityValue, double startTime, double duration)
        {
            var animation = new DoubleAnimation
            {
                To = opacityValue,
                BeginTime = TimeSpan.FromSeconds(startTime),
                Duration = TimeSpan.FromSeconds(duration),
                FillBehavior = FillBehavior.Stop
            };
            animation.Completed += (s, a) => uiElement.Opacity = opacityValue;
            uiElement.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private static List<FrameworkElement> _slidingItems;

        public static void Slide(this FrameworkElement uiElement, uint offset, double startTime, double duration, OffsetDirection direction = OffsetDirection.Left, Action OnCompleteAction = null)
        {
            if(_slidingItems == null)
                _slidingItems = new List<FrameworkElement>();
            if(_slidingItems.Contains(uiElement))
                return;
            _slidingItems.Add(uiElement);
            var off = (long) offset;
            if (direction == OffsetDirection.Right)
                off *= -1;
            var thick = new Thickness(uiElement.Margin.Left - off, uiElement.Margin.Top, uiElement.Margin.Right + off, uiElement.Margin.Bottom);
            var thicknessAnim = new ThicknessAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(startTime),
                Duration = TimeSpan.FromSeconds(duration),
                FillBehavior = FillBehavior.Stop,
                To = thick
            };
            thicknessAnim.Completed += (sender, args) => { uiElement.Margin = thick;
                                                             _slidingItems.Remove(uiElement);
                                                             OnCompleteAction?.Invoke();
            };
            uiElement.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnim);
        }

        public enum OffsetDirection
        {
            Left,
            Right
        }
    }
}
