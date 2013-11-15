/**
 * @file SearchViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using YNote.Models;

namespace YNote.ViewModels
{
    public class SearchViewModel : Common.BindableBase
    {
        private string _noteCountText;
        public string NoteCountText
        {
            get { return _noteCountText; }
            set { SetProperty(ref _noteCountText, value); }
        }

        private string _searchText;
        [DataMember(Name = "SearchText")]
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        // note list
        private ObservableCollection<NoteIndexInfo> _notes = new ObservableCollection<NoteIndexInfo>();
        [DataMember(Name = "Notes")]
        public ObservableCollection<NoteIndexInfo> Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public SearchViewModel()
        {

        }

        public void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
