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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPHttp.Serializer;

namespace Brook.ZhiHuRiBao.Utils
{
    public static class JsonSerializer
    {
        private static SimpleJsonSerializer _serialize = new SimpleJsonSerializer();

        public static string Serialize(object obj)
        {
            return _serialize.Serialize(obj);
        }

        public static T Deserialize<T>(string content)
        {
            return _serialize.Deserialize<T>(content);
        }
    }
}
