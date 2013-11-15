/**
 * @file LoginModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YNote.Common;

namespace YNote.Models
{
    class LoginModel
    {
        private AppSettings _appSettings;

        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginModel()
        {
            _appSettings = App.appSettings;
            LoadData();
        }

        public void LoadData()
        {
            UserName = _appSettings.UserName;
            Password = _appSettings.Password;
        }

        public void SaveData()
        {
            _appSettings.UserName = UserName;
            _appSettings.Password = Password;
        }
    }
}
