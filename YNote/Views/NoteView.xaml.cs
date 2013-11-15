/**
 * @file NoteView.xaml.cs
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
    public partial class NoteView : PhoneApplicationPage
    {
        private NoteViewModel _noteViewModel;
        private string _id;

        public NoteView()
        {
            InitializeComponent();

            _noteViewModel = App.viewModelLocator.noteViewModel;
            DataContext = _noteViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var queryString = NavigationContext.QueryString;
            if (queryString.ContainsKey("id"))
            {
                _id = queryString["id"];
                await _noteViewModel.OnNavigatedTo(_id);
                webBrowser.NavigateToString(_noteViewModel.Content);
            }
        }

        private void btnAttr_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/NoteAttrView.xaml?id=" + _id, UriKind.Relative));
        }
    }
}