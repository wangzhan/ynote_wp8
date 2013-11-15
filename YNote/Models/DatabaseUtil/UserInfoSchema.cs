/**
 * @file UserInfoSchema.cs
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
    public class UserInfoSchema
    {
        [PrimaryKey]
        public string Key { get; set; }

        public string Value{ get; set; }

        public UserInfoSchema()
        {

        }

        public UserInfoSchema(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
