#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Utils
{
    public class CommentLoadMoreCollection : RefreshLoadCollection<GroupComments>
    {
        public CommentLoadMoreCollection()
        {
            Add(new GroupComments() { GroupName = "长评论" });
            Add(new GroupComments() { GroupName = "短评论" });
        }

        public string CurrentStoryId { get; set; }

        string _lastRequestFlag;

        string LastCommentId
        {
            get
            {
                if (Count == 0)
                    return string.Empty;

                return ShortComments.Count > 0 ? ShortComments.Last().id.ToString() : 
                    (LongComments.Count > 0 ? LongComments.Last().id.ToString() : null);
            }
        }

        private CommentType _commonType = CommentType.Long;

        public ObservableCollection<Comment> LongComments { get { return this[0]; } }

        public ObservableCollection<Comment> ShortComments { get { return this[1]; } }

        protected override Task<int> LoadData()
        {
            RequestComments();
            return Task.Run(() =>
            {
                return LongComments.Count + ShortComments.Count;
            });
        }

        protected override bool NeedRequest()
        {
            return Count > 0 && !string.IsNullOrEmpty(CurrentStoryId) && _lastRequestFlag != _commonType.ToString() + LastCommentId;
        }

        public void Refresh(string storyId)
        {
            CurrentStoryId = storyId;

            Reset();
            Refresh(false);
        }

        public void Reset()
        {
            LongComments.Clear();
            ShortComments.Clear();
            _commonType = CommentType.Long;
            _lastRequestFlag = null;
        }

        public void RequestComments()
        {
            if (!NeedRequest())
                return;

            _lastRequestFlag = _commonType.ToString() + LastCommentId;
            if (_commonType == CommentType.Long)
                RequestLongComments();
            else if (_commonType == CommentType.Short)
                RequestShortComments();
        }

        async void RequestLongComments()
        {
            var longComment = await DataRequester.RequestLongComment(CurrentStoryId, LastCommentId);
            longComment.comments.ForEach(o => { LongComments.Add(o); });

            if (longComment.comments.Count == 0)
                RequestShortComments();
        }

        async void RequestShortComments()
        {
            var shortComment = await DataRequester.RequestShortComment(CurrentStoryId, _commonType == CommentType.Long ? null : LastCommentId);
            _commonType = CommentType.Short;
            shortComment.comments.ForEach(o => { ShortComments.Add(o); });
        }
    }
}
