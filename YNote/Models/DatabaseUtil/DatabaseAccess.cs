/**
 * @file DatabaseAccess.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace YNote.Models.DatabaseUtil
{
    public class DatabaseAccess
    {
        private SQLiteAsyncConnection _connection = null;
        private string _path = string.Empty;

        private UserInfoTableHandler _userInfoTableHandler;
        public UserInfoTableHandler userInfoTableHandler
        {
            get
            {
                if (_connection == null)
                {
                    return null;
                }

                if (_userInfoTableHandler == null)
                {
                    _userInfoTableHandler = new UserInfoTableHandler();
                }
                return _userInfoTableHandler;
            }
        }

        private NotebookTableHandler _notebookTableHandler;
        public NotebookTableHandler notebookTableHandler
        {
            get
            {
                if (_connection == null)
                {
                    return null;
                }

                if (_notebookTableHandler == null)
                {
                    _notebookTableHandler = new NotebookTableHandler();
                }
                return _notebookTableHandler;
            }
        }

        private NoteTableHandler _noteTableHandler;
        public NoteTableHandler noteTableHandler
        {
            get
            {
                if (_connection == null)
                {
                    return null;
                }

                if (_noteTableHandler == null)
                {
                    _noteTableHandler = new NoteTableHandler();
                }
                return _noteTableHandler;
            }
        }

        public DatabaseAccess()
        {

        }

        ~DatabaseAccess()
        {
            Reset();
        }

        public void Initialize(string path)
        {
            if ((path == _path) && (_connection != null))
            {
                return;
            }

            _path = path;
            GetConnection();
            CreateTables();
        }

        public void Reset()
        {
            _path = string.Empty;
            if (_connection != null)
            {
                _connection.Reset();
                _connection = null;
            }
        }

        private void GetConnection()
        {
            _connection = new SQLiteAsyncConnection(_path);
        }

        private void CreateTables()
        {
            if (userInfoTableHandler != null)
            {
                userInfoTableHandler.Initialize(_connection);
            }

            if (notebookTableHandler != null)
            {
                notebookTableHandler.Initialize(_connection);
            }

            if (noteTableHandler != null)
            {
                noteTableHandler.Initialize(_connection);
            }
        }
    }
}
