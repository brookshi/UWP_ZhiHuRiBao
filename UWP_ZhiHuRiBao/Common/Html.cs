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
using System.Text;

namespace Brook.ZhiHuRiBao.Common
{
    public static class Html
    {
        private const string _htmlTemplate = "<html><head><meta charset = \"utf-8\" > {1} {2} </head> {3} </html> ";

        private const string _cssTemplate = "<link rel=\"Stylesheet\" type=\"text/css\" href=\"{0}\" />";

        private const string _jsTemplate = "<script src=\"{0}\" type=\"text/javascript\"></script>";

        public static string Constructor(MainContent content)
        {
            var cssBuilder = new StringBuilder();
            var jsBuilder = new StringBuilder();

            content.css.ForEach(o => cssBuilder.Append(string.Format(_cssTemplate, o)));
            content.js.ForEach(o => jsBuilder.Append(string.Format(_jsTemplate, o)));

            var source = string.Format(_htmlTemplate, content.title, cssBuilder.ToString(), jsBuilder.ToString(), content.body);
            source = source.Replace("<div class=\"img-place-holder\"></div>", "<img height=\"300\" width=\"100%\" src=\"" + content + "\"/>");

            return source;
        }
    }
}
