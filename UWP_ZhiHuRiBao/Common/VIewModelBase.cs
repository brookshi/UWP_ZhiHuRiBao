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

using Brook.ZhiHuRiBao.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public static string CurrentStoryId { get; set; }

        public static StoryExtraInfo CurrentStoryExtraInfo { get; set; } = new StoryExtraInfo()
        {
            comments = 0,
            favorite = false,
            long_comments = 0,
            normal_comments = 0,
            popularity = 0,
            post_reasons = 0,
            short_comments = 0,
            vote_status = 0,
        };

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public virtual void Init()
        {

        }
    }
}
