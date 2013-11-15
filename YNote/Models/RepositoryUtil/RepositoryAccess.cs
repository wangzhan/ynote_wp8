/**
 * @file RepositoryAccess.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using YNote.OAuth;
using YNote.Common;
using System.IO;

namespace YNote.Models.RepositoryUtil
{
    public class RepositoryAccess
    {
        private OAuthController _oauthController = App.oauthController;

        private const string _contentTypeKey = "Content-Type";
        private const string _contentTypeSingleForm = "application/x-www-form-urlencoded";
        private const string _contentTypeMultipart = "multipart/form-data";

        public RepositoryAccess()
        {

        }

        /// <summary>
        /// Get user info from the server
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfoJson> GetUserInfo()
        {
            string requestUrl = "http://note.youdao.com/yws/open/user/get.json";
            string result = string.Empty;
            UserInfoJson userInfoJson = null;
            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams = await _oauthController.ConstructOpenAPIHeaders(requestUrl, "GET", queryParams);
            requestUrl += "?" + HttpHelper.ToQueryString(queryParams);
            result = await HttpHelper.HttpGet(requestUrl);
            if (!string.IsNullOrEmpty(result))
            {
                userInfoJson = JsonHelper.ParseJson<UserInfoJson>(result);
            }

            return userInfoJson;
        }

        public async Task<NotebookInfoListJson> ListNotebooks()
        {
            string requestUrl = "http://note.youdao.com/yws/open/notebook/all.json";
            string result = string.Empty;
            string content = string.Empty;
            NotebookInfoListJson lstNotebookInfoJson = null;
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(_contentTypeKey, _contentTypeSingleForm);

            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams = await _oauthController.ConstructOpenAPIHeaders(requestUrl, "POST", queryParams);
            content = HttpHelper.ToQueryString(queryParams);
            result = await HttpHelper.HttpPost(requestUrl, headers, content);
            if (!string.IsNullOrEmpty(result))
            {
                lstNotebookInfoJson = JsonHelper.ParseJson<NotebookInfoListJson>(result);
            }
            return lstNotebookInfoJson;
        }

        public async Task<NotePathListJson> ListNotes(string notebookPath)
        {
            string requestUrl = "http://note.youdao.com/yws/open/notebook/list.json";
            string result = string.Empty;
            string content = string.Empty;
            NotePathListJson notePathListJson = null;
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(_contentTypeKey, _contentTypeSingleForm);

            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("notebook", _oauthController.UrlEncode(notebookPath));
            queryParams = await _oauthController.ConstructOpenAPIHeaders(requestUrl, "POST", queryParams);
            content = HttpHelper.ToQueryString(queryParams);
            result = await HttpHelper.HttpPost(requestUrl, headers, content);
            if (!string.IsNullOrEmpty(result))
            {
                notePathListJson = JsonHelper.ParseJson<NotePathListJson>(result);
            }
            return notePathListJson;
        }

        public async Task<NoteInfoJson> GetNoteInfo(string notePath)
        {
            NoteInfoJson noteInfoJson = null;
            string requestUrl = "http://note.youdao.com/yws/open/note/get.json";
            string result = string.Empty;
            string content = string.Empty;
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(_contentTypeKey, _contentTypeSingleForm);

            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("path", HttpHelper.UrlEncode(notePath));
            queryParams = await _oauthController.ConstructOpenAPIHeaders(requestUrl, "POST", queryParams);
            content = HttpHelper.ToQueryString(queryParams);
            result = await HttpHelper.HttpPost(requestUrl, headers, content);
            if (!string.IsNullOrEmpty(result))
            {
                noteInfoJson = JsonHelper.ParseJson<NoteInfoJson>(result);
            }
            return noteInfoJson;
        }

        public async Task<Stream> GetAttach(string url)
        {
            string requestUrl = url;
            Stream result = null;
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(_contentTypeKey, _contentTypeSingleForm);

            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams = await _oauthController.ConstructOpenAPIHeaders(requestUrl, "GET", queryParams);
            requestUrl += "?" + HttpHelper.ToQueryString(queryParams);
            result = await HttpHelper.HttpGetStream(requestUrl);
            return result;
        }
    }
}
