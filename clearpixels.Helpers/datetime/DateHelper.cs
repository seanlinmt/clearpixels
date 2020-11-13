
/*
 * LICENSE NOTE:
 *
 * Copyright  2012-2013 Clear Pixels Limited, All Rights Reserved.
 *
 * Unless explicitly acquired and licensed from Licensor under another license, the
 * contents of this file are subject to the Reciprocal Public License ("RPL")
 * Version 1.5, or subsequent versions as allowed by the RPL, and You may not copy
 * or use this file in either source code or executable form, except in compliance
 * with the terms and conditions of the RPL. 
 *
 * All software distributed under the RPL is provided strictly on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND LICENSOR HEREBY
 * DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT, OR
 * NON-INFRINGEMENT. See the RPL for specific language governing rights and
 * limitations under the RPL.
 *
 * @author         Sean Lin Meng Teck <seanlinmt@clearpixels.co.nz>
 * @copyright      2012-2013 Clear Pixels Limited
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace clearpixels.Helpers.datetime
{
    public static class DateHelper
    {
        public const string DATEFORMAT_DATEPICKER = "ddd, d MMM yyyy";
        public const string DATEFORMAT_DATEPICKER_SHORT = "d MMM yyyy";
        public const string DATETIME_DAYMONTH = "ddd, dd MMM";
        public const string DATETIME_STANDARD = "dd MMMM yyyy";
        public const string DATETIME_SHORT_DATE = "dd MMM yyyy";
        public const string DATETIME_SHORT_DATE_DOTTED = "dd.MM.yy";
        public const string DATETIME_STANDARD_TIME = "h:mm tt";
        public const string DATETIME_FULL = "h:mm tt, dd MMMM yyyy";
        public const string DATETIME_SHORTTIME = "hh\\:mm";

        public static DateTime GetLastBusinessDay(int year, int month)
        {
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
            {
                return lastDayOfMonth.AddDays(-2);
            }

            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
            {
                return lastDayOfMonth.AddDays(-1);
            }

            return lastDayOfMonth;
        }

        public static bool IsWeekEnd(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }

            return false;
        }


        public static IEnumerable<SelectListItem> GetDayList(int? selectedDay = null, bool emptyFirstOption = false)
        {
            var dayList = new List<SelectListItem>();
            if (emptyFirstOption)
            {
                dayList.Add(new SelectListItem() { Text = "", Value = "" });
            }
            for (int i = 1; i <= 31; i++)
            {
                var val = i.ToString();
                var day = new SelectListItem()
                {
                    Text = val,
                    Value = val,
                    Selected = selectedDay.HasValue && selectedDay.Value == i
                };
                dayList.Add(day);
            }
            return dayList;
        }

        public static IEnumerable<SelectListItem> GetMonthList(int? selectedMonth = null, bool emptyFirstOption = false)
        {
            var monthList = new List<SelectListItem>();
            if (emptyFirstOption)
            {
                monthList.Add(new SelectListItem() { Text = "", Value = "" });
            }
            foreach (Month entry in Enum.GetValues(typeof(Month)))
            {
                var month = new SelectListItem()
                {
                    Text = entry.ToString(),
                    Value = entry.ToInt().ToString(),
                    Selected = selectedMonth.HasValue && selectedMonth.Value == entry.ToInt()
                };
                monthList.Add(month);
            }
            return monthList;
        }

        public static DateTime GetNextWeekDay(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                    return date.AddDays(1);
                case DayOfWeek.Friday:
                    return date.AddDays(3);
                case DayOfWeek.Saturday:
                    return date.AddDays(2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static DateTime GetPreviousWeekDay(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return date.AddDays(-2);
                case DayOfWeek.Monday:
                    return date.AddDays(-3);
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                case DayOfWeek.Saturday:
                    return date.AddDays(-1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static int GetTotalWeekDays(long starttick, long endtick)
        {
            return GetTotalWeekDays(new DateTime(starttick).Date, new DateTime(endtick).Date);
        }

        public static int GetTotalWeekDays(DateTime start, DateTime end)
        {
            int dowStart = ((int)start.DayOfWeek == 0 ? 7 : (int)start.DayOfWeek);
            int dowEnd = ((int)end.DayOfWeek == 0 ? 7 : (int)end.DayOfWeek);
            TimeSpan tSpan = end - start;
            if (dowStart <= dowEnd)
            {
                return (((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
            }
            else
            {
                return (((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));
            }
        }

        // no properties smaller than a day to make it easy to compare db datetimes
        public static DateTime ToDayDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static string ToReadableString(this TimeSpan span)
        {
            return string.Format("{0:D} {1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
        }

        public static string ToString(this DateTime? date, string format)
        {
            if (!date.HasValue)
            {
                return "";
            }

            return date.Value.ToString(format);
        }

        public static string ToShortDate(this DateTime row)
        {
            return row.ToString(DATETIME_SHORT_DATE);
        }

        public static string ToShortDate(this DateTime? row)
        {
            if (!row.HasValue)
            {
                return "";
            }

            return row.Value.ToShortDate();
        }

        public static IHtmlString ToSmartDate(this DateTime? row)
        {
            if (!row.HasValue)
            {
                return new HtmlString("");
            }

            return row.Value.ToSmartDate();
        }

        public static IHtmlString ToSmartDate(this DateTime row)
        {
            if (row < DateTime.UtcNow)
            {
                return new HtmlString(string.Format("<span class='font_red bold'>{0}</span>", row.ToString(DATETIME_SHORT_DATE)));
            }

            return new HtmlString(row.ToShortDate());
        }
    }
}
