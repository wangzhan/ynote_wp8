/**
 * @file NoteInfoSchema.cs
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
    public class NoteInfoSchema
    {
        [PrimaryKeyAttribute]
        public string ID { get; set; }
        public string NotebookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Size { get; set; }
        public string CreateTime { get; set; }
        public string ModifyTime { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public bool Dirty { get; set; }
        public int State { get; set; }
        public string Abstract{ get; set; }

        public NoteInfoSchema()
        {
            Dirty = false;
        }

        public string GetPath()
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(NotebookID))
            {
                path = string.Format("/{0}/{1}", NotebookID, ID);
            }
            return path;
        }

        public void SetPath(string path)
        {
            var values = path.Split('/');
            NotebookID = values[1];
            ID = values[2];
        }
    }
}
