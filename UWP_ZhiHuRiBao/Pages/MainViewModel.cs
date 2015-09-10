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
using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using XPHttp;
using XPHttp.Serializer;

namespace Brook.ZhiHuRiBao.Pages
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentDate;

        private int _currentCommentPage = 0;

        private int _currentMainListPage = 0;

        private readonly ObservableCollection<Story> _mainList = new ObservableCollection<Story>();

        public ObservableCollection<Story> MainList { get { return _mainList; } }

        private readonly ObservableCollection<Comment> _commentList = new ObservableCollection<Comment>();

        public ObservableCollection<Comment> CommentList { get { return _commentList; } }

        private string LastCommentId { get { return CommentList.Count > 0 ? CommentList.Last().id.ToString() : null; } }

        private string _htmlSource = string.Empty;
        public string HtmlSource
        {
            get { return _htmlSource; }
            set
            {
                if(value !=  _htmlSource)
                {
                    _htmlSource = value;
                    Notify("HtmlSource");
                }
            }
        }

        private int _mode = 0;
        public int Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public override void Init()
        {
            RequestMainList(false);
        }

        void Reset()
        {
            _currentDate = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            MainList.Clear();
        }

        void RequestMainList(bool isGetMore)
        {
            DataRequester.RequestStories(_currentDate, data =>
            {
                if (!isGetMore)
                    Reset();

                _currentDate = data.date;
                AppendData(data);
            });
        }

        void AppendData(MainData data)
        {
            data.stories.ForEach(o => MainList.Add(o));
        }

        public void RequestMainContent(string id)
        {
            DataRequester.RequestStoryContent(id, content => { HtmlSource = Html.Constructor(content); });
        }

        public void RequestComments(string id, bool isGetMore)
        {
            string before = null;

            if (!isGetMore)
                CommentList.Clear();
            else
                before = LastCommentId;

            RequestLongComments(id, before);
        }

        void RequestLongComments(string id, string before)
        {
            DataRequester.RequestLongComment(id, before, obj =>
            {
                obj.comments.ForEach(o => CommentList.Add(o));
                if (CommentList.Count < Config.GetMaxRowCountForMainList())
                {
                    if (obj.comments.Count != 0)
                        RequestLongComments(id, LastCommentId);
                    else
                        RequestShortComments(id, LastCommentId);
                }
            });
        }

        void RequestShortComments(string id, string before)
        {
            DataRequester.RequestShortComment(id, before, obj =>
            {
                obj.comments.ForEach(o => CommentList.Add(o));
                if (CommentList.Count < Config.GetMaxRowCountForMainList() && obj.comments.Count != 0)
                {
                    RequestShortComments(id, CommentList.Last().id.ToString());
                }
            });
        }

    }
}
