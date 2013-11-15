/**
 * @file SyncView.xaml.cs
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

// TODO: 
namespace YNote.Views
{
    public partial class SyncView : PhoneApplicationPage
    {
        private SyncViewModel _syncViewModel;

        public SyncView()
        {
            InitializeComponent();

            _syncViewModel = App.viewModelLocator.syncViewModel;
            DataContext = _syncViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _syncViewModel.OnNavigatedTo(e);
            NavigationService.Navigate(new Uri("/Views/MainHubView.xaml", UriKind.Relative));
        }
    }
}