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
using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Models
{
    public class Comments
    {
        public List<Comment> comments { get; set; }
    }

    public class GroupComments : ObservableCollectionExtended<Comment>
    {
        private string _groupName = "";
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (value != _groupName)
                {
                    _groupName = value;
                    Notify("GroupName");
                }
            }
        }

        protected void Notify(string property)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(property));
        }
    }

    public class ReplyTo
    {
        public string content { get; set; }
        public int status { get; set; }
        public int id { get; set; }
        public string author { get; set; }
    }

    public class Comment
    {
        public bool own { get; set; }
        public string author { get; set; }
        public string content { get; set; }
        public string avatar { get; set; }
        public int time { get; set; }
        public bool voted { get; set; }
        public int id { get; set; }
        public int likes { get; set; }
        public ReplyTo reply_to { get; set; }
    }
}
