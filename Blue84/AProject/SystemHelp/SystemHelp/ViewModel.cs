using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SystemHelp
{
    public class ViewModel
    {
        public static readonly string HelpContentDirectory = "HelpContent";
        public string GetContent(string moduleName)
        {
            string path = HelpContentDirectory + "\\" + "Help_"+moduleName+".txt";
            if (File.Exists(path))
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                    return content;
                }
                catch { return null; }
            }
            else
            {
                return null;
            }
        }
        public System.Windows.Documents.FlowDocument GetControlObject(string moduleName)
        {
            try
            {

            }
            catch { return null; }
            return null;
        }

        public string GetTitle(Modules module)
        {
            string title = "";
            switch (module)
            {
                case Modules.Book: { title = "图书阅读帮助"; } break;
                case Modules.Music: { title = "音乐播放器帮助"; } break;
                case Modules.Video: { title = "视频播放器帮助"; } break;
                case Modules.Picture: { title = "相册帮助"; } break;
                case Modules.Product: { title = "产品介绍"; } break;
                case Modules.GPS: { title = "GPS系统帮助"; } break;
                case Modules.FM: { title = "收音机帮助"; } break;
                case Modules.Explorer: { title = "系统资源管理"; } break;
                default: break;
            }
            return title;
        }
    }
    public enum Modules
    {
        Video=1,
        Music=2,
        Book=3,
        Picture=4,
        FM=5,
        GPS=6,
        Explorer=7,
        Product=8
    }
}
