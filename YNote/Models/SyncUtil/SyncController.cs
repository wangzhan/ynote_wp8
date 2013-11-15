/**
 * @file SyncController.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YNote.Models.DatabaseUtil;
using YNote.Models.RepositoryUtil;
using Microsoft.Phone.Net.NetworkInformation;
using YNote.Common;
using System.IO;

namespace YNote.Models.SyncUtil
{
    public delegate void SetCount(int arg0, int arg1);

    public enum ESyncState 
    {
        SYNC_START = 0,
        SYNC_END
    }
    public delegate void SyncState(ESyncState state);

    public delegate void RefreshNotebooks();

    public class SyncController
    {
        private DatabaseAccess _databaseAccess;
        private RepositoryAccess _repositoryAccess;

        private Dictionary<string, string> _HTMLEntityDict;

        public event SetCount notebookCountEvent;
        public event SetCount noteCountEvent;
        public event SyncState syncStateEvent;
        public event RefreshNotebooks notebookRefreshEvent;

        public SyncController()
        {
            _databaseAccess = App.databaseAccess;
            _repositoryAccess = App.repositoryAccess;

            FillHTMLEntityDict();
        }

        public async Task PullAllAsync()
        {
            try
            {
                // TODO: wifi only
                if (App.appSettings.WifiIsChecked && !DeviceNetworkInformation.IsWiFiEnabled)
                {
                    return;
                }

                syncStateEvent(ESyncState.SYNC_START);

                // 1. pull all notebooks
                NotebookInfoListJson notebookList = await PullNotebooksAsync();
                if ((notebookList == null) || (notebookList.Count == 0))
                {
                    return;
                }

                int notebookCount = notebookList.Count;
                int downloadedNotebookCount = 0;
                notebookCountEvent(downloadedNotebookCount, notebookCount);

                // 2. pull notes in notebooks
                foreach (var v in notebookList)
                {
                    await PullNotesAsync(v.path);
                    notebookCountEvent(++downloadedNotebookCount, notebookCount);
                }

                syncStateEvent(ESyncState.SYNC_END);
            }
            catch (System.Exception)
            {
            }
        }

        public async Task<NotebookInfoListJson> PullNotebooksAsync()
        {
            NotebookInfoListJson remoteNotebooks = null;
            IList<NotebookInfoSchema> localNotebooks = null;
            try
            {
                remoteNotebooks = await _repositoryAccess.ListNotebooks();
                localNotebooks = await _databaseAccess.notebookTableHandler.QueryAllNotebookSchemaAsync();
            }
            catch (System.Exception)
            {
                return null;
            }

            IList<NotebookInfoSchema> deleteList = new List<NotebookInfoSchema>();
            NotebookInfoListJson updateListForNotebook = new NotebookInfoListJson();
            NotebookInfoListJson updateListForNote = new NotebookInfoListJson();

            // 1. classify notebooks
            foreach (var v in localNotebooks)
            {
                try
                {
                    var item = remoteNotebooks.Find(
                            delegate(NotebookInfoJson value)
                            {
                                return (value.path == v.GetPath());
                            }
                        );

                    // 笔记本有没有修改
                    if (v.ModifyTime != item.modify_time)
                    {
                        updateListForNotebook.Add(item);
                    }
                    else
                    {
                        // 当前笔记本中的笔记是否下载完全
                        int iLocalNoteCount = await _databaseAccess.noteTableHandler.QueryNoteCountByNoteboookAsync(
                            PathHelper.ExtractNotebookIDFromNotebookPath(item.path)
                            );
                        int iRemoteNoteCount = int.Parse(item.notes_num);
                        if (iLocalNoteCount != iRemoteNoteCount)
                        {
                            updateListForNote.Add(item);
                        }
                    }

                    remoteNotebooks.Remove(item);
                }
                catch (System.Exception)
                {
                    // 没有在本地找到笔记本
                    deleteList.Add(v);
                }
            }

            updateListForNotebook.AddRange(remoteNotebooks);

            // 2. store notebooks
            foreach (var v in updateListForNotebook)
            {
                NotebookInfoSchema schema = new NotebookInfoSchema();
                schema.CreateTime = v.create_time;
                schema.ModifyTime = v.modify_time;
                schema.Name = v.name;
                schema.NotesNum = v.notes_num;
                schema.SetPath(v.path);
                await _databaseAccess.notebookTableHandler.UpdateNotebookInfoAsync(schema);

                notebookRefreshEvent();
            }

            foreach (var v in deleteList)
            {
                await DeleteNotebook(v.ID);
                notebookRefreshEvent();
            }

            // 3. need to update note list for notebook
            updateListForNotebook.AddRange(updateListForNote);
            return updateListForNotebook;
        }

        public async Task PullNotesAsync(string notebookPath)
        {
            if (string.IsNullOrEmpty(notebookPath))
            {
                return;
            }

            // 1. get notes list from server and local
            var ids = notebookPath.Split('/');
            NotePathListJson notePathList = null;
            List<NoteInfoSchema> schemas = null;
            try
            {
                notePathList = await _repositoryAccess.ListNotes(notebookPath);
                schemas = await _databaseAccess.noteTableHandler.QueryAllNoteSchemaAsync(ids[1]);
            }
            catch (System.Exception)
            {
                return;
            }

            int noteCount = notePathList.Count;
            int downloadedNoteCount = 0;
            noteCountEvent(downloadedNoteCount, noteCount);

            // 2. delete local notes
            foreach (var v in schemas)
            {
                bool bFind = false;
                foreach (var i in notePathList)
                {
                    if (v.GetPath() == i)
                    {
                        bFind = true;
                    }
                }

                if (!bFind)
                {
                    await DeleteNote(ids[1]);
                }
            }

            // 3. update local notes
            foreach (var v in notePathList)
            {
                NoteInfoJson noteInfoJson = await _repositoryAccess.GetNoteInfo(v);
                var pairs = v.Split('/');
                NoteInfo localInfo = await _databaseAccess.noteTableHandler.QueryNoteInfoAsync(pairs[2]);
                if ((localInfo == null) || (localInfo.ModifiedDate != noteInfoJson.modify_time))
                {
                    NoteInfoSchema schema = NoteInfoJsonToSchema(v, noteInfoJson);
                    await _databaseAccess.noteTableHandler.UpdateNoteInfoAsync(schema);

                    // 5. pull and update thumbnails, noteid/thumbnailid
                    List<string> urls = RegexUtil.GetThumbnailsUrl(noteInfoJson.content);
                    if (urls.Count > 0)
                    {
                        try
                        {
                            await PullPics(schema.ID, urls);
                        }
                        catch (System.Exception)
                        {
                        }
                    }

                    // 6. TODO: save note text to local, noteid/noteid
                    //await SaveNoteContent(schema.ID, urls, noteInfoJson.content);
                }
                noteCountEvent(++downloadedNoteCount, noteCount);
            }
        }

        private async Task PullPics(string noteID, List<string> urls)
        {
            // detele the note files first
            await App.appFolderController.DeleteNoteFiles(noteID);

            foreach (string url in urls)
            {
                Stream content = await _repositoryAccess.GetAttach(url);
                int index = url.LastIndexOf('/');
                ++index;
                string name = url.Substring(index, url.Length - index);
                name = string.Format("{0}\\{1}", noteID, name);
                await App.appFolderController.SaveFile(name, content);
            }
        }

        // TODO: 
        //private async Task SaveNoteContent(string noteId, List<string> urls, string content)
        //{
            // TODO: replace the url to local path
            //foreach (string url in urls)
            //{
            //    if (string.IsNullOrEmpty(url))
            //    {
            //        continue;
            //    }

            //    int index = url.LastIndexOf('/');
            //    ++index;
            //    string name = url.Substring(index, url.Length - index);
            //    if (!string.IsNullOrEmpty(name))
            //    {
            //        name = string.Format("{0}/{1}", noteId, name);
            //        content.Replace(url, name);
            //    }
            //}

            //string path = string.Format("{0}\\{1}", noteId, noteId);
            //await App.appFolderController.SaveFile(path, content);
        //}

        private string ContentToAbstract(string content)
        {
            
            // remote the tags
            content = RegexUtil.RemoveTags(content);

            // convert the encoding
            foreach (var item in _HTMLEntityDict)
            {
                content = content.Replace(item.Key, item.Value);
            }

            content = content.Trim();

            int maxLen = 50;
            if (content.Length > maxLen)
            {
                content = content.Substring(0, maxLen);
            }
            return content;
        }

        private void FillHTMLEntityDict()
        {
            _HTMLEntityDict = new Dictionary<string, string>();
            _HTMLEntityDict.Add("\n", " ");
            _HTMLEntityDict.Add("\t", " ");
            _HTMLEntityDict.Add("&nbsp;", " ");
            _HTMLEntityDict.Add("&lt;", "<");
            _HTMLEntityDict.Add("&gt;", ">");
            _HTMLEntityDict.Add("&amp;", "&");
            _HTMLEntityDict.Add("&quot;", "\"");
            _HTMLEntityDict.Add("&apos;", "'");
            _HTMLEntityDict.Add("&times;", "×");
            _HTMLEntityDict.Add("&divide;", "÷");
            _HTMLEntityDict.Add("&reg;", "®");
            _HTMLEntityDict.Add("&copy;", "©");
        }

        private NoteInfoSchema NoteInfoJsonToSchema(string path, NoteInfoJson noteInfoJson)
        {
            NoteInfoSchema schema = new NoteInfoSchema();
            schema.CreateTime = noteInfoJson.create_time;
            schema.ModifyTime = noteInfoJson.modify_time;
            schema.SetPath(path);
            schema.Size = noteInfoJson.size;
            schema.Source = noteInfoJson.source;
            schema.Thumbnail = noteInfoJson.thumbnail;
            schema.Title = noteInfoJson.title;
            schema.Content = noteInfoJson.content;
            schema.Abstract = ContentToAbstract(noteInfoJson.content);
            return schema;
        }

        private async Task DeleteNotebook(string notebookID)
        {
            List<NoteIndexInfo> noteList = await _databaseAccess.noteTableHandler.QueryAllNoteIndexInfoAsync(notebookID);
            foreach (var v in noteList)
            {
                await App.appFolderController.DeleteNoteFiles(v.ID);
            }

            await _databaseAccess.noteTableHandler.DeleteNoteInfoByNotebookAsync(notebookID);
            await _databaseAccess.notebookTableHandler.DeleteNotebookInfoAsync(notebookID);
        }

        private async Task DeleteNote(string noteID)
        {
            await App.appFolderController.DeleteNoteFiles(noteID);
            await _databaseAccess.noteTableHandler.DeleteNoteInfoAsync(noteID);
        }
    }
}
