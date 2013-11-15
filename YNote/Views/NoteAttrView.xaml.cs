/**
 * @file NoteAttrView.xaml.cs
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
    public partial class NoteAttrView : PhoneApplicationPage
    {
        private NoteAttrViewModel _noteAttrViewModel;
        private string _id;

        public NoteAttrView()
        {
            InitializeComponent();

            _noteAttrViewModel = App.viewModelLocator.noteAttrViewModel;
            DataContext = _noteAttrViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var queryString = NavigationContext.QueryString;
            if (queryString.ContainsKey("id"))
            {
                _id = queryString["id"];
                _noteAttrViewModel.OnNavigatedTo(_id);
            }
        }
    }
}