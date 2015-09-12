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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Brook.ZhiHuRiBao.Utils
{
    public abstract class RefreshLoadCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading, ISupportRefresh
    {
        public bool HasMoreItems { get; private set; } = true;
        private bool _isStarted = false;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint unuseCount)
        {
            if (!_isStarted || !NeedRequest())
                return Task.Run(() => { return new LoadMoreItemsResult() { Count = 0 }; }).AsAsyncOperation();

            return AsyncInfo.Run(async c =>
            {
                if (Start != null) Start();

                int count = await LoadData();

                HasMoreItems = count != 0;

                if (Finish != null) Finish();

                return new LoadMoreItemsResult() { Count = (uint)count };
            });
        }

        public void Refresh(bool needClear)
        {
            if (needClear)
                Clear();

            _isStarted = true;
            LoadData();
        }

        protected abstract bool NeedRequest();

        protected abstract Task<int> LoadData();

        public Action Start { get; set; }

        public Action Finish { get; set; }
    }
}
