/**
 * @file NotebookInfo.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace YNote.Models
{
    public class NotebookInfo
    {
        // ID
        private string _id;
        [DataMember(Name = "ID")]
        public string ID
        {
            get;
            set;
        }

        // title
        private string _title;
        [DataMember(Name = "Title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        // note count
        private string _noteCount;
        public string NoteCount
        {
            get;
            set;
        }

        // note count text
        private string _noteCountText;
        public string NoteCountText
        {
            get { return _noteCountText; }
            set { _noteCountText = value; }
        }

        // unit: s
        private string _createTime;
        public string CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; } 
        }

        // unit: s
        private string _modifyTime;
        public string ModifyTime
        {
            get { return _modifyTime; }
            set { _modifyTime = value; }
        }

        public NotebookInfo()
        {
            _id = string.Empty;
            _title = string.Empty;
            _noteCountText = string.Empty;
            _createTime = string.Empty;
            _modifyTime = string.Empty;
        }
    }
}
