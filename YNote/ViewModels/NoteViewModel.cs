/**
 * @file NoteViewModel.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using YNote.Models;
using YNote.Common;

namespace YNote.ViewModels
{
    public class NoteViewModel : Common.BindableBase
    {
        private string _title;
        public string Title
        {
            get 
            { 
                if (string.IsNullOrEmpty(_title))
                {
                    _title = ConstantVariables.NoteTitleForEmpty;
                }
                return _title; 
            }
            set { SetProperty(ref _title, value); }
        }

        private string _content;
        public string Content
        {
            get 
            { 
                if (_content == null)
                {
                    return string.Empty;
                }
                return _content; 
            }
            set { SetProperty(ref _content, value); }
        }

        public NoteViewModel()
        {

        }

        public async Task OnNavigatedTo(string id)
        {
            // TODO:  1. replace the thumbnail url; 3. add the click json
            try
            {
                NoteInfo noteInfo = await App.databaseAccess.noteTableHandler.QueryNoteInfoAsync(id);
                Title = noteInfo.Title;

                // replace 'img' tag 'src' attribute to local img path
                string basePath = string.Format("{0}\\{1}", App.appFolderController.ImagesPath, id);
                Content = string.Format("<html><head></head><body>{0}</body></html>", RegexUtil.ReplaceThumbnailsUrl(noteInfo.Content, basePath));
            }
            catch (System.Exception)
            {
            }
        }
    }
}
