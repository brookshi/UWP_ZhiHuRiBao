using Brook.ZhiHuRiBao.Authorization;
using Brook.ZhiHuRiBao.Events;
using Brook.ZhiHuRiBao.Models;
using LLQ;
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
using XP;

namespace Brook.ZhiHuRiBao.Elements
{
    public sealed partial class CommentToolBar : UserControl
    {
        const string Normal_State = "Normal";
        const string Owner_State = "Owner";

        public CommentToolBar()
        {
            this.InitializeComponent();
        }

        public bool IsOwner
        {
            get { return (bool)GetValue(IsOwnerProperty); }
            set { SetValue(IsOwnerProperty, value); }
        }
        public static readonly DependencyProperty IsOwnerProperty =
            DependencyProperty.Register("IsOwner", typeof(bool), typeof(CommentToolBar), new PropertyMetadata(false, (s,d)=>
            {
                var toolBar = (CommentToolBar)s;
                VisualStateManager.GoToState(toolBar, toolBar.IsOwner ? Owner_State : Normal_State, false);
            }));

        public int CommentLikeCount
        {
            get { return (int)GetValue(CommentLikeCountProperty); }
            set { SetValue(CommentLikeCountProperty, value); }
        }
        public static readonly DependencyProperty CommentLikeCountProperty =
            DependencyProperty.Register("CommentLikeCount", typeof(int), typeof(CommentToolBar), new PropertyMetadata(0));

        public bool IsLikeComment
        {
            get { return (bool)GetValue(IsLikeCommentProperty); }
            set { SetValue(IsLikeCommentProperty, value); }
        }
        public static readonly DependencyProperty IsLikeCommentProperty =
            DependencyProperty.Register("IsLikeComment", typeof(bool), typeof(CommentToolBar), new PropertyMetadata(false));

        private void LikeComment(object sender, ToggleEventArgs e)
        {
            if (!AuthorizationHelper.IsLogin)
            {
                e.IsCancel = true;
                return;
            }

            CommentLikeCount = CommentLikeCount + (e.IsChecked ? 1 : -1);
            LLQNotifier.Default.Notify(new CommentEvent() { Type = CommentEventType.Like, Comment = (Comment)DataContext, IsLike = e.IsChecked });
        }

        private void ReplyComment()
        {
            if (!AuthorizationHelper.IsLogin)
            {
                return;
            }

            LLQNotifier.Default.Notify(new CommentEvent() { Type = CommentEventType.Reply, Comment = (Comment)DataContext });
        }

        private void DelComment()
        {
            if (!AuthorizationHelper.IsLogin)
            {
                return;
            }

            LLQNotifier.Default.Notify(new CommentEvent() { Type = CommentEventType.Delete, Comment = (Comment)DataContext });
        }
    }
}
