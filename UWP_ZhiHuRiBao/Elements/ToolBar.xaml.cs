using Brook.ZhiHuRiBao.Authorization;
using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Events;
using LLQ;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XP;

namespace Brook.ZhiHuRiBao.Elements
{
    public sealed partial class ToolBar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ToolBarHost Host { get; set; }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ToolBar), new PropertyMetadata(""));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(ToolBar), new PropertyMetadata(""));


        private string _commentCount = "0";
        public string CommentCount
        {
            get { return _commentCount; }
            set
            {
                if (value != _commentCount)
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
                if (value != _likeCount)
                {
                    _likeCount = value;
                    Notify("LikeCount");
                }
            }
        }

        private bool _isLikeButtonChecked = false;
        public bool IsLikeButtonChecked
        {
            get { return _isLikeButtonChecked; }
            set
            {
                _isLikeButtonChecked = value;
                Notify("IsLikeButtonChecked");
            }
        }

        private bool _isFavoriteButtonChecked = false;
        public bool IsFavoriteButtonChecked
        {
            get { return _isFavoriteButtonChecked; }
            set
            {
                _isFavoriteButtonChecked = value;
                Notify("IsFavoriteButtonChecked");
            }
        }

        public ToolBar()
        {
            this.InitializeComponent();
            Loaded += (s, e) =>
            {
                if (this.Visibility == Visibility.Visible)
                {
                    LLQNotifier.Default.Register(this);
                }
            };
        }

        [SubscriberCallback(typeof(StoryExtraEvent))]
        private void Subscriber(StoryExtraEvent param)
        {
            CommentCount = param.StoryExtraInfo.comments.ToString();
            LikeCount = param.StoryExtraInfo.popularity.ToString();
            IsLikeButtonChecked = param.StoryExtraInfo.vote_status == 1;
            IsFavoriteButtonChecked = param.StoryExtraInfo.favorite;
        }

        [SubscriberCallback(typeof(OpenNewStoryEvent))]
        public void Reset()
        {
            CommentCount = "0";
            LikeCount = "0";
            IsLikeButtonChecked = false;
            IsFavoriteButtonChecked = false;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            LLQNotifier.Default.Notify(new DefaultEvent() { EventType = EventType.ClickMenu });
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            LLQNotifier.Default.Notify(new DefaultEvent() { EventType = EventType.ClickComment });
        }

        private void LikeStatusChanged(object sender, ToggleEventArgs e)
        {
            if (!AuthorizationHelper.IsLogin)
            {
                e.IsCancel = true;
                return;
            }

            LikeCount = (int.Parse(LikeCount) + (e.IsChecked ? 1 : -1)).ToString();
            LLQNotifier.Default.Notify(new DefaultEvent() { EventType = EventType.ClickLike, IsChecked = e.IsChecked });
        }

        private void FavStatusChanged(object sender, ToggleEventArgs e)
        {
            if (!AuthorizationHelper.IsLogin)
            {
                e.IsCancel = true;
                return;
            }
            LLQNotifier.Default.Notify(new DefaultEvent() { EventType = EventType.ClickFav, IsChecked = e.IsChecked });
        }

        private void Notify(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
