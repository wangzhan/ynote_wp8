/**
 * @file LoginView.xaml.cs
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
using YNote.OAuth;

namespace YNote.Views
{
    public partial class LoginView : PhoneApplicationPage
    {
        private LoginViewModel _loginViewModel;
        public LoginView()
        {
            InitializeComponent();

            padding.Height = 60 * Application.Current.Host.Content.ActualHeight / 100;

            _loginViewModel = App.viewModelLocator.loginViewModel;
            DataContext = _loginViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IDictionary<string, string> queryString = NavigationContext.QueryString;
            if (queryString.ContainsKey("error"))
            {
                return;
            }

            if (App.appSettings.HasAuthorization())
            {
                NavigationService.Navigate(new Uri("/Views/MainHubView.xaml", UriKind.Relative));
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool author = await _loginViewModel.btnLogin_Click(sender, e);
                if (!author)
                {
                    return;
                }

                OAuthController controller = App.oauthController;
                string requestUrl = controller.AuthorUrl + controller.RequestToken;
                NavigationService.Navigate(new Uri("/Views/OAuthView.xaml?requesttoken=" + requestUrl, UriKind.Relative));
            }
            catch (System.Exception)
            {

            }
        }
    }
}