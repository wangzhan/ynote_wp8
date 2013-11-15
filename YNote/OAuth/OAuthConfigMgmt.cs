/**
 * @file OAuthConfigMgmt.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;

namespace YNote.OAuth
{
    [XmlRoot("Configuration")]
    public class Configuration
    {
        [XmlArray("AppSettings"), XmlArrayItem("Setting")]
        public Setting[] settings { get; set; }

        public string GetAttr(string attr)
        {
            for (int i = 0; i < settings.Length; ++i)
            {
                if (settings[i].Key == attr)
                {
                    return settings[i].Value;
                }
            }
            return string.Empty;
        }
    }

    [XmlRoot("Setting")]
    public class Setting
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

    }

    public class OAuthConfigMgmt
    {
        private Configuration _config;
        public Configuration Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public OAuthConfigMgmt()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                var sri = App.GetResourceStream(new Uri("OAuth\\OAuth.xml", UriKind.Relative));
                XmlSerializer serialize = new XmlSerializer(typeof(Configuration));
                using (XmlReader reader = XmlReader.Create(sri.Stream))
                {
                    _config = (Configuration)serialize.Deserialize(reader);
                }
            }
            catch
            {
            }

        }

        public string ConsumerKey
        {
            get
            {
                return Config.GetAttr("AppKey");
            }
        }

        public string ConsumerSecret
        {
            get
            {
                return Config.GetAttr("AppSecret");
            }
        }

        public string CallBack
        {
            get
            {
                return Config.GetAttr("CallBack");
            }
        }
    }
}
