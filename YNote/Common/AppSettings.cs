/**
 * @file AppSettings.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace YNote.Common
{
    /// <summary>
    /// App setting
    /// </summary>
    public class AppSettings
    {
        // The key names of our settings
        const string UserNameSettingKeyName = "LastLoginUser";

        // 用户名
        private string _userName;
        public string UserName
        {
            get 
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }

        // 密码
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = Password;
            }
        }

        private string _accessToken;
        public string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        private string _accessTokenSecret;
        public string AccessTokenSecret
        {
            get { return _accessTokenSecret; }
            set { _accessTokenSecret = value; }
        }

        private bool _wifiIsChecked;
        public bool WifiIsChecked
        {
            get { return _wifiIsChecked; }
            set { _wifiIsChecked = value; }
        }

        public AppSettings()
        {
            WifiIsChecked = true;
        }

        public void Reset()
        {
            UserName = string.Empty;
            Password = string.Empty;
            AccessToken = string.Empty;
            AccessTokenSecret = string.Empty;
            WifiIsChecked = true;
        }

        public static AppSettings LoadData(string userName)
        {
            AppSettings settings = new AppSettings();
            if (string.IsNullOrEmpty(userName))
            {
                // load the username
                userName = IsolatedStorageSettingsHelper.GetValueOrDefault(UserNameSettingKeyName, string.Empty);
            }

            return IsolatedStorageSettingsHelper.GetValueOrDefault(userName, settings);
        }

        public static void SaveData(AppSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.UserName))
            {
                IsolatedStorageSettingsHelper.AddOrUpdateValue(UserNameSettingKeyName, settings.UserName);
                IsolatedStorageSettingsHelper.AddOrUpdateValue(settings.UserName, settings);
            }
        }

        public bool HasAuthorization()
        {
            bool unAuthor = string.IsNullOrEmpty(_accessToken) ||
                string.IsNullOrEmpty(_accessTokenSecret);
            return !unAuthor;
        }

        public static void DeleteSettings()
        {
            IsolatedStorageSettingsHelper.RemoveAll();
        }
    }
}
