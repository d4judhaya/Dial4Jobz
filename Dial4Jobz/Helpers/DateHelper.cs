using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Collections;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;


namespace Dial4Jobz.Helpers
{
    public class DateHelper
    {
                
        public static string GetFriendlyDate(DateTime dateTime)
        {
            var utcTime = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            TimeSpan ts = utcTime.Subtract(dateTime);
            //TimeSpan ts = DateTime.UtcNow.Subtract(dateTime);
            //TimeSpan ts = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);

            string friendlyDate = dateTime.ToShortDateString();
            int totalDays = (int)System.Math.Round(ts.TotalDays);
            int totalHours = (int)System.Math.Round(ts.TotalHours);
            int totalMinutes = (int)System.Math.Round(ts.TotalMinutes);
            int totalSeconds = (int)System.Math.Round(ts.TotalSeconds);
            int totalMilliSeconds = (int)System.Math.Round(ts.TotalMilliseconds);

            int totalMonths = totalDays / 31;  //approx.. change this
            int totalYears = totalDays / 365; //approx.. change this

            if (totalYears > 0) //give in terms of years
            {
                if (totalYears == 1)
                    friendlyDate = "last year";
                else
                    friendlyDate = totalYears + " years ago";
            }
            else if (totalMonths > 1) //give in terms of months
            {
                if (totalMonths == 1)
                    friendlyDate = "last month";
                else
                    friendlyDate = totalMonths + " months ago";
            }
            else if (totalDays > 1) //give in terms of days (at least 2 days)
            {
                friendlyDate = totalDays + " days ago";
            }
            else if (totalHours > 0) //give in terms of hours
            {
                if (totalHours == 1)
                    friendlyDate = "1 hour ago";
                else
                    friendlyDate = totalHours + " hours ago";
            }
            else if (totalMinutes > 0) // give in terms of minutes
            {
                if (totalMinutes == 1)
                    friendlyDate = "1 minute ago";
                else
                    friendlyDate = totalMinutes + " minutes ago";
            }
            else if (totalSeconds > 0) //give in terms of seconds
            {
                if (totalSeconds == 1)
                    friendlyDate = "1 second ago";
                else
                    friendlyDate = totalSeconds + " seconds ago";
            }
            else //just now
            {
                friendlyDate = "a moment ago";
            }

            return friendlyDate;
        }
    }
    
}