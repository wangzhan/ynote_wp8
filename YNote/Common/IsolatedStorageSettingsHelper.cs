/**
 * @file IsolatedStorageSettingsHelper.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace YNote.Common
{
    public class IsolatedStorageSettingsHelper
    {
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;
            if (settings.Contains(Key))
            {
                if (settings[Key] != value)
                {
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }

            if (valueChanged)
            {
                Save();
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public static void Remove(string Key)
        {
            if (settings.Contains(Key))
            {
                settings.Remove(Key);
            }
        }

        public static void RemoveAll()
        {
            settings.Clear();
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public static void Save()
        {
            settings.Save();
        }
    }
}
