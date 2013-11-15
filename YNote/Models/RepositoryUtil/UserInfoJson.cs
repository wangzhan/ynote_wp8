/**
 * @file UserInfoJson.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace YNote.Models.RepositoryUtil
{
    [DataContract]
    public class UserInfoJson
    {
        [DataMember]
        public string user { get; set; }

        [DataMember]
        public string total_size { get; set; }

        [DataMember]
        public string used_size { get; set; }

        [DataMember]
        public string register_time { get; set; }

        [DataMember]
        public string last_login_time { get; set; }

        [DataMember]
        public string last_modify_time { get; set; }

        [DataMember]
        public string default_notebook { get; set; }
    }
}
