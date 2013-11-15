/**
 * @file NoteTableHandler.cs
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
    public class NoteTableHandler
    {
        private SQLiteAsyncConnection _connection = null;

        public NoteTableHandler()
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
            await _connection.CreateTableAsync<NoteInfoSchema>();
        }

        public async Task<NoteInfo> QueryNoteInfoAsync(string id)
        {
            if ((_connection == null) || string.IsNullOrEmpty(id))
            {
                return null;
            }

            NoteInfoSchema schema = null;
            try
            {
                var loads = _connection.Table<NoteInfoSchema>().Where(v => v.ID == id);
                List<NoteInfoSchema> schemas = await loads.ToListAsync();
                if (schemas.Count > 0)
                {
                    schema = schemas[0];
                }
            }
            catch (System.Exception)
            {
            }
            
            if (schema == null)
            {
                return null;
            }
            NoteInfo noteInfo = new NoteInfo();
            SchemaToInfo(schema, ref noteInfo);
            return noteInfo;
        }

        public async Task<List<NoteInfoSchema>> QueryAllNoteSchemaAsync(string notebookID)
        {
            if (_connection == null)
            {
                return null;
            }

            try
            {
                var loads = _connection.Table<NoteInfoSchema>().Where(v => v.NotebookID == notebookID);
                List<NoteInfoSchema> schemas = await loads.ToListAsync();
                return schemas;
            }
            catch (System.Exception)
            {
            }

            return null;
        }

        public async Task<int> QueryNoteCountByNoteboookAsync(string notebookID)
        {
            int iCount = 0;
            if (_connection == null)
            {
                return iCount;
            }

            try
            {
                var loads = _connection.Table<NoteInfoSchema>().Where(v => v.NotebookID == notebookID);
                iCount = await loads.CountAsync();
                return iCount;
            }
            catch (System.Exception)
            {
            }

            return 0;
        } 

        public async Task<List<NoteInfo>> QueryAllNoteInfoAsync(string notebookID)
        {
            if (_connection == null)
            {
                return null;
            }

            List<NoteInfo> infos = new List<NoteInfo>();

            try
            {
                AsyncTableQuery<NoteInfoSchema> values = null;
                if (string.IsNullOrEmpty(notebookID))
                {
                    values = _connection.Table<NoteInfoSchema>();
                }
                else
                {
                    values = _connection.Table<NoteInfoSchema>().Where(v => v.NotebookID == notebookID);
                }

                List<NoteInfoSchema> schemas = await values.ToListAsync();
                foreach (var v in schemas)
                {
                    NoteInfo noteInfo = new NoteInfo();
                    SchemaToInfo(v, ref noteInfo);
                    infos.Add(noteInfo);
                }
            }
            catch (System.Exception)
            {
            }

            return infos;
        }

        public async Task UpdateNoteInfoAsync(NoteInfoSchema item)
        {
            if ((_connection == null) || (item == null))
            {
                return;
            }

            await _connection.InsertOrReplaceAsync(item);
        }

        public async Task DeleteNoteInfoAsync(string ID)
        {
            if ((_connection == null) || string.IsNullOrEmpty(ID))
            {
                return;
            }

            await _connection.DeleteAsync<NoteInfoSchema>(ID);
        }

        public async Task DeleteNoteInfoByNotebookAsync(string ID)
        {
            if ((_connection == null) || string.IsNullOrEmpty(ID))
            {
                return;
            }

            string sql = string.Format("delete from NoteInfoSchema where NotebookID='{0}'", ID);
            await _connection.ExecuteAsync(sql);
        }

        private void SchemaToInfo(NoteInfoSchema schema, ref NoteInfo info)
        {
            info.ID = schema.ID;
            info.Path = schema.GetPath();
            info.CreatedDate = schema.CreateTime;
            info.ModifiedDate = schema.ModifyTime;
            info.Source = schema.Source;
            info.Thumbnail = null;
            info.ThumbnialExist = false;
            info.Title = schema.Title;
            info.Abstract = schema.Abstract;
            info.Content = schema.Content;
        }

        public async Task<List<NoteIndexInfo>> QueryAllNoteIndexInfoAsync(string notebookID)
        {
            if (_connection == null)
            {
                return null;
            }

            List<NoteIndexInfo> infos = new List<NoteIndexInfo>();

            try
            {
                string sql;
                sql = "select ID, NotebookID, Title, ModifyTime, CreateTime, Source, Abstract from NoteInfoSchema";
                if (!string.IsNullOrEmpty(notebookID))
                {
                    sql += string.Format(" where NotebookID='{0}'", notebookID);
                }

                var values = await _connection.QueryAsync<NoteInfoSchema>(sql);
                foreach (var v in values)
                {
                    NoteIndexInfo noteInfo = new NoteIndexInfo();
                    SchemaToIndexInfo(v, ref noteInfo);
                    infos.Add(noteInfo);
                }
            }
            catch (System.Exception)
            {
            }

            return infos;
        }

        private void SchemaToIndexInfo(NoteInfoSchema schema, ref NoteIndexInfo info)
        {
            info.ID = schema.ID;
            info.Path = schema.GetPath();
            info.CreatedDate = schema.CreateTime;
            info.ModifiedDate = schema.ModifyTime;
            info.Source = schema.Source;
            info.Thumbnail = null;
            info.ThumbnialExist = false;
            info.Title = schema.Title;
            info.Abstract = schema.Abstract;
        }
    }
}
