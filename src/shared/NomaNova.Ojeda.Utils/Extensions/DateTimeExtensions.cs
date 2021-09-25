using System;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToRelativeTime(this DateTime date, ITimeKeeper timeKeeper)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var ts = new TimeSpan(timeKeeper.UtcNow.Ticks - date.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            switch (delta)
            {
                case < 1 * second:
                    return "Right now";
                case < 5 * second:
                    return "A few seconds ago";
                case < 1 * minute:
                    return ts.Seconds + " seconds ago";
                case < 2 * minute:
                    return "A minute ago";
                case < 1 * hour:
                    return ts.Minutes + " minutes ago";
                case < 2 * hour:
                    return "An hour ago";
                case < 24 * hour:
                    return ts.Hours + " hours ago";
                case < 48 * hour:
                    return "Yesterday";
                case < 30 * day:
                    return ts.Days + " days ago";
                case < 12 * month:
                {
                    var months = Convert.ToInt32(Math.Floor((double) ts.Days / 30));
                    return months <= 1 ? "One month ago" : months + " months ago";
                }
                default:
                {
                    var years = Convert.ToInt32(Math.Floor((double) ts.Days / 365));
                    return years <= 1 ? "One year ago" : years + " years ago";
                }
            }
        }
    }
}