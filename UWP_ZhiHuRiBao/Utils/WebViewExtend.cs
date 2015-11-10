using Brook.ZhiHuRiBao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Brook.ZhiHuRiBao.Utils
{
    public class WebViewExtend : DependencyObject
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(MainContent), typeof(WebViewExtend), new PropertyMetadata(null, LoadHtmlSource));

        private static void LoadHtmlSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webView = d as WebView;
            if (webView == null)
                return;

            var mainHtmlContent = e.NewValue as MainContent;
            if (mainHtmlContent == null)
                return;

            if (!string.IsNullOrEmpty(mainHtmlContent.body))
            {
                webView.NavigateToString(mainHtmlContent.body);
            }
            else if(!string.IsNullOrEmpty(mainHtmlContent.share_url))
            {
                webView.Navigate(new Uri(mainHtmlContent.share_url));
            }
        }

        public static void SetContent(DependencyObject obj, string value)
        {
            obj.SetValue(ContentProperty, value);
        }

        public static string GetContent(DependencyObject obj)
        {
            return (string)obj.GetValue(ContentProperty);
        }
    }
}
