/**
 * @file AppFolderController.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace YNote.Models
{
    public class AppFolderController
    {
        private string _basePath;
        public string BasePath
        {
            get { return _basePath; }
            set { _basePath = value; }
        }

        // C:\Data\Users\DefApps\AppData\{02A6F30E-XXXX-46BE-92D2-8D298C2EB240}\Local\Database\yno111.db
        private string _databasePath;
        public string DatabasePath
        {
            get { return string.Format("{0}\\{1}", _databasePath, _user); }
            set { _databasePath = value; }
        }

        private string _imagesPath;
        public string ImagesPath
        {
            get { return _imagesPath; }
            set { _imagesPath = value; }
        }

        private string _user = string.Empty;
        private string _databaseName = "Database";
        private string _attachName = "Attaches";
        
        /// <summary>
        /// Construct the app folders
        /// </summary>
        /// <param name="user"></param>
        public async Task InitializeFoldersAsync(string user)
        {
            user = user.Replace('*', '1');
            _user = user + ".db";

            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // C:\Data\Users\DefApps\AppData\{02A6F30E-XXXX-46BE-92D2-8D298C2EB240}\Local
            _basePath = localFolder.Path;

            // C:\Data\Users\DefApps\AppData\{02A6F30E-XXXX-46BE-92D2-8D298C2EB240}\Local\Database
            _databasePath = string.Format("{0}\\{1}", _basePath, _databaseName);

            // C:\Data\Users\DefApps\AppData\{02A6F30E-XXXX-46BE-92D2-8D298C2EB240}\Local\Attaches
            _imagesPath = string.Format("{0}\\{1}", _basePath, _attachName);

            // TODO: for test
            //await DeleteFiles();

            try
            {
                // Database
                await localFolder.CreateFolderAsync(_databaseName, CreationCollisionOption.OpenIfExists);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                // Images
                await localFolder.CreateFolderAsync(_attachName, CreationCollisionOption.OpenIfExists);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteFiles()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                // database file
                StorageFolder databaseFolder = await localFolder.GetFolderAsync(_databaseName);
                await databaseFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                // images file
                StorageFolder imagesFolder = await localFolder.GetFolderAsync(_attachName);
                await imagesFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<ulong> GetSize()
        {
            ulong size = 0;
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Get database file size
            StorageFolder databaseFolder = await localFolder.GetFolderAsync(_databaseName);
            size += await GetFolderSize(databaseFolder);

            // Get images file size
            StorageFolder imagesFolder = await localFolder.GetFolderAsync(_attachName);
            size += await GetFolderSize(imagesFolder);
            return size;
        }

        public async Task SaveFile(string path, Stream content)
        {
            // split path
            string[] dirsPath = path.Split('\\');

            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFolder attachesFolder = await localFolder.GetFolderAsync(_attachName);

            // create dirs
            for (int i = 0; i < dirsPath.Length - 1; ++i)
            {
                attachesFolder = await attachesFolder.CreateFolderAsync(dirsPath[i], CreationCollisionOption.OpenIfExists);
            }

            // save file
            string fileName = dirsPath[dirsPath.Length - 1];
            using (Stream writeStream = await attachesFolder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting))
            {
                await content.CopyToAsync(writeStream);
            }
        }

        public async Task SaveFile(string path, string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                await SaveFile(path, stream);
            }
        }

        public async Task<ulong> GetFolderSize(IStorageFolder folder)
        {
            ulong size = 0;
            try
            {
                // 计算文件的大小
                IReadOnlyList<IStorageFile> files = await folder.GetFilesAsync();
                foreach (var fileItem in files)
                {
                    var propertis = await fileItem.GetBasicPropertiesAsync();
                    size += propertis.Size;
                }

                // 递归记录目录的大小
                IReadOnlyList<IStorageFolder> folders = await folder.GetFoldersAsync();
                foreach (var folderItem in folders)
                {
                    size += await GetFolderSize(folderItem);
                }
            }
            catch (System.Exception)
            {
            }
            return size;
        }

        public async Task DeleteNoteFiles(string noteID)
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                // images file for specified note
                StorageFolder imagesFolder = await localFolder.GetFolderAsync(_attachName);
                StorageFolder noteFolder = await localFolder.GetFolderAsync(noteID);
                await noteFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
