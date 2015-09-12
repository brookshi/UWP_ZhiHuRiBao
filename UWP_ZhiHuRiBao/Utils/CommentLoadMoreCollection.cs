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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Utils
{
    public class CommentLoadMoreCollection : RefreshLoadCollection<Comment>
    {
        public string CurrentStoryId { get; set; }

        string _lastRequestFlag;

        string LastCommentId { get { return Count > 0 ? this.Last().id.ToString() : null; } }

        private CommentType _commonType = CommentType.Long;

        protected override Task<int> LoadData()
        {
            RequestComments();
            return Task.Run(() =>
            {
                return Count;
            });
        }

        protected override bool NeedRequest()
        {
            return !string.IsNullOrEmpty(CurrentStoryId) && _lastRequestFlag != _commonType.ToString() + LastCommentId;
        }

        public void Refresh(string storyId)
        {
            CurrentStoryId = storyId;

            Reset();
            Refresh();
        }

        public void Reset()
        {
            _commonType = CommentType.Long;
            _lastRequestFlag = null;
        }

        void RequestComments()
        {
            if (string.IsNullOrEmpty(CurrentStoryId))
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
            longComment.comments.ForEach(o => Add(o));

            if (longComment.comments.Count == 0)
                RequestShortComments();
        }

        async void RequestShortComments()
        {
            var shortComment = await DataRequester.RequestShortComment(CurrentStoryId, _commonType == CommentType.Long ? null : LastCommentId);
            _commonType = CommentType.Short;
            shortComment.comments.ForEach(o => Add(o));
        }
    }
}
