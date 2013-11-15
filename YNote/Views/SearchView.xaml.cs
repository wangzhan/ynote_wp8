/**
 * @file SearchView.xaml.cs
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
    public partial class SearchView : PhoneApplicationPage
    {
        private SearchViewModel _searchViewModel;

        public SearchView()
        {
            InitializeComponent();

            lstNotes.lstNotes.SelectionChanged += lstNotes_SelectionChanged;
            _searchViewModel = App.viewModelLocator.searchViewModel;
            DataContext = _searchViewModel;
        }

        private void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _searchViewModel.lstNotes_SelectionChanged(sender, e);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _searchViewModel.btnSearch_Click(sender, e);
        }
    }
}