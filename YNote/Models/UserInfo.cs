/**
 * @file UserInfo.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNote.Models
{
    public class UserInfo
    {
        public string ClientVersion { get; set; }
        public string User { get; set; }
        public string TotalSize { get; set; }
        public string UsedSize { get; set; }
        public string RegisterTime { get; set; }
        public string LastLoginTime { get; set; }
        public string LastModifyTime { get; set; }
        public string DefaultNotebook { get; set; }

        public UserInfo()
        {

        }
    }
}
