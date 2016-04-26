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

using Brook.ZhiHuRiBao.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Brook.ZhiHuRiBao.Utils
{
    public static class WebViewUtil
    {
        static WebViewUtil()
        {
            _webViewInstance.ScriptNotify += async (s, e) =>
            {
                string data = e.Value;
                if (!string.IsNullOrEmpty(data) && data.StartsWith(Html.NotifyPrex))
                {
                    await Launcher.LaunchUriAsync(new Uri(data.Substring(Html.NotifyPrex.Length)));
                }
            };
        }

        private readonly static object _parentLocker = new object();

        private readonly static List<Panel> _webViewParents = new List<Panel>();

        public readonly static WebView _webViewInstance = new WebView();

        public static void AddWebViewWithBinding(Panel parent, object source, string path)
        {
            Clear();
            RemoveParent();
            _webViewInstance.SetBinding(WebViewExtend.ContentProperty, new Binding() { Source = source, Path = new PropertyPath(path) });
            lock(_parentLocker)
            {
                parent.Children.Add(_webViewInstance);
                if (!_webViewParents.Contains(parent))
                {
                    _webViewParents.Add(parent);
                }
            }
        }

        public static void Clear()
        {
            _webViewInstance.NavigateToString("");
        }

        public static bool IsParent(Panel panel)
        {
            return panel.Children.Contains(_webViewInstance);
        }

        public static bool HasParent { get { return _webViewInstance.Parent is Panel; } }

        public static void RemoveParent()
        {
            lock(_parentLocker)
            {
                _webViewParents.ForEach(panel =>
                {
                    if (panel.Children.Contains(_webViewInstance))
                        panel.Children.Remove(_webViewInstance);
                });
            }
        }
    }
}
