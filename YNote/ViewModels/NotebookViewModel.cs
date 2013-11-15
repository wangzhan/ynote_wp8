/**
 * @file NotebookViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Controls;
using YNote.Models;
using System.Windows.Navigation;

namespace YNote.ViewModels
{
    public class NotebookViewModel : Common.BindableBase
    {
        private string _noteCountText;
        public string NoteCountText
        {
            get { return _noteCountText; }
            set { SetProperty(ref _noteCountText, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        // note list
        private ObservableCollection<NoteIndexInfo> _notes = new ObservableCollection<NoteIndexInfo>();
        [DataMember(Name = "Notes")]
        public ObservableCollection<NoteIndexInfo> Notes
        {
            get { return _notes; }
            set { SetProperty( ref _notes, value); }
        }

        public async void OnNavigatedTo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            try
            {
                List<NoteIndexInfo> noteInfoList = await App.databaseAccess.noteTableHandler.QueryAllNoteIndexInfoAsync(id);
                Notes = new ObservableCollection<NoteIndexInfo>(noteInfoList.OrderByDescending(v => v.ModifiedDate));

                NoteCountText = string.Format("{0} 笔记", Notes.Count);

                NotebookInfo notebookInfo = await App.databaseAccess.notebookTableHandler.QueryNotebookInfo(id);
                Title = notebookInfo.Title;
            }
            catch (System.Exception)
            {
            }
        }

        public string lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = string.Empty;
            if (e.AddedItems.Count > 0)
            {
                NoteIndexInfo noteInfo = e.AddedItems[0] as NoteIndexInfo;
                if (noteInfo != null)
                {
                    id = noteInfo.ID;
                }
            }
            return id;
        }
    }
}
