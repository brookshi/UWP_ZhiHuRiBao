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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Brook.ZhiHuRiBao.Elements
{
    public sealed class FlipViewIndicator : ListBox
    {
        DispatcherTimer _timer;

        public FlipViewIndicator()
        {
            this.DefaultStyleKey = typeof(FlipViewIndicator);
        }

        public FlipView FlipView
        {
            get { return (FlipView)GetValue(FlipViewProperty); }
            set { SetValue(FlipViewProperty, value); }
        }
        public static readonly DependencyProperty FlipViewProperty =
            DependencyProperty.Register("FlipView", typeof(FlipView), typeof(FlipViewIndicator), new PropertyMetadata(null, (obj, args) =>
            {
                FlipViewIndicator indicator = (FlipViewIndicator)obj;
                FlipView flipView = (FlipView)args.NewValue;

                flipView.SelectionChanged += (s, e) =>
                {
                    indicator.Visibility = flipView.Items.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
                    indicator.ItemsSource = flipView.ItemsSource;
                };

                indicator.ItemsSource = flipView.ItemsSource;

                Binding eb = new Binding();
                eb.Mode = BindingMode.TwoWay;
                eb.Source = flipView;
                eb.Path = new PropertyPath("SelectedItem");
                indicator.SetBinding(SelectedItemProperty, eb);
            }));

        public int AutoSwitchDuration
        {
            get { return (int)GetValue(AutoSwitchDurationProperty); }
            set { SetValue(AutoSwitchDurationProperty, value); }
        }
        public static readonly DependencyProperty AutoSwitchDurationProperty =
            DependencyProperty.Register("AutoSwitchDuration", typeof(int), typeof(FlipViewIndicator), new PropertyMetadata(0, (obj, args) =>
            {
                FlipViewIndicator indicator = (FlipViewIndicator)obj;
                indicator.AutoSwitch();
            }));

        private void AutoSwitch()
        {
            if (AutoSwitchDuration <= 0)
                return;

            if(_timer != null)
            {
                _timer.Stop();
                _timer.Interval = TimeSpan.FromMilliseconds(AutoSwitchDuration);
                _timer.Start();
                return;
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(AutoSwitchDuration);
            _timer.Tick += (s, e) =>
            {
                if (FlipView == null)
                    return;

                try
                {
                    if (FlipView.SelectedIndex == FlipView.Items.Count - 1)
                        FlipView.SelectedIndex = 0;
                    else
                        FlipView.SelectedIndex++;
                }
                catch { }
            };
            _timer.Start();
        }
    }
}
