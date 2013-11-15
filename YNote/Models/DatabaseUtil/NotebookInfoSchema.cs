/**
 * @file NotebookInfoSchema.cs
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
    public class NotebookInfoSchema
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string Name { get; set; }
        public string NotesNum { get; set; }
        public string CreateTime { get; set; }
        public string ModifyTime { get; set; }
        public bool Dirty { get; set; }
        public int State { get; set; }

        public NotebookInfoSchema()
        {
            Dirty = false;
        }

        public string GetPath()
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(ID))
            {
                path = string.Format("/{0}", ID);
            }
            return path;
        }

        public void SetPath(string path)
        {
            ID = path.TrimStart('/');
        }
    }
}
