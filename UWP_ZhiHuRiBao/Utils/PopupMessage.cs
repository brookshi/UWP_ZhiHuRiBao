using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Brook.ZhiHuRiBao.Utils
{
    public class PopupMessage
    {
        private static Popup _popup = new Popup();

        private static TextBlock _msgText = new TextBlock();

        private readonly static Queue<string> _msgQueue = new Queue<string>();

        private static int _popupState = 0;

        static PopupMessage()
        {
            _msgText.Padding = new Thickness(20, 10, 20, 10);
            _msgText.Foreground = new SolidColorBrush(Colors.White);
            Border border = new Border();
            border.Background = new SolidColorBrush(Colors.Black);
            border.Child = _msgText;
            _popup.Child = border;
            UpdatePosition();
            _popup.RenderTransform = new TranslateTransform();
            _popup.IsOpen = true;
        }

        public static void DisplayMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            lock(_msgQueue)
            {
                if (!_msgQueue.Contains(msg))
                    _msgQueue.Enqueue(msg);
            }

            Display();
        }

        public static void Display()
        {
            if (_msgQueue.Count == 0)
                return;

            if (Interlocked.CompareExchange(ref _popupState, 1, 0) == 1)
                return;

            string msg;
            lock(_msgQueue)
            {
                msg = _msgQueue.Dequeue();
            }

            _msgText.Text = msg;


        }

        private static void PopupWithAnim()
        {
            Storyboard story = new Storyboard();
            DoubleAnimation popupAnim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                To = -300,
                EasingFunction = new BounceEase()
            };
            Storyboard.SetTarget(popupAnim, _popup.RenderTransform);
            Storyboard.SetTargetProperty(popupAnim, "Y");
            story.Begin();
        }

        private static void UpdatePosition()
        {
            var window = Window.Current.CoreWindow;
            _popup.HorizontalOffset = window.Bounds.Width / 2 - _popup.Width / 2;
            _popup.VerticalOffset = window.Bounds.Height + _popup.Height / 2 + 10;
        }
    }
}
