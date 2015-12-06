using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Events;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Utils;
using LLQ;
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

        private bool _isReplingTo = false;
        public bool IsReplingTo
        {
            get { return _isReplingTo; }
            set
            {
                if(value != _isReplingTo)
                {
                    _isReplingTo = value;
                    Notify("IsReplingTo");
                }
            }
        }

        private string _replyTip;
        public string ReplyTip
        {
            get { return _replyTip; }
            set
            {
                if(value != _replyTip)
                {
                    _replyTip = value;
                    Notify("ReplyTip");
                }
            }
        }

        private string _commentContent;
        public string CommentContent
        {
            get { return _commentContent; }
            set
            {
                if (value != _commentContent)
                {
                    _commentContent = value;
                    Notify("CommentContent");
                }
            }
        }

        public int? ReplyCommentId { get; set; } = null;

        public CommentViewModel()
        {
            LLQNotifier.Default.Register(this);
        }

        static CommentViewModel()
        {
            LLQNotifier.Default.Register(CommentExclusiveSubscriber.Instance);
        }

        public async Task RequestComments(bool isLoadingMore)
        {
            if (!isLoadingMore)
            {
                ResetComments();
                InitCommentInfo();
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

        private void InitCommentInfo()
        {
            if (CommentList.Count == 0)
            {
                CommentList.Add(new GroupComments() { GroupName = StringUtil.GetCommentGroupName(CommentType.Long, CurrentStoryExtraInfo.long_comments.ToString()) });
                CommentList.Add(new GroupComments() { GroupName = StringUtil.GetCommentGroupName(CommentType.Short, CurrentStoryExtraInfo.short_comments.ToString()) });
            }
        }

        public void ResetComments()
        {
            CommentList.Clear();
            _currCommentType = CommentType.Long;
        }

        public int CurrentCommentCount { get { return CommentList.Count > 1 ? CommentList[0].Count + CommentList[1].Count : 0; } }

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
            if (shortComment != null)
            {
                CommentList.Last().AddRange(shortComment.comments);
            }
        }

        public async Task SendComment()
        {
            await DataRequester.SendComment(CurrentStoryId, CommentContent, ReplyCommentId);
        }

        public void CancelReply()
        {
            IsReplingTo = false;
        }

        [SubscriberCallback(typeof(StoryExtraEvent))]
        private void StoryExtraSubscriber(StoryExtraEvent param)
        {
            if(CommentList.Count > 1)
            {
                CommentList[0].GroupName = StringUtil.GetCommentGroupName(CommentType.Long, CurrentStoryExtraInfo.long_comments.ToString());
                CommentList[1].GroupName = StringUtil.GetCommentGroupName(CommentType.Short, CurrentStoryExtraInfo.short_comments.ToString());
            }
        }

        [SubscriberCallback(typeof(CommentEvent))]
        private void CommentSubscriber(CommentEvent param)
        {
            switch(param.Type)
            {
                case CommentEventType.Reply:
                    ReplyCommentId = param.Comment.id;
                    IsReplingTo = true;
                    ReplyTip = string.Format(StringUtil.GetString("ReplyTip"), param.Comment.author);
                    break;
                case CommentEventType.Delete:
                    if(CommentList.Count > 1)
                    {
                        var comment = CommentList[0].SingleOrDefault(o => o.id == param.Comment.id);
                        if(comment != null)
                        {
                            CommentList[0].Remove(comment);
                            return;
                        }

                        comment = CommentList[1].SingleOrDefault(o => o.id == param.Comment.id);
                        if (comment != null)
                        {
                            CommentList[1].Remove(comment);
                            return;
                        }
                    }
                    break;
            }
        }

        internal class CommentExclusiveSubscriber
        {
            internal static CommentExclusiveSubscriber Instance = new CommentExclusiveSubscriber();

            [SubscriberCallback(typeof(CommentEvent))]
            private void Subscriber(CommentEvent param)
            {
                var commentId = param.Comment.id.ToString();
                switch (param.Type)
                {
                    case CommentEventType.Delete:
                        DataRequester.DeleteComment(commentId);
                        break;
                    case CommentEventType.Like:
                        if (param.IsLike)
                        {
                            DataRequester.LikeComment(commentId);
                        }
                        else
                        {
                            DataRequester.UnlikeComment(commentId);
                        }
                        break;
                }
            }
        }
    }
}
