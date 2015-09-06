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
using XPHttp;
using XPHttp.Serializer;

namespace Brook.ZhiHuRiBao.Pages
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Story> _mainList = new ObservableCollection<Story>();

        public ObservableCollection<Story> MainList {
            get { return _mainList; }
        }

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

        public override void Init()
        {
            RequestMainList(DateTime.Now, false);
        }

        void RequestMainList(DateTime dt, bool isGetMore)
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
                .AddUrlSegements("dt", dt.AddDays(1).ToString("yyyyMMdd"));
            SerializerFactory.ReplaceSerializer(typeof(JsonSerializer), new SimpleJsonSerializer());
            XPHttpClient.DefaultClient.GetAsync("stories/before/{dt}", httpParam, new XPResponseHandler<MainData>()
            {
                OnSuccess = (response, list) => 
                {
                    if (!isGetMore)
                        MainList.Clear();

                    AppendData(list);
                },
                OnFailed = response => { }
            });
        }

        void AppendData(MainData data)
        {
            data.stories.ForEach(o => MainList.Add(o));
        }

        public void RequestMainContent(string id)
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
                .AddUrlSegements("id", id);
            XPHttpClient.DefaultClient.GetAsync("story/{id}", httpParam, new XPResponseHandler<MainContent>()
            {
                OnSuccess = (response, content) =>
                {
                    HtmlSource = Html.Constructor(content);
                },
                OnFailed = response => { }
            });
        }

    }
}
