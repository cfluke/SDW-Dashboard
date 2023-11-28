using System;
using System.Globalization;
using SerializableData;
using TMPro;
using UnityEngine;

namespace Widgets.Clock
{
    public class ClockWidget : Widget
    {
        public enum ClockFormat
        {
            Local,
            UTC,
            Julian,
            Sidereal
        }
    
        [SerializeField] private TMP_Text text;
        private ClockFormat _format = ClockFormat.Local;

        private void Update()
        {
            UpdateClockText();
        }
    
        public void NextClockFormat()
        {
            int formatCount = Enum.GetNames(typeof(ClockFormat)).Length;
            int nextFormatIndex = ((int)_format + 1) % formatCount; // mod by formatCount to restrict indexes between 0-length
            _format = (ClockFormat)nextFormatIndex;
        }

        private void UpdateClockText()
        {
            // Update the clock text based on the selected format
            switch (_format)
            {
                case ClockFormat.Local:
                    text.text = "Local Time\n" + GetLocal().ToString(CultureInfo.CurrentCulture);
                    break;
                case ClockFormat.UTC:
                    text.text = "UTC\n" + GetUTC().ToString(CultureInfo.CurrentCulture);
                    break;
                case ClockFormat.Julian:
                    text.text = "Julian\n" + GetJulian();
                    break;
                case ClockFormat.Sidereal:
                    text.text = "Sidereal\n" + ConvertDegreesToTime(GetSidereal(145.3425f)); // Swinburne Uni Longitude
                    break;
            }
        }

        private DateTime GetLocal()
        {
            return DateTime.Now;
        }

        private DateTime GetUTC()
        {
            return DateTime.UtcNow;
        }
    
        public static double GetSidereal(double longitude)
        {
            const double siderealSecondsPerDay = 86400.0;
        
            // Calculate the Julian Day
            double julianDay = GetJulian();
            double t = (julianDay - 2451545.0) / 36525.0; // Calculate the number of centuries since J2000.0

            // Calculate the GMST (Greenwich Mean Sidereal Time) in seconds
            double gmstSeconds = 280.46061837 + 360.98564736629 * (julianDay - 2451545.0) + 0.000387933 * t * t - t * t * t / 38710000.0;
            gmstSeconds %= siderealSecondsPerDay;

            // Calculate the Local Sidereal Time (LST) at the given longitude
            double lstSeconds = gmstSeconds + longitude * siderealSecondsPerDay / 360.0;
            lstSeconds %= siderealSecondsPerDay;

            // return hours
            return lstSeconds * 24.0 / siderealSecondsPerDay;
        }

        private static double GetJulian()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;

            if (month <= 2)
            {
                year--;
                month += 12;
            }

            int a = year / 100;
            int b = 2 - a + a / 4;

            // return Julian Day
            return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5;
        }
    
        public static TimeSpan ConvertDegreesToTime(double degrees)
        {
            // Convert degrees to seconds
            double totalSeconds = degrees * 240.0; // 1 degree = 240 seconds

            // Calculate hours, minutes, and seconds
            int hours = (int)(totalSeconds / 3600);
            int minutes = (int)((totalSeconds % 3600) / 60);
            int seconds = (int)(totalSeconds % 60);

            // Create a TimeSpan object
            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);

            return timeSpan;
        }
    
        public override WidgetData Serialize()
        {
            return new ClockWidgetData(base.Serialize())
            {
                format = (int)_format
            };
        }

        public override void Deserialize(WidgetData widgetData)
        {
            base.Deserialize(widgetData);
            _format = (ClockFormat)((ClockWidgetData)widgetData).format;
        }
    }
}
