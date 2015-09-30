using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Library.Net
{
    /// <summary>
    /// Leap indicator field values
    /// </summary>
    public enum _LeapIndicator
    {
        /// <summary>
        /// 0 - No warning
        /// </summary>
        NoWarning,

        /// <summary>
        /// 1 - Last minute has 61 seconds
        /// </summary>
        LastMinute61,

        /// <summary>
        /// 2 - Last minute has 59 seconds
        /// </summary>
        LastMinute59,

        /// <summary>
        /// 3 - Alarm condition (clock not synchronized)
        /// </summary>
        Alarm
    }

    /// <summary>
    /// Mode field values
    /// </summary>
    public enum _Mode
    {
        /// <summary>
        /// 1 - Symmetric active
        /// </summary>
        SymmetricActive,

        /// <summary>
        /// 2 - Symmetric pasive
        /// </summary>
        SymmetricPassive,

        /// <summary>
        /// 3 - Client
        /// </summary>
        Client,

        /// <summary>
        /// 4 - Server
        /// </summary>
        Server,

        /// <summary>
        /// 5 - Broadcast
        /// </summary>
        Broadcast,

        /// <summary>
        /// 0, 6, 7 - Reserved
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Stratum field values
    /// </summary>
    public enum _Stratum
    {
        /// <summary>
        /// 0 - unspecified or unavailable
        /// </summary>
        Unspecified,

        /// <summary>
        /// 1 - primary reference (e.g. radio-clock)
        /// </summary>
        PrimaryReference,

        /// <summary>
        /// 2-15 - secondary reference (via NTP or SNTP)
        /// </summary>
        SecondaryReference,

        /// <summary>
        /// 16-255 - reserved
        /// </summary>
        Reserved
    }
}
