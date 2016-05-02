using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Brook.ZhiHuRiBao.Elements
{
    [ContentProperty(Name = "Content")]
    public sealed class CustomMenuFlyoutItem : MenuFlyoutItem
    {
        public CustomMenuFlyoutItem()
        {
            this.DefaultStyleKey = typeof(CustomMenuFlyoutItem);
        }

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(CustomMenuFlyoutItem), new PropertyMetadata(null));


    }
}
