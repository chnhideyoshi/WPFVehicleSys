using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
namespace Config
{
    public class ConfigReader
    {
        public ConfigReader(string path)
        {
            Path = path;
            if (doc == null)
            {
                doc = new XmlDocument();
                doc.Load(Path);
            }
            if (xnm == null)
            {
                xnm = new XmlNamespaceManager(doc.NameTable);
            }
        }
        #region private
        private  string Path = null;
        private XmlDocument doc;
        private XmlNamespaceManager xnm;
        private XmlNode GetNode(string rPath)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
                doc.Load(Path);
            }
            if (xnm == null)
            {
                xnm = new XmlNamespaceManager(doc.NameTable);
            }
            //读取节点值
            XmlNode node = doc.SelectSingleNode(rPath, xnm);
            return node;
        }      
        private string GetSettingItem(string name)
        {
            XmlNode node = GetNode("/Settings");
            foreach (XmlNode e in node.ChildNodes)
            {
                if ((e as XmlElement).Name == name)
                {
                    return e.Attributes["value"].Value;
                }
            }
            return null;
        }
        #endregion
        #region public method
        public bool GetBooleanSettingItem(string name,bool defaultvalue)
        {
            try
            {
                string value = GetSettingItem(name);
                return Convert.ToBoolean(value);
            }
            catch { return defaultvalue; }
        }
        public int GetIntSettingItem(string name,int defaultvalue)
        {
            try
            {
                string value = GetSettingItem(name);
                return Convert.ToInt32(value);
            }
            catch { return defaultvalue; }
        }
        public double GetDoubleSettingItem(string name,double defaultvalue)
        {
            try
            {
                string value = GetSettingItem(name);
                return Convert.ToDouble(value);
            }
            catch { return defaultvalue; }
        }
        public string GetStringSettingItem(string name,string defaultvalue)
        {
            if (string.IsNullOrEmpty(GetSettingItem(name)))
            {
                return defaultvalue;
            }
            else
            {
                return GetSettingItem(name);
            }
        }
        public List<string> GetStringCollectionSettingItem(string name,List<string> defaultvalue)
        {
            List<string> list = new List<string>();
            try
            {
                XmlNode node = GetNode("/Settings/"+name);
                foreach (XmlNode e in node.ChildNodes)
                {
                    string ext = e.Attributes["value"].Value;
                    list.Add(ext);
                }
            }
            catch { return defaultvalue; }
            if (list.Count == 0)
            {
                return defaultvalue;
            }
            return list;
        }
        #endregion
    }
}
