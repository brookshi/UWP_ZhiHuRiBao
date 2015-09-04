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

namespace Brook.ZhiHuRiBao.Pages
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ObservableCollection<MainItem> _mainList = new ObservableCollection<MainItem>();

        public ObservableCollection<MainItem> MainList {
            get { return _mainList; }
        }

        public override void Init()
        {
            
        }

        void GetDataList(DateTime dt)
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
                .AddUrlSegements("dt", dt.AddDays(1).ToString("yyyyMMdd"));
            XPHttpClient.DefaultClient.GetAsync("stories/before/{dt}", httpParam, new XPResponseHandler<dynamic>()
            {
                OnSuccess = (response, data) => { }
            });
        }
    }
}
