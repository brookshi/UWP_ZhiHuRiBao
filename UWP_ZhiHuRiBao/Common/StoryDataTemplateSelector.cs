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
using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Brook.ZhiHuRiBao.Common
{
    public class StoryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }

        public DataTemplate GroupTemplate { get; set; }

        public DataTemplate NoImageTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var story = item as Story;
            if(story != null && Misc.IsGroupItem(story.type))
            {
                return GroupTemplate;
            }

            if(story.images == null || story.images.Count == 0 || string.IsNullOrEmpty(story.images[0]))
            {
                return NoImageTemplate;
            }

            return ImageTemplate;
        }
    }
}
