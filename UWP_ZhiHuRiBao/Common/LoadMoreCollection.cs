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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Brook.ZhiHuRiBao.Common
{
    public abstract class LoadMoreCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public bool HasMoreItems { get; set; } = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint unuseCount)
        {
            return AsyncInfo.Run(async c =>
            {
                uint count = 0;
                T obj = await LoadMoreData(out count);
                HasMoreItems = count != 0;

                return new LoadMoreItemsResult() { Count = count };
            });
        }

        protected abstract Task<T> LoadMoreData(out uint count);
    }
}
