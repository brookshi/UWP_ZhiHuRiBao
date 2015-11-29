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

namespace Brook.ZhiHuRiBao.Common
{
    public class AdaptiveTriggerExtended : StateTriggerBase
    {
        public double MinWindowWidth
        {
            get { return (double)GetValue(MinWindowWidthProperty); }
            set { SetValue(MinWindowWidthProperty, value); }
        }
        public static readonly DependencyProperty MinWindowWidthProperty =
            DependencyProperty.Register("MinWindowWidth", typeof(double), typeof(AdaptiveTriggerExtended), new PropertyMetadata(0));

        public bool ExtraCondition
        {
            get { return (bool)GetValue(ExtraConditionProperty); }
            set { SetValue(ExtraConditionProperty, value); }
        }
        public static readonly DependencyProperty ExtraConditionProperty =
            DependencyProperty.Register("ExtraCondition", typeof(bool), typeof(AdaptiveTriggerExtended), new PropertyMetadata(true));

        private FrameworkElement _targetElement;
        public FrameworkElement TargetElement
        {
            get { return _targetElement; }
            set
            {
                _targetElement = value;
                _targetElement.SizeChanged += (s, e) => SetActive(ExtraCondition && e.NewSize.Width >= MinWindowWidth);
            }
        }
    }
}
