using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace YNote.ViewModels
{
    public class SyncViewModel : Common.BindableBase
    {
        private string _notebookSyncInfo;
        public string NotebookSyncInfo
        {
            get { return _notebookSyncInfo; }
            set { SetProperty(ref _notebookSyncInfo, value); }
        }

        private string _noteSyncInfo;
        public string NoteSyncInfo
        {
            get { return _noteSyncInfo; }
            set { SetProperty(ref _noteSyncInfo, value); }
        }

        public SyncViewModel()
        {
            App.syncController.notebookCountEvent += SetNotebookSyncInfo;
            App.syncController.noteCountEvent += SetNoteSyncInfo;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            NotebookSyncInfo = "笔记本等待同步";
            NoteSyncInfo = "笔记等待同步";
        }

        public void SetNotebookSyncInfo(int downloadedNotebookCount, int allNotebookCount)
        {
            NotebookSyncInfo = string.Format("{0}/{1} 笔记本同步中", downloadedNotebookCount, allNotebookCount);
        }

        public void SetNoteSyncInfo(int downloadedNoteCount, int allNoteCount)
        {
            NoteSyncInfo = string.Format("{0}/{1} 笔记同步中", downloadedNoteCount, allNoteCount);
        }
    }
}
