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

        private readonly CommentLoadMoreCollection _commentList = new CommentLoadMoreCollection();

        public CommentLoadMoreCollection CommentList { get { return _commentList; } }


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
            Reset();
            RequestMainList(false);
        }

        void Reset()
        {
            _currentDate = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            MainList.Clear();
        }

        async void RequestMainList(bool isGetMore)
        {
            var stories = await DataRequester.GetStories(_currentDate);

            if (!isGetMore)
                Reset();

            _currentDate = stories.date;
            AppendData(stories);
        }

        void AppendData(MainData data)
        {
            data.stories.ForEach(o => MainList.Add(o));
        }

        public async void RequestMainContent(string id)
        {
            var content = await DataRequester.RequestStoryContent(id);
            HtmlSource = Html.Constructor(content);
        }

        void RefreshComments(string storyId)
        {

        }
    }
}
