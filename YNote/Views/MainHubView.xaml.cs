/**
 * @file MainHubView.xaml.cs
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
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace YNote.Views
{
    public partial class MainHubView : PhoneApplicationPage
    {
        private MainHubViewModel _mainHubViewModel;
        private Popup _popup;

        public MainHubView()
        {
            InitializeComponent();

            _mainHubViewModel = App.viewModelLocator.mainHubViewModel;
            DataContext = _mainHubViewModel;

            createPopup();
            showPopup(false);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _mainHubViewModel.OnNavigatedTo(e);

            while (NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = _mainHubViewModel.lstNotes_SelectionChanged(sender, e);
            if (!string.IsNullOrEmpty(id))
            {
                NavigationService.Navigate(new Uri("/Views/NoteView.xaml?id=" + id, UriKind.Relative));
            }

            // used to select the same item next time
            lstNotes.SelectedIndex = -1;
        }

        private void lstNotebooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = _mainHubViewModel.lstNotebooks_SelectionChanged(sender, e);
            if (!string.IsNullOrEmpty(id))
            {
                NavigationService.Navigate(new Uri("/Views/NotebookView.xaml?id=" + id, UriKind.Relative));
            }
            // used to select the same item next time
            lstNotebooks.SelectedIndex = -1;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            showPopup(true);
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            _mainHubViewModel.btnSync_Click(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // TODO: unimplement
            _mainHubViewModel.btnSearch_Click(sender, e);
        }

        private void menuItemSetting_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingView.xaml", UriKind.Relative));
        }

        private void menuItemLogout_Click(object sender, EventArgs e)
        {
            var box = new CustomMessageBox
            {
                Caption = "云笔记",
                Title = "",
                Message = "确定退出登录？",
                LeftButtonContent = "确定",
                RightButtonContent = "取消",
                Background = new SolidColorBrush(Color.FromArgb(255, 55, 144, 206)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))
            };
            box.Dismissed += async (sb, eb) =>
                {
                    switch (eb.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            await _mainHubViewModel.menuItemLogout_Click(sender, e);
                            NavigationService.Navigate(new Uri("/Views/LoginView.xaml", UriKind.Relative));
                            break;
                        case CustomMessageBoxResult.RightButton:
                            break;
                        default:
                            break;
                    }
                };
            box.Show();
        }

        private void btnSortByTitle_Click(object sender, EventArgs e)
        {
            _mainHubViewModel.btnSortByTitle_Click(sender, e);
        }

        private void btnSortByCreateTime_Click(object sender, EventArgs e)
        {
            _mainHubViewModel.btnSortByCreateTime_Click(sender, e);
        }

        private void btnSortByModifiedTime_Click(object sender, EventArgs e)
        {
            _mainHubViewModel.btnSortByModifiedTime_Click(sender, e);
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainHubViewModel.Pivot_SelectionChanged(sender, e);
        }

        private void createPopup()
        {
            _popup = new Popup();
            var userControl = new PopupUserControl();
            userControl.padding.Tap += (s, e) =>
            {
                showPopup(false);
            };

            userControl.btnTitle.Click += (s, args) =>
            {
                _popup.IsOpen = false;
                ApplicationBar.IsVisible = true;
                btnSortByTitle_Click(s, args);
            };
            userControl.btnCreateTime.Click += (s, args) =>
            {
                _popup.IsOpen = false;
                ApplicationBar.IsVisible = true;
                btnSortByCreateTime_Click(s, args);
            };
            userControl.btnModifiedTime.Click += (s, args) =>
            {
                _popup.IsOpen = false;
                ApplicationBar.IsVisible = true;
                btnSortByModifiedTime_Click(s, args);
            };
            userControl.padding.Height = Application.Current.Host.Content.ActualHeight - userControl.buttonContainer.Height;
            userControl.LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;
            userControl.LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            _popup.Child = userControl;
        }

        private void showPopup(bool bShow)
        {
            ApplicationBar.IsVisible = !bShow;
            _popup.IsOpen = bShow;
        }
    }
}