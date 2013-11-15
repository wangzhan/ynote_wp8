/**
 * @file NoteInfo.cs
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
    public class NoteInfo : NoteIndexInfo
    {
        // content
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public NoteInfo()
        {

        }
    }
}
