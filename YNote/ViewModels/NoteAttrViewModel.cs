/**
 * @file NoteAttrViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YNote.Models;
using YNote.Common;

namespace YNote.ViewModels
{
    public class NoteAttrViewModel : Common.BindableBase
    {
        private string _noteTitle;
        public string NoteTitle
        {
            get 
            { 
                if (string.IsNullOrEmpty(_noteTitle))
                {
                    _noteTitle = ConstantVariables.NoteTitleForEmpty;
                }
                return _noteTitle; 
            }
            set { SetProperty(ref _noteTitle, value); }
        }

        private string _notebookTitle;
        public string NotebookTitle
        {
            get { return _notebookTitle; }
            set { SetProperty(ref _notebookTitle, value); }
        }

        private string _createdTime;
        public string CreatedTime
        {
            get { return _createdTime; }
            set { SetProperty(ref _createdTime, value); }
        }

        private string _modifiedTime;
        public string ModifiedTime
        {
            get { return _modifiedTime; }
            set { SetProperty(ref _modifiedTime, value); }
        }

        private string _source;
        public string Source
        {
            get 
            {
                if (string.IsNullOrEmpty(_source))
                {
                    _source = ConstantVariables.SourceForEmpty;
                }
                return _source; 
            }
            set { SetProperty(ref _source, value); }
        }

        public NoteAttrViewModel()
        {

        }

        public async void OnNavigatedTo(string id)
        {
            NoteInfo noteInfo = await App.databaseAccess.noteTableHandler.QueryNoteInfoAsync(id);
            if (noteInfo == null)
            {
                return;
            }
            CreatedTime = noteInfo.CreatedDate;
            ModifiedTime = noteInfo.ModifiedDate;
            Source = noteInfo.Source;
            NoteTitle = noteInfo.Title;

            string[] ids = noteInfo.Path.Split('/');
            if (ids.Length > 1)
            {
                string notebookID = ids[1];
                NotebookInfo notebookInfo = await App.databaseAccess.notebookTableHandler.QueryNotebookInfo(notebookID);
                if (notebookInfo != null)
                {
                    NotebookTitle = notebookInfo.Title;
                }
            }
        }
    }
}
