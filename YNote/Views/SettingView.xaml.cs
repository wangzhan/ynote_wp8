/**
 * @file SettingView.xaml.cs
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
    public partial class SettingView : PhoneApplicationPage
    {
        private SettingViewModel _settingViewModel;

        public SettingView()
        {
            InitializeComponent();

            _settingViewModel = App.viewModelLocator.settingViewModel;
            DataContext = _settingViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _settingViewModel.OnNavigatedTo(e);
        }

        private void btnClearCache_Click(object sender, RoutedEventArgs e)
        {
            _settingViewModel.btnClearCache_Click(sender, e);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            _settingViewModel.OnClick(sender, e);
        }
    }
}