using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YNote.ViewModels;

namespace YNote.ViewModels
{
    public class ViewModelLocator
    {
        private LoginViewModel _loginViewModel = null;
        public LoginViewModel loginViewModel
        {
            get 
            { 
                if (_loginViewModel == null)
                {
                    _loginViewModel = new LoginViewModel();
                }
                return _loginViewModel;
            }
        }

        private MainHubViewModel _mainHubViewModel;
        public MainHubViewModel mainHubViewModel
        {
            get
            {
                if (_mainHubViewModel == null)
                {
                    _mainHubViewModel = new MainHubViewModel();
                }
                return _mainHubViewModel;
            }
        }

        private NotebookViewModel _notebookViewModel;
        public NotebookViewModel notebookViewModel
        {
            get
            {
                if (_notebookViewModel == null)
                {
                    _notebookViewModel = new NotebookViewModel();
                }
                return _notebookViewModel;
            }
        }

        private SearchViewModel _searchViewModel;
        public SearchViewModel searchViewModel
        {
            get
            {
                if (_searchViewModel == null)
                {
                    _searchViewModel = new SearchViewModel();
                }
                return _searchViewModel;
            }
        }

        private NoteViewModel _noteViewModel;
        public NoteViewModel noteViewModel
        {
            get
            {
                if (_noteViewModel == null)
                {
                    _noteViewModel = new NoteViewModel();
                }
                return _noteViewModel;
            }
        }

        private NoteAttrViewModel _noteAttrViewModel;
        public NoteAttrViewModel noteAttrViewModel
        {
            get
            {
                if (_noteAttrViewModel == null)
                {
                    _noteAttrViewModel = new NoteAttrViewModel();
                }
                return _noteAttrViewModel;
            }
        }

        private SettingViewModel _settingViewModel;
        public SettingViewModel settingViewModel
        {
            get
            {
                if (_settingViewModel == null)
                {
                    _settingViewModel = new SettingViewModel();
                }
                return _settingViewModel;
            }
        }

        private SyncViewModel _syncViewModel;
        public SyncViewModel syncViewModel
        {
            get
            {
                if (_syncViewModel == null)
                {
                    _syncViewModel = new SyncViewModel();
                }
                return _syncViewModel;
            }
        }

        private OAuthViewModel _oauthViewModel;
        public OAuthViewModel oauthViewModel
        {
            get 
            { 
                if (_oauthViewModel == null)
                {
                    _oauthViewModel = new OAuthViewModel();
                }
                return _oauthViewModel; 
            }
        }
    }
}
