using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Brook.ZhiHuRiBao.Elements
{
    public sealed partial class ToolBar : UserControl
    {
        public bool IsCommentButtonToggleMode
        {
            get { return (bool)GetValue(IsCommentButtonToggleModeProperty); }
            set { SetValue(IsCommentButtonToggleModeProperty, value); }
        }
        public static readonly DependencyProperty IsCommentButtonToggleModeProperty =
            DependencyProperty.Register("IsCommentButtonToggleMode", typeof(bool), typeof(ToolBar), new PropertyMetadata(true));



        public ToolBar()
        {
            this.InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainView.IsPaneOpen = !MainView.IsPaneOpen;
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        { }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        { }

        private void FavButton_Click(object sender, RoutedEventArgs e)
        { }
    }
}
