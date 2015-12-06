using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Utils;
using Brook.ZhiHuRiBao.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class CommentPage : Page
    {
        public CommentViewModel VM { get { return DataContext as CommentViewModel; } }

        public Visibility ToolBarVisibility { get { return Config.UIStatus == AppUIStatus.List ? Visibility.Visible : Visibility.Collapsed; } }

        private bool _isLoadComplete = false;

        public CommentPage()
        {
            this.InitializeComponent();

            CommentListView.Refresh = RefreshCommentList;
            CommentListView.LoadMore = LoadMoreComments;
        }

        private async void RefreshCommentList()
        {
            Debug.WriteLine("refresh comment start");
            await VM.RequestComments(false);
            CommentListView.SetRefresh(false);
            Debug.WriteLine("refresh comment end");
        }

        private async void LoadMoreComments()
        {
            Debug.WriteLine("load more comment start");
            if (_isLoadComplete)
            {
                CommentListView.FinishLoadingMore();
                Debug.WriteLine("load more comment end");
                return;
            }

            var preCount = VM.CurrentCommentCount;
            await VM.RequestComments(true);
            CommentListView.FinishLoadingMore();
            _isLoadComplete = preCount == VM.CurrentCommentCount;
            Debug.WriteLine("load more comment end");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(ViewModelBase.CurrentStoryId))
            {
                CommentListView.SetRefresh(true);
            }
        }

        private async void SendComment()
        {
            if (string.IsNullOrEmpty(VM.CommentContent))
                return;

            await VM.SendComment();
            VM.CommentContent = "";
            CommentListView.SetRefresh(true);
        }
    }
}
