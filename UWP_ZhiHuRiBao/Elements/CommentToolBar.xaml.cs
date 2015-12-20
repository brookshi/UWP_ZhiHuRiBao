using Brook.ZhiHuRiBao.Authorization;
using Brook.ZhiHuRiBao.Events;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Utils;
using LLQ;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XP;

namespace Brook.ZhiHuRiBao.Elements
{
    public sealed partial class CommentToolBar : UserControl
    {
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
                ((CommentToolBar)s).ResetCtrlVisible();
            }));

        private void ResetCtrlVisible()
        {
            CommentLike.Visibility = IsOwner ? Visibility.Collapsed : Visibility.Visible;
            CommentReply.Visibility = IsOwner ? Visibility.Collapsed : Visibility.Visible;
            CommentDel.Visibility = IsOwner ? Visibility.Visible : Visibility.Collapsed;
        }

        public int CommentLikeCount
        {
            get { return (int)GetValue(CommentLikeCountProperty); }
            set { SetValue(CommentLikeCountProperty, value); }
        }
        public static readonly DependencyProperty CommentLikeCountProperty =
            DependencyProperty.Register("CommentLikeCount", typeof(int), typeof(CommentToolBar), new PropertyMetadata(0));

        public bool? IsLikeComment
        {
            get { return (bool?)GetValue(IsLikeCommentProperty); }
            set { SetValue(IsLikeCommentProperty, value); }
        }
        public static readonly DependencyProperty IsLikeCommentProperty =
            DependencyProperty.Register("IsLikeComment", typeof(bool), typeof(CommentToolBar), new PropertyMetadata(false));

        private void LikeComment(object sender, ToggleEventArgs e)
        {
            if (!AuthorizationHelper.IsLogin)
            {
                PopupMessage.DisplayMessageInRes("NeedLogin");
                e.IsCancel = true;
                return;
            }

            CommentLikeCount = CommentLikeCount + (e.IsChecked ? 1 : -1);
            LLQNotifier.Default.Notify(new CommentEvent() { Type = CommentEventType.Like, Comment = (Comment)DataContext, IsLike = e.IsChecked });
        }

        private void ReplyComment(object sender, RoutedEventArgs e)
        {
            if (!AuthorizationHelper.IsLogin)
            {
                PopupMessage.DisplayMessageInRes("NeedLogin");
                return;
            }

            LLQNotifier.Default.Notify(new CommentEvent() { Type = CommentEventType.Reply, Comment = (Comment)DataContext });
        }

        private void DelComment(object sender, RoutedEventArgs e)
        {
            if (!AuthorizationHelper.IsLogin)
            {
                PopupMessage.DisplayMessageInRes("NeedLogin");
                return;
            }

            LLQNotifier.Default.Notify(new CommentEvent() { Type = CommentEventType.Delete, Comment = (Comment)DataContext });
        }
    }
}
