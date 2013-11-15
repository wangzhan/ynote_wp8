/**
 * @file OAuthView.xaml.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using YNote.ViewModels;

namespace YNote.Views
{
    public partial class OAuthView : PhoneApplicationPage
    {
        private OAuthViewModel _oauthViewModel;
        private const string _keyRequestToken = "requesttoken";

        public OAuthView()
        {
            InitializeComponent();

            webBrowser.Navigating += Browser_Navigating;
            _oauthViewModel = App.viewModelLocator.oauthViewModel;
            DataContext = _oauthViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await webBrowser.ClearCookiesAsync();
            await webBrowser.ClearInternetCacheAsync();
            IDictionary<string, string> queryString = NavigationContext.QueryString;
            if (queryString.ContainsKey(_keyRequestToken))
            {
                string requestUrl = queryString[_keyRequestToken];
                webBrowser.Navigate(new Uri(requestUrl, UriKind.Absolute));
            }
        }

        async void Browser_Navigating(Object sender, NavigatingEventArgs e)
        {
            try
            {
                string absPath = e.Uri.AbsolutePath;
                if (absPath.IndexOf(App.oauthController.OAuthMgmt.CallBack) == -1)
                {
                    return;
                }

                bool author = await _oauthViewModel.Browser_Navigating(sender, e.Uri.AbsoluteUri);
                if (author)
                {
                    NavigationService.Navigate(new Uri("/Views/MainHubView.xaml", UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Views/LoginView.xaml?error=1", UriKind.Relative));
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
}