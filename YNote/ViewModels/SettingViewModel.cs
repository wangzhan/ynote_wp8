/**
 * @file SettingViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using YNote.Common;

namespace YNote.ViewModels
{
    public class SettingViewModel : Common.BindableBase
    {
        private string _account;
        public string Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private string _capacityInfo;
        public string CapacityInfo
        {
            get { return _capacityInfo; }
            set { SetProperty(ref _capacityInfo, value); }
        }

        private string _about;
        public string About
        {
            get { return _about; }
            set { SetProperty(ref _about, value); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }

        public SettingViewModel()
        {
            About = "有道云笔记个人版，基于有道云笔记的OpenAPI接口，其中，1.0版本实现了笔记本和笔记的查看，后续版本将增加对笔记本和笔记的创建、修改及删除。";
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            Account = App.appSettings.UserName;
            IsChecked = App.appSettings.WifiIsChecked;

            try
            {
                var size = await App.appFolderController.GetSize();
                size /= 1024;
                CapacityInfo = string.Format("占用空间：{0} KB", size);
            }
            catch (System.Exception)
            {
            }
        }

        public void OnClick(object sender, RoutedEventArgs e)
        {
            App.appSettings.WifiIsChecked = IsChecked;
            AppSettings.SaveData(App.appSettings);
        }

        public void btnClearCache_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 
        }
    }
}
