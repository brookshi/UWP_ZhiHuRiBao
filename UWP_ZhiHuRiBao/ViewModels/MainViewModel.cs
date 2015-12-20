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
using System.Linq;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private string _currentDate;

        private readonly ObservableCollectionExtended<Story> _storyDataList = new ObservableCollectionExtended<Story>();

        public ObservableCollectionExtended<Story> StoryDataList { get { return _storyDataList; } }

        private List<TopStory> _topStoryList = new List<TopStory>();

        public List<TopStory> TopStoryList { get { return _topStoryList; } set { if (value != _topStoryList) { _topStoryList = value; Notify("TopStoryList"); } } }

        private List<bool> _indicators = new List<bool>();

        public List<bool> Indicators { get { return _indicators; } set { if (value != _indicators) { _indicators = value; Notify("Indicators"); } } }

        private string _userPhotoUrl = "ms-appx:///Assets/Login.png";
        public string UserPhotoUrl
        {
            get { return _userPhotoUrl; }
            set
            {
                if(value != _userPhotoUrl)
                {
                    _userPhotoUrl = value;
                    Notify("UserPhotoUrl");
                }
            }
        }

        private string _userName = StringUtil.GetString("PleaseLogin");
        public string UserName
        {
            get { return _userName; }
            set
            {
                if(value != _userName)
                {
                    _userName = value;
                    Notify("UserName");
                }
            }
        }

        public override void Init()
        {
            InitCategories();
        }

        public async Task Refresh()
        {
            ResetStorys();
            await RequestMainList(false);

            if (StoryDataList.Count < Misc.Page_Count)
            {
                await LoadMore();
            }
        }

        public async Task LoadMore()
        {
            await RequestMainList(true);
        }

        protected void ResetStorys()
        {
            _currentDate = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            StoryDataList.Clear();
            TopStoryList.Clear();
            Indicators.Clear();
        }

        private async Task RequestMainList(bool isLoadingMore)
        {
            if(CurrentCategoryId == Misc.Default_Category_Id)
            {
                await RequestDefaultCategoryData(isLoadingMore);
            }
            else if(CurrentCategoryId == Misc.Favorite_Category_Id)
            {
                await RequestFavorites(isLoadingMore);
            }
            else
            {
                await RequestMinorCategoryData(isLoadingMore);
            }
        }

        private async Task RequestDefaultCategoryData(bool isLoadingMore)
        {
            MainData storyData = null;

            if (isLoadingMore)
            {
                storyData = await DataRequester.RequestStories(_currentDate);
            }
            else
            {
                ResetStorys();
                storyData = await DataRequester.RequestLatestStories();
                if (storyData != null)
                {
                    TopStoryList = storyData.top_stories;
                    UpdateIndicators(TopStoryList.Count);
                    CurrentStoryId = storyData.stories.First().id.ToString();
                }
            }

            if (storyData == null)
                return;

            _currentDate = storyData.date;

            StoryDataList.Add(new Story() { title = StringUtil.GetStoryGroupName(_currentDate), type = Misc.Group_Name_Type });
            StoryDataList.AddRange(storyData.stories);
        }

        private void UpdateIndicators(int count)
        {
            var indicators = new List<bool>();
            for(int i=0;i< count; i++)
            {
                indicators.Add(true);
            }
            Indicators = indicators;
        }

        private async Task RequestMinorCategoryData(bool isLoadingMore)
        {
            MinorData storyData = null;

            if (isLoadingMore)
            {
                storyData = await DataRequester.RequestCategoryStories(CurrentCategoryId.ToString(), _currentDate);
            }
            else
            {
                ResetStorys();
                storyData = await DataRequester.RequestCategoryLatestStories(CurrentCategoryId.ToString());
                if (storyData != null)
                {
                    var firstStoryId = storyData.stories.First().id;
                    TopStoryList = new List<TopStory>() { new TopStory() { image = storyData.background, id = Misc.Unvalid_Image_Id, title = storyData.description } };
                    CurrentStoryId = firstStoryId.ToString();
                }
            }
            if (storyData == null || storyData.stories.Count == 0)
                return;

            _currentDate = storyData.stories.Last().id.ToString();

            StoryDataList.AddRange(storyData.stories);
        }

        public void LoginSuccess()
        {
            var info = StorageUtil.StorageInfo.ZhiHuAuthoInfo;
            if (info == null)
                return;

            UserPhotoUrl = info.avatar;
            UserName = info.name;
        }
    }
}
