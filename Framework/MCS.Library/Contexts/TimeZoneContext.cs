using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Library.Core
{
    /// <summary>
    /// 时区相关的上下文
    /// </summary>
    [ActionContextDescription(Key = "TimeZoneContext")]
    public class TimeZoneContext : ActionContextBase<TimeZoneContext>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public TimeZoneContext()
        {
            this.CurrentTimeZone = TimeZoneInfo.Local;
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeZoneInfo CurrentTimeZone
        {
            get;
            set;
        }

        /// <summary>
        /// 从Utc时间转变为对应时区的本地时间
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns></returns>
        public DateTime ConvertTimeFromUtc(DateTime utcTime)
        {
            TimeZoneInfo tz = this.CurrentTimeZone;

            if (tz == null)
                tz = TimeZoneInfo.Local;

            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, tz);
        }

        /// <summary>
        /// 将某个时区的时间转变为Utc时间
        /// </summary>
        /// <param name="localTime"></param>
        /// <returns></returns>
        public DateTime ConvertTimeToUtc(DateTime localTime)
        {
            TimeZoneInfo tz = this.CurrentTimeZone;

            if (tz == null)
                tz = TimeZoneInfo.Local;

            return TimeZoneInfo.ConvertTimeToUtc(localTime, tz);
        }
    }
}
