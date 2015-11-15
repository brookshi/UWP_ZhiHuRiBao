using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class ToolBar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCommentButtonToggleMode
        {
            get { return (bool)GetValue(IsCommentButtonToggleModeProperty); }
            set { SetValue(IsCommentButtonToggleModeProperty, value); }
        }
        public static readonly DependencyProperty IsCommentButtonToggleModeProperty =
            DependencyProperty.Register("IsCommentButtonToggleMode", typeof(bool), typeof(ToolBar), new PropertyMetadata(true));

        public bool IsCommentChecked
        {
            get { return (bool)GetValue(IsCommentCheckedProperty); }
            set { SetValue(IsCommentCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCommentCheckedProperty =
            DependencyProperty.Register("IsCommentChecked", typeof(bool), typeof(ToolBar), new PropertyMetadata(false));


        private string _commentCount = "0";
        public string CommentCount
        {
            get { return _commentCount; }
            set
            {
                if(value != _commentCount)
                {
                    _commentCount = value;
                    Notify("CommentCount");
                }
            }
        }

        private string _likeCount = "0";
        public string LikeCount
        {
            get { return _likeCount; }
            set
            {
                if(value != _likeCount)
                {
                    _likeCount = value;
                    Notify("LikeCount");
                }
            }
        }

        public ToolBar()
        {
            this.InitializeComponent();
        }

        private void Notify(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
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
