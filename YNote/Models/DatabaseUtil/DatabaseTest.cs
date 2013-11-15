/**
 * @file DatabaseTest.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;
using System.IO;

namespace YNote.Models.DatabaseUtil
{
    // test
    public class TestDB
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
    }

    class DatabaseTest
    {
        private SQLiteAsyncConnection _connection = null;
        private SQLiteConnection _connection2 = null;
        private string _path = string.Empty;

        public async void Init()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            _path = localFolder.Path + "\\yno.db";

            try
            {
                var res = await localFolder.GetFileAsync(_path);
                if (res != null)
                {
                    await res.DeleteAsync();
                }
            }
            catch (System.Exception)
            {
            }

            _connection2 = new SQLiteConnection(_path);
            int iRes = _connection2.CreateTable<TestDB>();


            _connection = new SQLiteAsyncConnection(_path);
            if (_connection != null)
            {
                await _connection.CreateTableAsync<TestDB>();
            }

            int a = 1;
            a++;
        }
    }
}
