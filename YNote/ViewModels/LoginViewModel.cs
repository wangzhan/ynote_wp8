/**
 * @file LoginViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using YNote.Models;
using YNote.OAuth;
using YNote.Models.DatabaseUtil;

namespace YNote.ViewModels
{
    public class LoginViewModel : Common.BindableBase
    {
        private LoginModel _loginModel;

        public LoginViewModel()
        {
            _loginModel = new LoginModel();
        }

        public async Task<bool> btnLogin_Click(object sender, EventArgs e)
        {
            // todo:
            //testDB();
            //return false;

            OAuthController controller = App.oauthController;
            try
            {
                await controller.GetRequestToken();
            }
            catch (System.Exception)
            {
            }

            bool unAuthor = string.IsNullOrEmpty(controller.RequestToken);
            return !unAuthor;
        }

        private void testDB()
        {
            DatabaseTest testdb = new DatabaseTest();
            testdb.Init();
        }
    }
}
