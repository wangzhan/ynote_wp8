/**
 * @file JsonHelper.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace YNote.Common
{
    public class JsonHelper
    {
        public static T ParseJson<T>(string value)
        {
            DataContractJsonSerializer jsonSerializeer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
            T obj = (T)jsonSerializeer.ReadObject(stream);
            return obj;
        }

        public static string SerializeJson<T>(T value)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, value);

            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes, 0, dataBytes.Length);
        }
    }
}
