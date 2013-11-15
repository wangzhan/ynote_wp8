/**
 * @file PathHelper.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNote.Common
{
    class PathHelper
    {
        public static string ExtractNoteIDFromNotePath(string notePath)
        {
            string id = string.Empty;
            try
            {
                var ids = notePath.Split('/');
                id = ids[2];
            }
            catch (System.Exception)
            {
            }
            return id;
        }

        public static string ExtractNotebookIDFromNotePath(string notePath)
        {
            string id = string.Empty;
            try
            {
                var ids = notePath.Split('/');
                id = ids[1];
            }
            catch (System.Exception)
            {
            }
            return id;
        }

        public static string ExtractNotebookIDFromNotebookPath(string notebookPath)
        {
            string id = string.Empty;
            try
            {
                id = notebookPath.Trim('/');
            }
            catch (System.Exception)
            {
            }
            return id;
        }
    }
}
