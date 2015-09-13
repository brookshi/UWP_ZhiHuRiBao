#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace XamlListView.Samples.SimpleListViewSample
{
    public class MaterialDesignRefreshAction : IRefreshAction
    {
        TextBlock _text = new TextBlock();
        public FrameworkElement RefreshMark
        {
            get
            {
                return _text;
            }
        }

        public void OnInit()
        {
            _text.Text = "123";
            _text.Foreground = new SolidColorBrush(Colors.Red);
            _text.HorizontalAlignment = HorizontalAlignment.Center;
            _text.Margin = new Thickness(0, -30, 0, 0);
            Canvas.SetZIndex(_text, 100);
        }

        public void OnPull(double currentY, double originY)
        {
            _text.Margin = new Thickness(0, currentY - originY - 30, 0, 0);
            _text.Text = "pull to refresh";
        }

        public void OnRefresh(double currentY, double originY)
        {
            _text.Text = "refreshing";
        }
    }

    public class RefreshLoadPanel : Grid
    {
        public RefreshLoadPanel()
        {
            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            ManipulationStarted += RefreshLoadPanel_ManipulationStarted;
            ManipulationDelta += RefreshLoadPanel_ManipulationDelta;
            ManipulationCompleted += RefreshLoadPanel_ManipulationCompleted;
            ManipulationInertiaStarting += RefreshLoadPanel_ManipulationInertiaStarting;
            ManipulationStarting += RefreshLoadPanel_ManipulationStarting;

            PointerPressed += RefreshLoadPanel_PointerPressed;
            PointerMoved += RefreshLoadPanel_PointerMoved;
            PointerReleased += RefreshLoadPanel_PointerReleased;

            this.Tapped += RefreshLoadPanel_Tapped;

            Loaded += RefreshLoadPanel_Loaded;
        }

        private void RefreshLoadPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("taped");
        }

        private void RefreshLoadPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = GetScrollViewer(this);
            if(RefreshAction != null)
            {
                Children.Add(RefreshAction.RefreshMark);
                RefreshAction.OnInit();
            }
        }

        public IRefreshAction RefreshAction
        {
            get { return (IRefreshAction)GetValue(RefreshActionProperty); }
            set { SetValue(RefreshActionProperty, value); }
        }

        public static readonly DependencyProperty RefreshActionProperty =
            DependencyProperty.Register("RefreshAction", typeof(IRefreshAction), typeof(RefreshLoadPanel), new PropertyMetadata(null));

        ScrollViewer _scrollViewer;

        public static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
                return obj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var view = GetScrollViewer(VisualTreeHelper.GetChild(obj, i));
                if (view != null)
                    return view;
            }

            return null;
        }

        private bool _isPressed = false;
        double _originY = 0, _prevY = 0;
        bool _isRefreshing = false;

        private void RefreshLoadPanel_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _isPressed = false;
        }

        private void RefreshLoadPanel_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TouchMove(GetCurrentY(e));
        }

        private void TouchMove(double currentY)
        {
            if (_isPressed && !_isRefreshing)
            {
                var diff = currentY - _originY;
                if (diff > 20)
                {
                    if (diff > 100)
                    {
                        _isRefreshing = true;
                        if (RefreshAction != null)
                        {
                            RefreshAction.OnRefresh(currentY, _originY);
                        }
                    }
                    else
                    {
                        if (RefreshAction != null)
                        {
                            RefreshAction.OnPull(currentY, _originY);
                        }
                    }
                }
            }
        }

        private void RefreshLoadPanel_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TouchBegin(GetCurrentY(e));
        }

        void TouchBegin(double currentY)
        {
            _isPressed = true;
            _originY = currentY;
            _prevY = _originY;
        }

        double GetCurrentY(PointerRoutedEventArgs arg)
        {
            return arg.GetCurrentPoint(this).Position.Y;
        }

        private void RefreshLoadPanel_ManipulationStarting(object sender, Windows.UI.Xaml.Input.ManipulationStartingRoutedEventArgs e)
        {
            Debug.WriteLine("### Starting");
        }

        private void RefreshLoadPanel_ManipulationInertiaStarting(object sender, Windows.UI.Xaml.Input.ManipulationInertiaStartingRoutedEventArgs e)
        {
            Debug.WriteLine("### Inertia Starting");
        }

        private void RefreshLoadPanel_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            _isPressed = false;
        }

        private void RefreshLoadPanel_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            TouchMove(e.Delta.Translation.Y);
            Debug.WriteLine("### Delta");
        }

        private void RefreshLoadPanel_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            TouchBegin(e.Position.Y);
            Debug.WriteLine("### Started");
        }
    }
}
