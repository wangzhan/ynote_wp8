/**
 * @file OAuthViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Navigation;
using YNote.Models.RepositoryUtil;

namespace YNote.ViewModels
{
    public class OAuthViewModel
    {
        public OAuthViewModel()
        {

        }

        public async Task<bool> Browser_Navigating(Object sender, string result)
        {
            if (result.IndexOf(App.oauthController.OAuthMgmt.CallBack) != -1)
            {
                IDictionary<string, string> dictVerifier = Common.HttpHelper.GetQueryParameters(result);
                if (dictVerifier.ContainsKey(App.oauthController.OAuthVerifierKey))
                {
                    App.oauthController.RequestVerifier = dictVerifier[App.oauthController.OAuthVerifierKey];
                }
            }

            await App.oauthController.GetAccessToken();
            bool author = !string.IsNullOrEmpty(App.oauthController.AccessToken);
            if (author)
            {
                // TODO: for test
                //testRepositoryAccess();
                //return true;

                UserInfoJson userInfoJson = await App.repositoryAccess.GetUserInfo();
                if (userInfoJson != null)
                {
                    // Store the access token and secret
                    App.appSettings.UserName = userInfoJson.user;
                    App.appSettings.AccessToken = App.oauthController.AccessToken;
                    App.appSettings.AccessTokenSecret = App.oauthController.AccessSecret;
                    Common.AppSettings.SaveData(App.appSettings);

                    await App.appFolderController.InitializeFoldersAsync(userInfoJson.user);
                    App.databaseAccess.Initialize(App.appFolderController.DatabasePath);
                }
            }
            return author;
        }

        private void testRepositoryAccess()
        {
            RepositoryAccessTest test = new RepositoryAccessTest();
            test.TestRepositoryAccess();
        }
    }
}
