using MCS.Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Library.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class SNTPClient
    {
        /// <summary>
        /// SNTP Data Structure Length
        /// </summary>
        private const byte SNTPDataLength = 48;

        /// <summary>
        /// SNTP Data Structure (as described in RFC 2030)
        /// </summary>
        private byte[] _SNTPData = new byte[SNTPDataLength];

        /// <summary>
        /// Offset constants for timestamps in the data structure
        /// </summary>
        private const byte offReferenceID = 12;
        private const byte offReferenceTimestamp = 16;
        private const byte offOriginateTimestamp = 24;
        private const byte offReceiveTimestamp = 32;
        private const byte offTransmitTimestamp = 40;

        /// <summary>
        /// Leap Indicator
        /// </summary>
        public _LeapIndicator LeapIndicator
        {
            get
            {
                // Isolate the two most significant bits
                byte val = (byte)(_SNTPData[0] >> 6);
                switch (val)
                {
                    case 0: return _LeapIndicator.NoWarning;
                    case 1: return _LeapIndicator.LastMinute61;
                    case 2: return _LeapIndicator.LastMinute59;
                    case 3: goto default;
                    default:
                        return _LeapIndicator.Alarm;
                }
            }
        }

        /// <summary>
        /// Version Number
        /// </summary>
        public byte VersionNumber
        {
            get
            {
                // Isolate bits 3 - 5
                return (byte)((_SNTPData[0] & 0x38) >> 3);
            }
        }

        /// <summary>
        /// Mode
        /// </summary>
        public _Mode Mode
        {
            get
            {
                // Isolate bits 0 - 3
                byte val = (byte)(_SNTPData[0] & 0x7);
                switch (val)
                {
                    case 0: goto default;
                    case 6: goto default;
                    case 7: goto default;
                    default:
                        return _Mode.Unknown;
                    case 1:
                        return _Mode.SymmetricActive;
                    case 2:
                        return _Mode.SymmetricPassive;
                    case 3:
                        return _Mode.Client;
                    case 4:
                        return _Mode.Server;
                    case 5:
                        return _Mode.Broadcast;
                }
            }
        }

        /// <summary>
        /// Stratum
        /// </summary>
        public _Stratum Stratum
        {
            get
            {
                byte val = (byte)_SNTPData[1];
                if (val == 0) return _Stratum.Unspecified;
                else
                    if (val == 1) return _Stratum.PrimaryReference;
                    else
                        if (val <= 15) return _Stratum.SecondaryReference;
                        else
                            return _Stratum.Reserved;
            }
        }

        /// <summary>
        /// Poll Interval (in seconds)
        /// </summary>
        public uint PollInterval
        {
            get
            {
                // Thanks to Jim Hollenhorst <hollenho@attbi.com>
                return (uint)(Math.Pow(2, (sbyte)_SNTPData[2]));
            }
        }

        /// <summary>
        /// Precision (in seconds)
        /// </summary>
        public double Precision
        {
            get
            {
                // Thanks to Jim Hollenhorst <hollenho@attbi.com>
                return (Math.Pow(2, (sbyte)_SNTPData[3]));
            }
        }

        /// <summary>
        /// Root Delay (in milliseconds)
        /// </summary>
        public double RootDelay
        {
            get
            {
                int temp = 0;
                temp = 256 * (256 * (256 * _SNTPData[4] + _SNTPData[5]) + _SNTPData[6]) + _SNTPData[7];
                return 1000 * (((double)temp) / 0x10000);
            }
        }

        /// <summary>
        /// Root Dispersion (in milliseconds)
        /// </summary>
        public double RootDispersion
        {
            get
            {
                int temp = 0;
                temp = 256 * (256 * (256 * _SNTPData[8] + _SNTPData[9]) + _SNTPData[10]) + _SNTPData[11];
                return 1000 * (((double)temp) / 0x10000);
            }
        }

        /// <summary>
        /// Reference Identifier
        /// </summary>
        public string ReferenceID
        {
            get
            {
                string val = "";
                switch (Stratum)
                {
                    case _Stratum.Unspecified:
                        goto case _Stratum.PrimaryReference;
                    case _Stratum.PrimaryReference:
                        val += (char)_SNTPData[offReferenceID + 0];
                        val += (char)_SNTPData[offReferenceID + 1];
                        val += (char)_SNTPData[offReferenceID + 2];
                        val += (char)_SNTPData[offReferenceID + 3];
                        break;
                    case _Stratum.SecondaryReference:
                        switch (VersionNumber)
                        {
                            case 3:	// Version 3, Reference ID is an IPv4 address
                                string Address = _SNTPData[offReferenceID + 0].ToString() + "." +
                                                 _SNTPData[offReferenceID + 1].ToString() + "." +
                                                 _SNTPData[offReferenceID + 2].ToString() + "." +
                                                 _SNTPData[offReferenceID + 3].ToString();
                                try
                                {
                                    IPHostEntry Host = Dns.GetHostEntry(Address);
                                    val = Host.HostName + " (" + Address + ")";
                                }
                                catch (Exception)
                                {
                                    val = "N/A";
                                }
                                break;
                            case 4: // Version 4, Reference ID is the timestamp of last update
                                DateTime time = ComputeDate(GetMilliSeconds(offReferenceID));
                                // Take care of the time zone
                                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                                val = (time + offspan).ToString();
                                break;
                            default:
                                val = "N/A";
                                break;
                        }
                        break;
                }
                return val;
            }
        }

        /// <summary>
        /// Reference Timestamp
        /// </summary>
        public DateTime ReferenceTimestamp
        {
            get
            {
                DateTime time = ComputeDate(GetMilliSeconds(offReferenceTimestamp));
                // Take care of the time zone
                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                return time + offspan;
            }
        }

        /// <summary>
        /// Originate Timestamp (T1)
        /// </summary>
        public DateTime OriginateTimestamp
        {
            get
            {
                return ComputeDate(GetMilliSeconds(offOriginateTimestamp));
            }
        }

        /// <summary>
        /// Receive Timestamp (T2)
        /// </summary>
        public DateTime ReceiveTimestamp
        {
            get
            {
                DateTime time = ComputeDate(GetMilliSeconds(offReceiveTimestamp));
                // Take care of the time zone
                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                return time + offspan;
            }
        }

        /// <summary>
        /// Transmit Timestamp (T3)
        /// </summary>
        public DateTime TransmitTimestamp
        {
            get
            {
                DateTime time = ComputeDate(GetMilliSeconds(offTransmitTimestamp));
                // Take care of the time zone
                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                return time + offspan;
            }
            set
            {
                SetDate(offTransmitTimestamp, value);
            }
        }

        /// <summary>
        /// Destination Timestamp (T4)
        /// </summary>
        public DateTime DestinationTimestamp;

        /// <summary>
        /// Round trip delay (in milliseconds)
        /// </summary>
        public int RoundTripDelay
        {
            get
            {
                // Thanks to DNH <dnharris@csrlink.net>
                TimeSpan span = (DestinationTimestamp - OriginateTimestamp) - (ReceiveTimestamp - TransmitTimestamp);
                return (int)span.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Local clock offset (in milliseconds)
        /// </summary>
        public int LocalClockOffset
        {
            get
            {
                // Thanks to DNH <dnharris@csrlink.net>
                TimeSpan span = (ReceiveTimestamp - OriginateTimestamp) + (TransmitTimestamp - DestinationTimestamp);
                return (int)(span.TotalMilliseconds / 2);
            }
        }

        /// <summary>
        /// Compute date, given the number of milliseconds since January 1, 1900
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        private DateTime ComputeDate(ulong milliseconds)
        {
            TimeSpan span = TimeSpan.FromMilliseconds((double)milliseconds);
            DateTime time = new DateTime(1900, 1, 1);
            time += span;

            return time;
        }

        /// <summary>
        /// Compute the number of milliseconds, given the offset of a 8-byte array
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private ulong GetMilliSeconds(byte offset)
        {
            ulong intpart = 0, fractpart = 0;
            for (int i = 0; i <= 3; i++)
            {
                intpart = 256 * intpart + _SNTPData[offset + i];
            }
            for (int i = 4; i <= 7; i++)
            {
                fractpart = 256 * fractpart + _SNTPData[offset + i];
            }

            ulong milliseconds = intpart * 1000 + (fractpart * 1000) / 0x100000000L;

            return milliseconds;
        }

        /// <summary>
        /// Compute the 8-byte array, given the date
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="date"></param>
        private void SetDate(byte offset, DateTime date)
        {
            ulong intpart = 0, fractpart = 0;
            DateTime StartOfCentury = new DateTime(1900, 1, 1, 0, 0, 0);	// January 1, 1900 12:00 AM
            ulong milliseconds = (ulong)(date - StartOfCentury).TotalMilliseconds;

            intpart = milliseconds / 1000;
            fractpart = ((milliseconds % 1000) * 0x100000000L) / 1000;

            ulong temp = intpart;

            for (int i = 3; i >= 0; i--)
            {
                _SNTPData[offset + i] = (byte)(temp % 256);
                temp = temp / 256;
            }

            temp = fractpart;

            for (int i = 7; i >= 4; i--)
            {
                _SNTPData[offset + i] = (byte)(temp % 256);
                temp = temp / 256;
            }
        }

        /// <summary>
        /// Initialize the NTPClient data
        /// </summary>
        private void Initialize()
        {
            // Set version number to 4 and Mode to 3 (client)
            this._SNTPData[0] = 0x1B;
            // Initialize all other fields with 0
            for (int i = 1; i < 48; i++)
                this._SNTPData[i] = 0;

            // Initialize the transmit timestamp
            this.TransmitTimestamp = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        public SNTPClient(string host)
        {
            this.TimeServer = host;
        }

        /// <summary>
        /// Connect to the time server and update system time
        /// </summary>
        /// <param name="updateSystemTime"></param>
        public void Connect(bool updateSystemTime)
        {
            try
            {
                // Resolve server address
                IPHostEntry hostadd = Dns.GetHostEntry(TimeServer);
                IPEndPoint epHost = new IPEndPoint(hostadd.AddressList[0], 123);

                //Connect the time server
                UdpClient timeSocket = new UdpClient();
                timeSocket.Connect(epHost);
                // Initialize data structure

                this.Initialize();

                timeSocket.Send(this._SNTPData, this._SNTPData.Length);
                this._SNTPData = timeSocket.Receive(ref epHost);

                if (IsResponseValid() == false)
                {
                    throw new ApplicationException("Invalid response from " + TimeServer);
                }

                this.DestinationTimestamp = DateTime.Now;
            }
            catch (SocketException e)
            {
                throw new Exception(e.Message);
            }

            // Update system time
            if (updateSystemTime)
            {
                this.SetTime();
            }
        }

        /// <summary>
        /// Check if the response from server is valid
        /// </summary>
        /// <returns></returns>
        public bool IsResponseValid()
        {
            if (_SNTPData.Length < SNTPDataLength || Mode != _Mode.Server)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Converts the object to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str;
            str = "Leap Indicator: ";

            switch (LeapIndicator)
            {
                case _LeapIndicator.NoWarning:
                    str += "No warning";
                    break;
                case _LeapIndicator.LastMinute61:
                    str += "Last minute has 61 seconds";
                    break;
                case _LeapIndicator.LastMinute59:
                    str += "Last minute has 59 seconds";
                    break;
                case _LeapIndicator.Alarm:
                    str += "Alarm Condition (clock not synchronized)";
                    break;
            }
            str += "\r\nVersion number: " + VersionNumber.ToString() + "\r\n";
            str += "Mode: ";
            switch (Mode)
            {
                case _Mode.Unknown:
                    str += "Unknown";
                    break;
                case _Mode.SymmetricActive:
                    str += "Symmetric Active";
                    break;
                case _Mode.SymmetricPassive:
                    str += "Symmetric Pasive";
                    break;
                case _Mode.Client:
                    str += "Client";
                    break;
                case _Mode.Server:
                    str += "Server";
                    break;
                case _Mode.Broadcast:
                    str += "Broadcast";
                    break;
            }
            str += "\r\nStratum: ";
            switch (Stratum)
            {
                case _Stratum.Unspecified:
                case _Stratum.Reserved:
                    str += "Unspecified";
                    break;
                case _Stratum.PrimaryReference:
                    str += "Primary Reference";
                    break;
                case _Stratum.SecondaryReference:
                    str += "Secondary Reference";
                    break;
            }
            str += "\r\nLocal time: " + TransmitTimestamp.ToString();
            str += "\r\nPrecision: " + Precision.ToString() + " s";
            str += "\r\nPoll Interval: " + PollInterval.ToString() + " s";
            str += "\r\nReference ID: " + ReferenceID.ToString();
            str += "\r\nRoot Delay: " + RootDelay.ToString() + " ms";
            str += "\r\nRoot Dispersion: " + RootDispersion.ToString() + " ms";
            str += "\r\nRound Trip Delay: " + RoundTripDelay.ToString() + " ms";
            str += "\r\nLocal Clock Offset: " + LocalClockOffset.ToString() + " ms";
            str += "\r\n";

            return str;
        }

        /// <summary>
        /// Set system time according to transmit timestamp
        /// </summary>
        private void SetTime()
        {
            NativeMethods.SYSTEMTIME st;

            // Thanks to Jim Hollenhorst <hollenho@attbi.com>
            DateTime trts = DateTime.Now.AddMilliseconds(LocalClockOffset);
            st.year = (short)trts.Year;
            st.month = (short)trts.Month;
            st.dayOfWeek = (short)trts.DayOfWeek;
            st.day = (short)trts.Day;
            st.hour = (short)trts.Hour;
            st.minute = (short)trts.Minute;
            st.second = (short)trts.Second;
            st.milliseconds = (short)trts.Millisecond;
            
            NativeMethods.SetLocalTime(ref st);
        }

        /// <summary>
        /// The URL of the time server we're connecting to
        /// </summary>
        private string TimeServer;
    }
}


