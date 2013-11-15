/**
 * @file NotebookInfoJson.cs
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
    public class NotebookInfoJson
    {
        [DataMember]
        public string path { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string notes_num { get; set; }

        [DataMember]
        public string create_time { get; set; }

        [DataMember]
        public string modify_time { get; set; }
    }

    [CollectionDataContract]
    public class NotebookInfoListJson : List<NotebookInfoJson>
    {

    }
}
