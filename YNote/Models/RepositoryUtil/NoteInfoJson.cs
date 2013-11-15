/**
 * @file NoteInfoJson.cs
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
    public class NoteInfoJson
    {
        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string author { get; set; }

        [DataMember]
        public string source { get; set; }

        [DataMember]
        public string size { get; set; }

        [DataMember]
        public string create_time { get; set; }

        [DataMember]
        public string modify_time { get; set; }

        [DataMember]
        public string content { get; set; }

        [DataMember]
        public string thumbnail { get; set; }
    }

    [CollectionDataContract]
    public class NotePathListJson : List<string>
    {

    }
}
