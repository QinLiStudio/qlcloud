using System;
using System.Collections.Generic;
using System.Text;

namespace qld.DataTemplates
{
    public class MetaData
    {
        public class FilesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
        }

        public class Config
        {
            /// <summary>
            /// 琴理下载中心
            /// </summary>
            public string siteName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string searchEnable { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string username { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string domain { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string customJs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string customCss { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tableSize { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string showOperator { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string showDocument { get; set; }
            /// <summary>
            /// 如有更多需求，请发送邮件至 ql@sylu.edu.cn
            /// </summary>
            public string announcement { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string showAnnouncement { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string layout { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string readme { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string debugMode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string defaultSwitchToImgMode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string showLinkBtn { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string showShortLink { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string showPathLink { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string directLinkPrefix { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public List<FilesItem> files { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Config config { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 操作成功
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data data { get; set; }
        }

    }
}
