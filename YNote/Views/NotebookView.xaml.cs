/**
 * @file NotebookView.xaml.cs
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
    public partial class NotebookView : PhoneApplicationPage
    {
        private NotebookViewModel _notebookViewModel;

        public NotebookView()
        {
            InitializeComponent();

            lstNotes.lstNotes.SelectionChanged += lstNotes_SelectionChanged;
            _notebookViewModel = App.viewModelLocator.notebookViewModel;
            DataContext = _notebookViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var queryString = NavigationContext.QueryString;
            if (queryString.ContainsKey("id"))
            {
                var id = queryString["id"];
                _notebookViewModel.OnNavigatedTo(id);
            }
        }

        public void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = _notebookViewModel.lstNotes_SelectionChanged(sender, e);
            if (!string.IsNullOrEmpty(id))
            {
                NavigationService.Navigate(new Uri("/Views/NoteView.xaml?id=" + id, UriKind.Relative));
            }
            lstNotes.lstNotes.SelectedIndex = -1;
        }
    }
}