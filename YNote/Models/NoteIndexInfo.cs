/**
 * @file NoteIndexInfo.cs
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
    public class NoteIndexInfo
    {
        // ID
        private string _id;
        [DataMember(Name="ID")]
        public string ID
        {
            get;
            set;
        }

        // path
        private string _path;
        [DataMember(Name="Path")]
        public string Path
        {
            get;
            set;
        }

        // title
        private string _title;
        [DataMember(Name = "Title")]
        public string Title
        {
            get 
            { 
                if (string.IsNullOrEmpty(_title))
                {
                    _title = "无标题笔记";
                }
                return _title; 
            }
            set { _title = value; }
        }

        // modified date
        private string _modifiedDate;
        [DataMember(Name = "ModifiedDate")]
        public string ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        // created date
        private string _createdDate;
        [DataMember(Name = "CreatedDate")]
        public string CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        // src url
        private string _source;
        [DataMember(Name = "Source")]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        // thumbnail
        private ImageSource _thumbnail;
        public ImageSource Thumbnail
        {
            get { return _thumbnail; }
            set { _thumbnail = value; }
        }

        // abstract
        private string _abstract;
        public string Abstract
        {
            get { return _abstract; }
            set { _abstract = value; }
        }

        // has thumbnail
        private bool _thumbnailExit;
        public bool ThumbnialExist
        {
            get { return _thumbnailExit; }
            set { _thumbnailExit = value; }
        }

        // has abstract
        private bool _abstractExist;
        public bool AbstractExist
        {
            get { return _abstractExist; }
            set { _abstractExist = value; }
        }

        public NoteIndexInfo()
        {
            _id = string.Empty;
            _title = string.Empty;
            _modifiedDate = string.Empty;
            _thumbnail = null;
            _abstract = string.Empty;
            _thumbnailExit = false;
            _abstractExist = true;
        }
    }
}
