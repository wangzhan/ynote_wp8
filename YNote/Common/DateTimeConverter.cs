/**
 * @file DataTimeConverter.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace YNote.Common
{
    /// <summary>
    /// Value converter that translates true to <see cref="DateTime"/> and false to
    /// </summary>
    public sealed class DateTimeConverter : IValueConverter
    {
        private System.Globalization.CultureInfo zhCulture = new System.Globalization.CultureInfo("zh-CN");
        private DateTime startTime = new DateTime(1970, 1, 1);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value == null) || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }

            DateTime curTime = startTime.AddSeconds(double.Parse(value.ToString()));
            if (curTime == DateTime.MinValue)
            {
                return null;
            }

            return curTime.ToString((string)parameter, zhCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string val = (string)value;
            DateTime outDate;
            if (DateTime.TryParse(val, zhCulture, DateTimeStyles.None, out outDate))
            {
                outDate.Subtract(startTime);
                return outDate.Second;
            }

            return null;
        }
    }
}
