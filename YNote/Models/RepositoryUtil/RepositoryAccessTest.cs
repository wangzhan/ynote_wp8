/**
 * @file RepositoryAccessTest.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNote.Models.RepositoryUtil
{
    public class RepositoryAccessTest
    {
        public async void TestRepositoryAccess()
        {
            RepositoryAccess access = new RepositoryAccess();

            // test get user info
            UserInfoJson userInfo = await access.GetUserInfo();

            // Folder operation
            AppFolderController controller = new AppFolderController();
            await controller.InitializeFoldersAsync(userInfo.user);
            
            // test list notebooks
            NotebookInfoListJson notebooks = await access.ListNotebooks();
            
            // test list notes in one notebook
            string notebookPath = userInfo.default_notebook;
            notebookPath = notebookPath.TrimStart('/');
            NotePathListJson notePathList = await access.ListNotes(notebookPath);

            // test get note info
            foreach (string s in notePathList)
            {
                NoteInfoJson noteInfo = await access.GetNoteInfo(s);
            }
        }
    }
}
