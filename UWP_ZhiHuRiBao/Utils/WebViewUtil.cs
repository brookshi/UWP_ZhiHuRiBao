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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Brook.ZhiHuRiBao.Utils
{
    public static class WebViewUtil
    {
        private readonly static WebView _webViewInstance = new WebView();

        public static WebView GetWebViewWithBinding(object source, string path)
        {
            _webViewInstance.SetBinding(WebViewExtend.ContentProperty, new Binding() { Source = source, Path = new PropertyPath(path) });
            return _webViewInstance;
        }

        public static void RemoveParent()
        {
            var parent = _webViewInstance.Parent as Panel;
            if (parent == null)
                return;

            parent.Children.Remove(_webViewInstance);
        }
    }
}
