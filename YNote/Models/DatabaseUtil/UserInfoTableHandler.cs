/**
 * @file UserInfoTableHandler.cs
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
    public class UserInfoTableHandler
    {
        private SQLiteAsyncConnection _connection = null;

        private const string ClientVersionKey = "ClientVersion";
        private const string UserKey = "User";
        private const string TotalSizeKey = "TotalSize";
        private const string UsedSizeKey = "UsedSize";
        private const string RegisterTimeKey = "RegisterTime";
        private const string LastLoginTimeKey = "LastLoginTime";
        private const string LastModifyTimeKey = "LastModifyTime";
        private const string DefaultNotebookKey = "DefaultNotebook";

        public UserInfoTableHandler()
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
            var result = await _connection.CreateTableAsync<UserInfoSchema>();
        }

        public async Task<UserInfo> QueryUserInfoAsync()
        {
            if (_connection == null)
            {
                return null;
            }

            var query = _connection.Table<UserInfoSchema>();
            var loaded = await query.ToListAsync();
            if (loaded.Count == 0)
            {
                return null;
            }

            UserInfo userInfo = new UserInfo();
            userInfo.ClientVersion = GetValue(ClientVersionKey, loaded);
            userInfo.User = GetValue(UserKey, loaded);
            userInfo.TotalSize = GetValue(TotalSizeKey, loaded);
            userInfo.UsedSize = GetValue(UsedSizeKey, loaded);
            userInfo.RegisterTime = GetValue(RegisterTimeKey, loaded);
            userInfo.LastLoginTime = GetValue(LastLoginTimeKey, loaded);
            userInfo.LastModifyTime = GetValue(LastModifyTimeKey, loaded);
            userInfo.DefaultNotebook = GetValue(DefaultNotebookKey, loaded);
            return userInfo;
        }

        private string GetValue(string key, IList<UserInfoSchema> userInfoList)
        {
            string value = string.Empty;
            foreach (var v in userInfoList)
            {
                if (v.Key == key)
                {
                    value = v.Value;
                }
            }
            return value;
        }

        public async void UpdateUserInfoAsync(UserInfo userInfo)
        {
            if ((_connection == null) || (userInfo == null))
            {
                return;
            }

            IList<UserInfoSchema> userInfoList = new List<UserInfoSchema>();
            userInfoList.Add(new UserInfoSchema(ClientVersionKey, userInfo.ClientVersion));
            userInfoList.Add(new UserInfoSchema(UserKey, userInfo.User));
            userInfoList.Add(new UserInfoSchema(TotalSizeKey, userInfo.TotalSize));
            userInfoList.Add(new UserInfoSchema(UsedSizeKey, userInfo.UsedSize));
            userInfoList.Add(new UserInfoSchema(RegisterTimeKey, userInfo.RegisterTime));
            userInfoList.Add(new UserInfoSchema(LastLoginTimeKey, userInfo.LastLoginTime));
            userInfoList.Add(new UserInfoSchema(LastModifyTimeKey, userInfo.LastModifyTime));
            userInfoList.Add(new UserInfoSchema(DefaultNotebookKey, userInfo.DefaultNotebook));
            var result = await _connection.UpdateAllAsync(userInfoList);
        }
    }
}
