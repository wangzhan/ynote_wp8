/**
 * @file NotebookTableHandler.cs
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
    public class NotebookTableHandler
    {
        private SQLiteAsyncConnection _connection = null;

        public NotebookTableHandler()
        {

        }

        public void Initialize(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            CreateTableAsync();
        }

        private async void CreateTableAsync()
        {
            if (_connection == null)
            {
                return;
            }
            await _connection.CreateTableAsync<NotebookInfoSchema>();
        }

        public async Task<NotebookInfo> QueryNotebookInfo(string id)
        {
            if ((_connection == null) || (string.IsNullOrEmpty(id)))
            {
                return null;
            }

            NotebookInfoSchema schema = await _connection.GetAsync<NotebookInfoSchema>(id);
            if (schema == null)
            {
                return null;
            }

            NotebookInfo notebookInfo = new NotebookInfo();
            SchemaToInfo(schema, ref notebookInfo);
            return notebookInfo;
        }

        public async Task<List<NotebookInfoSchema>> QueryAllNotebookSchemaAsync()
        {
            if (_connection == null)
            {
                return null;
            }

            var values = _connection.Table<NotebookInfoSchema>();
            if (values == null)
            {
                return null;
            }

            List<NotebookInfoSchema> schemas = await values.ToListAsync();
            return schemas;
        }

        public async Task<List<NotebookInfo>> QueryAllNotebookInfoAsync()
        {
            if (_connection == null)
            {
                return null;
            }

            List<NotebookInfo> infos = new List<NotebookInfo>();

            try
            {
                var values = _connection.Table<NotebookInfoSchema>();
                List<NotebookInfoSchema> schemas = await values.ToListAsync();
                foreach (var v in schemas)
                {
                    NotebookInfo info = new NotebookInfo();
                    SchemaToInfo(v, ref info);
                    infos.Add(info);
                }
            }
            catch (System.Exception)
            {
            }

            return infos;
        }

        public async Task UpdateNotebookInfoAsync(NotebookInfoSchema item)
        {
            if ((_connection == null) || (item == null))
            {
                return;
            }

            await _connection.InsertOrReplaceAsync(item);
        }

        public async Task DeleteNotebookInfoAsync(string ID)
        {
            if ((_connection == null) || string.IsNullOrEmpty(ID))
            {
                return;
            }

            await _connection.DeleteAsync<NotebookInfoSchema>(ID);
        }

        private void SchemaToInfo(NotebookInfoSchema schema, ref NotebookInfo info)
        {
            info.ID = schema.ID;
            info.Title = schema.Name;
            info.NoteCountText = string.Format("{0}条笔记", schema.NotesNum);
            info.NoteCount = schema.NotesNum;
            info.CreateTime = schema.CreateTime;
            info.ModifyTime = schema.ModifyTime;
        }
    }
}
