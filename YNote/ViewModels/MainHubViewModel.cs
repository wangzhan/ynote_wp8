/**
 * @file MainHubViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.Serialization;
using YNote.Models;
using System.Windows.Navigation;
using YNote.Models.SyncUtil;
using YNote.Common;
using Microsoft.Phone.Controls;

namespace YNote.ViewModels
{
    /// <summary>
    /// sort type
    /// </summary>
    public enum ESortType
    {
        TYPE_TITLE,
        TYPE_CREATE_TIME,
        TYPE_MODIFY_TIME
    }

    /// <summary>
    /// refresh type: note, notebook or both
    /// </summary>
    public enum ERefreshEntry
    {
        REFRESH_NOTE,
        REFRESH_NOTEBOOK,
        REFRESH_ALL
    }

    public class MainHubViewModel : Common.BindableBase
    {
        private MainHubModel _mainHubModel;
        private SyncController _syncController;

        private ESortType _sortType;
        private bool _sortAscending;

        // 笔记列表
        private ObservableCollection<NoteIndexInfo> _notes = new ObservableCollection<NoteIndexInfo>();
        [DataMember(Name = "Notes")]
        public ObservableCollection<NoteIndexInfo> Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        // 笔记本列表
        private ObservableCollection<NotebookInfo> _notebooks = new ObservableCollection<NotebookInfo>();
        [DataMember(Name = "Notebooks")]
        public ObservableCollection<NotebookInfo> Notebooks
        {
            get { return _notebooks; }
            set { SetProperty(ref _notebooks, value); }
        }

        // 是否同步
        private bool _isSync;
        public bool IsSync
        {
            get { return _isSync; }
            set { SetProperty(ref _isSync, value); }
        }

        // 同步状态
        private string _syncStatus = "同步中";
        public string SyncStatus
        {
            get { return _syncStatus; }
            set { SetProperty(ref _syncStatus, value); }
        }

        // 笔记或笔记本数量
        private string _entryCountText;
        public string EntryCountText
        {
            get { return _entryCountText; }
            set { SetProperty(ref _entryCountText, value); }
        }

        private bool _firstSync = true;
        
        // 当前pivot位置
        private int _pivotIndex = 0;
        private const int _RefreshInterval = 10;
        private int _downloadNoteCount = 0;
        private int _downloadNotebookCount = 0;
        
        public MainHubViewModel()
        {
            _mainHubModel = new MainHubModel();
            _syncController = App.syncController;
            _syncController.syncStateEvent += onSyncState;
            _syncController.notebookCountEvent += SetNotebookSyncInfo;
            _syncController.noteCountEvent += SetNoteSyncInfo;
            _syncController.notebookRefreshEvent += RefreshNotebooks;

            _sortType = ESortType.TYPE_MODIFY_TIME;
            _sortAscending = false;
            _pivotIndex = 0;
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_firstSync)
            {
                await refreshEntries(ERefreshEntry.REFRESH_ALL);

                _firstSync = false;
                await _syncController.PullAllAsync();
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

        public string lstNotebooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = string.Empty;
            var index = (sender as ListBox).SelectedIndex;
            if ((0 <= index) && (index < Notebooks.Count))
            {
                id = Notebooks[index].ID;
            }
            return id;
        }

        public async void btnSync_Click(object sender, EventArgs e)
        {
            if (!IsSync)
            {
                await _syncController.PullAllAsync();
            }
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            // TODO: 
        }

        public async Task menuItemLogout_Click(object sender, EventArgs e)
        {
            // delete local data and reset the appsettings in app instance
            _firstSync = true;
            Notebooks.Clear();
            Notes.Clear();

            AppSettings.DeleteSettings();
            App.appSettings = new AppSettings();
            App.databaseAccess.Reset();
            App.oauthController = new OAuth.OAuthController();
            await App.appFolderController.DeleteFiles();
        }

        public void btnSortByTitle_Click(object sender, EventArgs e)
        {
            if (_sortType == ESortType.TYPE_TITLE)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortAscending = true;
                _sortType = ESortType.TYPE_TITLE;
            }
            
            if (_sortAscending)
            {
                Notebooks = new ObservableCollection<NotebookInfo>(Notebooks.OrderBy(v => v.Title));
                Notes = new ObservableCollection<NoteIndexInfo>(Notes.OrderBy(v => v.Title));
            }
            else
            {
                Notebooks = new ObservableCollection<NotebookInfo>(Notebooks.OrderByDescending(v => v.Title));
                Notes = new ObservableCollection<NoteIndexInfo>(Notes.OrderByDescending(v => v.Title));
            }
        }

        public void btnSortByCreateTime_Click(object sender, EventArgs e)
        {
            if (_sortType == ESortType.TYPE_CREATE_TIME)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortAscending = false;
                _sortType = ESortType.TYPE_CREATE_TIME;
            }

            if (_sortAscending)
            {
                Notebooks = new ObservableCollection<NotebookInfo>(Notebooks.OrderBy(v => v.CreateTime));
                Notes = new ObservableCollection<NoteIndexInfo>(Notes.OrderBy(v => v.CreatedDate));
            }
            else
            {
                Notebooks = new ObservableCollection<NotebookInfo>(Notebooks.OrderByDescending(v => v.CreateTime));
                Notes = new ObservableCollection<NoteIndexInfo>(Notes.OrderByDescending(v => v.CreatedDate));
            }
        }

        public void btnSortByModifiedTime_Click(object sender, EventArgs e)
        {
            if (_sortType == ESortType.TYPE_MODIFY_TIME)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortAscending = false;
                _sortType = ESortType.TYPE_MODIFY_TIME;
            }

            if (_sortAscending)
            {
                Notebooks = new ObservableCollection<NotebookInfo>(Notebooks.OrderBy(v => v.ModifyTime));
                Notes = new ObservableCollection<NoteIndexInfo>(Notes.OrderBy(v => v.ModifiedDate));
            }
            else
            {
                Notebooks = new ObservableCollection<NotebookInfo>(Notebooks.OrderByDescending(v => v.ModifyTime));
                Notes = new ObservableCollection<NoteIndexInfo>(Notes.OrderByDescending(v => v.ModifiedDate));
            }
        }

        public async void onSyncState(ESyncState state)
        {
            if (state == ESyncState.SYNC_START)
            {
                IsSync = true;
                _downloadNotebookCount = 0;
                _downloadNoteCount = 0;
            }
            else if (state == ESyncState.SYNC_END)
            {
                IsSync = false;
                if ((_downloadNotebookCount % _RefreshInterval) != 0)
                {
                    await refreshEntries(ERefreshEntry.REFRESH_NOTEBOOK);
                }

                if ((_downloadNoteCount % _RefreshInterval) != 0)
                {
                    await refreshEntries(ERefreshEntry.REFRESH_NOTE);
                }
            }
        }

        public async Task refreshEntries(ERefreshEntry refresh)
        {
            try
            {
                if ((refresh == ERefreshEntry.REFRESH_NOTEBOOK) || (refresh == ERefreshEntry.REFRESH_ALL))
                {
                    // Read lcoal notebooks
                    List<NotebookInfo> notebookInfoList = await App.databaseAccess.notebookTableHandler.QueryAllNotebookInfoAsync();
                    if (notebookInfoList != null)
                    {
                        Notebooks.Clear();
                        notebookInfoList.ForEach(v => Notebooks.Add(v));
                    }
                }

                if ((refresh == ERefreshEntry.REFRESH_NOTE) || (refresh == ERefreshEntry.REFRESH_ALL))
                {
                    // Read local notes
                    List<NoteIndexInfo> noteInfoList = await App.databaseAccess.noteTableHandler.QueryAllNoteIndexInfoAsync(string.Empty);
                    if (noteInfoList != null)
                    {
                        Notes.Clear();
                        noteInfoList.ForEach(v => Notes.Add(v));
                    }
                }

                _sortAscending = !_sortAscending;
                if (_sortType == ESortType.TYPE_TITLE)
                {
                    btnSortByTitle_Click(null, null);
                }
                else if (_sortType == ESortType.TYPE_CREATE_TIME)
                {
                    btnSortByCreateTime_Click(null, null);
                }
                else if (_sortType == ESortType.TYPE_MODIFY_TIME)
                {
                    btnSortByModifiedTime_Click(null, null);
                }

                SetEntryCountText();
            }
            catch (System.Exception)
            {

            }
        }

        public void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            _pivotIndex = pivot.SelectedIndex;
            SetEntryCountText();
        }

        private void SetNoteCountText()
        {
            if (Notes != null)
            {
                EntryCountText = string.Format("{0} 笔记", Notes.Count);
            }
            else
            {
                EntryCountText = string.Format("0 笔记");
            }
        }

        private void SetNotebookCountText()
        {
            if (Notebooks != null)
            {
                EntryCountText = string.Format("{0} 笔记本", Notebooks.Count);
            }
            else
            {
                EntryCountText = string.Format("0 笔记本");
            }
        }

        private void SetEntryCountText()
        {
            if (_pivotIndex == 0)
            {
                SetNoteCountText();
            }
            else if (_pivotIndex == 1)
            {
                SetNotebookCountText();
            }
        }

        private async void SetNotebookSyncInfo(int downloadedNotebookCount, int allNotebookCount)
        {
            if ((downloadedNotebookCount > 0) && (downloadedNotebookCount % _RefreshInterval) == 0)
            {
                await refreshEntries(ERefreshEntry.REFRESH_NOTEBOOK);
            }
            _downloadNotebookCount = downloadedNotebookCount;
        }

        private async void SetNoteSyncInfo(int downloadedNoteCount, int allNoteCount)
        {
            if ((downloadedNoteCount > 0) && (downloadedNoteCount % _RefreshInterval) == 0)
            {
                await refreshEntries(ERefreshEntry.REFRESH_NOTE);
            }
            _downloadNoteCount = downloadedNoteCount;
        }

        private async void RefreshNotebooks()
        {
            // need to refresh notebooks and notes
            await refreshEntries(ERefreshEntry.REFRESH_ALL);
        }
    }
}
