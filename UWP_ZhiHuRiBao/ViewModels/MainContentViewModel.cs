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

using Brook.ZhiHuRiBao.Authorization;
using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Events;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Utils;
using LLQ;

namespace Brook.ZhiHuRiBao.ViewModels
{
    public class MainContentViewModel : ViewModelBase
    {
        private MainContent _mainHtmlContent;
        public MainContent MainHtmlContent
        {
            get { return _mainHtmlContent; }
            set
            {
                if (value != _mainHtmlContent)
                {
                    _mainHtmlContent = value;
                    Notify("MainHtmlContent");
                }
            }
        }

        private bool _isRefreshContent = false;
        public bool IsRefreshContent
        {
            get { return _isRefreshContent; }
            set
            {
                if (value != _isRefreshContent)
                {
                    _isRefreshContent = value;
                    Notify("IsRefreshContent");
                }
            }
        }

        public MainContentViewModel()
        {
            LLQNotifier.Default.Register(this);
        }

        public string Title
        {
            get { return StringUtil.GetString("ContentTitle"); }
        }

        public async void RequestMainContent()
        {
            if (string.IsNullOrEmpty(CurrentStoryId))
                return;

            IsRefreshContent = true;
            var content = await DataRequester.RequestStoryContent(CurrentStoryId);
            if (content != null)
            {
                Html.ArrangeMainContent(content);
                MainHtmlContent = content;
            }
            IsRefreshContent = false;
        }

        public async void RequestStoryExtraInfo()
        {
            CurrentStoryExtraInfo = await DataRequester.RequestStoryExtraInfo(CurrentStoryId);
            if (CurrentStoryExtraInfo == null)
                return;

            LLQNotifier.Default.Notify(new StoryExtraEvent() { StoryExtraInfo = CurrentStoryExtraInfo });
        }

        public void RequestStoryData()
        {
            RequestMainContent();

            RequestStoryExtraInfo();
        }

        [SubscriberCallback(typeof(DefaultEvent))]
        private void Subscriber(DefaultEvent param)
        {
            switch(param.EventType)
            {
                case EventType.ClickFav:
                    DataRequester.SetStoryFavorite(CurrentStoryId, param.IsChecked);
                    break;
                case EventType.ClickLike:
                    DataRequester.SetStoryLike(CurrentStoryId, param.IsChecked);
                    break;

            }
        }
    }
}
