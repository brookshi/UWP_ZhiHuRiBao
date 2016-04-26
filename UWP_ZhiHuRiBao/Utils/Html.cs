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

namespace Brook.ZhiHuRiBao.Utils
{
    public static class Html
    {
        public const string NotifyPrex = "LinkNotify:";

        private const string _htmlTemplate = "<html><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=yes\" /><head><meta charset = \"utf-8\" > {0} {1} </head> {2} {3}</html> ";

        private const string _cssTemplate = "<link rel=\"Stylesheet\" type=\"text/css\" href=\"{0}\" />";

        private const string _jsTemplate = "<script src=\"{0}\" type=\"text/javascript\"></script>";

        private const string _notifyScript = "<script type=\"text/javascript\">for (var i = 0; i < document.links.length; i++) { document.links[i].onclick = function() { window.external.notify('"+ NotifyPrex + "' + this.href); return false; } }</script>";

        private const string _headerTemplate = "<div style=\"position:relative; height:250;  background:url({0}) no-repeat center center; background-size:100%; \">"
                                               + "<div style=\"position:relative; height:250;  background-image:url(ms-appx-web:///Assets/header_background.png); background-size:100% 100%;\">"
                                               + "<table style = \"position:absolute; Bottom:30px; color:white; font-weight:bold; font-size:30;\" >"
                                               +"<tr><td style=\"width:20px\"></td><td>{1}</td><td style=\"width:20px\"></td></tr>"
                                               +"</table>"
                                               +"<table style=\"position:absolute; right:4px; margin:0,20; Bottom:8px;color:white;font-size:15;\">"
                                               + "<tr><td>{2}</td><td style=\"width:20px\"></td></tr>"
                                               + "</table>"
                                               + "</div><a href=\"circlely://circle/118429\">aaa</a>"
                                               +"</div>";

        public static void ArrangeMainContent(MainContent content)
        {
            content.body = ConstructorHtml(content);
        }

        public static string ConstructorHtml(MainContent content)
        {
            if (string.IsNullOrEmpty(content.body))
                return string.Empty;

            var cssBuilder = new StringBuilder();
            var jsBuilder = new StringBuilder();

            content.css.ForEach(o => cssBuilder.Append(string.Format(_cssTemplate, o)));
            content.js.ForEach(o => jsBuilder.Append(string.Format(_jsTemplate, o)));

            var header = string.Format(_headerTemplate, content.image, content.title, content.image_source);
            var source = string.Format(_htmlTemplate, cssBuilder.ToString(), jsBuilder.ToString(), content.body, _notifyScript);

            source = source.Replace("<div class=\"img-place-holder\"></div>", header);

            return source;
        }
    }
}
