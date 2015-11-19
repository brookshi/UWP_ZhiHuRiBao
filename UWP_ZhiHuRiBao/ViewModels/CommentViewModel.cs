using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        private CommentType _currCommentType = CommentType.Long;

        public string CurrentStoryId { get; set; }

        private readonly ObservableCollectionExtended<GroupComments> _commentList = new ObservableCollectionExtended<GroupComments>();

        public ObservableCollectionExtended<GroupComments> CommentList { get { return _commentList; } }

        public string LastCommentId
        {
            get { return CommentList.Last().LastOrDefault()?.id.ToString() ?? null; }
        }

        public string Title
        {
            get { return StringUtil.GetString("CommentTitle"); }
        }

        public async Task RequestComments(bool isLoadingMore)
        {
            if (!isLoadingMore)
            {
                ResetComments();
                await InitCommentInfo();
            }

            if (_currCommentType == CommentType.Long)
            {
                await RequestLongComments(isLoadingMore);
            }
            else if (_currCommentType == CommentType.Short)
            {
                await RequestShortComments();
            }
        }

        private async Task InitCommentInfo()
        {
            var commentInfo = await DataRequester.RequestCommentInfo(CurrentStoryId);
            if (commentInfo == null)
                return;

            if (CommentList.Count == 0)
            {
                CommentList.Add(new GroupComments() { GroupName = StringUtil.GetCommentGroupName(CommentType.Long, commentInfo.long_comments.ToString()) });
                CommentList.Add(new GroupComments() { GroupName = StringUtil.GetCommentGroupName(CommentType.Short, commentInfo.short_comments.ToString()) });
            }
        }

        public void ResetComments()
        {
            CommentList.Clear();
            _currCommentType = CommentType.Long;
        }

        private async Task RequestLongComments(bool isLoadingMore)
        {
            var longComment = await DataRequester.RequestLongComment(CurrentStoryId, LastCommentId);
            if (longComment == null)
                return;

            CommentList.First().AddRange(longComment.comments);

            if (longComment == null || longComment.comments.Count < Misc.Page_Count)
            {
                await RequestShortComments();
            }
        }

        private async Task RequestShortComments()
        {
            if (_currCommentType == CommentType.Long)
            {
                _currCommentType = CommentType.Short;
            }
            var shortComment = await DataRequester.RequestShortComment(CurrentStoryId, _currCommentType == CommentType.Long ? null : LastCommentId);
            if (shortComment == null)
                return;

            CommentList.Last().AddRange(shortComment.comments);
        }
    }
}
