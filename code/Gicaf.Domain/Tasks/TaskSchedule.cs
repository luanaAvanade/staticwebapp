using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Tasks
{
    [Flags]
    public enum DaysOfWeek
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64
    }

    public enum DateFrequencyType
    {
        Daily,
        Weekly,
        Monthly,
    }

    public class TaskSchedule
    {
        public DateFrequency DateFrequency { get; set; }
        public TimeFrequency TimeFrequency { get; set; }
        public DateTime LastExecution { get; set; }

        public bool Ready
        {
            get
            {
                var dateTime = DateTime.Now;
                var timeofDay = dateTime.TimeOfDay;

                var ready = dateTime >= TimeFrequency.StartDate && (TimeFrequency.EndingDate == null || dateTime <= TimeFrequency.EndingDate);

                if(ready && TimeFrequency.Type == TimeFrequencyType.Once)
                {
                    ready = TimeFrequency.OccursOnceAt >= timeofDay;
                }

                if (ready && TimeFrequency.Type == TimeFrequencyType.Periodically)
                {
                    ready = timeofDay >= TimeFrequency.StartingAt && timeofDay <= TimeFrequency.EndingAt;
                    ready = ready && dateTime >= LastExecution.AddSeconds(TimeFrequency.OccursEverySeconds);
                }

                if (ready && DateFrequency is DailyFrequency dailyFrequency)
                {
                    ready = dateTime >= LastExecution.AddDays(dailyFrequency.Days);
                }

                if (ready && DateFrequency is WeeklyFrequency weeklyFrequency)
                {
                    ready = dateTime >= LastExecution.AddDays(7 * weeklyFrequency.Weeks);
                    ready = ready && weeklyFrequency.DaysOfWeek.HasFlag(Enum.Parse<DaysOfWeek>(dateTime.DayOfWeek.ToString()));
                }

                if (ready && DateFrequency is MonthlyFrequency monthlyFrequency)
                {
                    ready = dateTime >= LastExecution.AddMonths(monthlyFrequency.Months);
                    
                    if(ready)
                    {
                        if (monthlyFrequency.MonthlyFrequencyType == MonthlyFrequencyType.Day)
                        {
                            ready = dateTime.Day == monthlyFrequency.Day;
                        }

                        if (monthlyFrequency.MonthlyFrequencyType == MonthlyFrequencyType.DayOfWeek)
                        {
                            ready = dateTime.Day == dateTime.NthOf(((int)monthlyFrequency.Ocurrence+1),monthlyFrequency.DayOfWeek).Day;
                        }
                    }
                }
                return ready;
            }
        }

    }

    public abstract class DateFrequency
    {
        public DateFrequencyType Type { get; set; }
    }

    public class DailyFrequency: DateFrequency
    {
        public int Days { get; set; }
    }

    public class WeeklyFrequency : DateFrequency
    {
        public int Weeks { get; set; }
        public DaysOfWeek DaysOfWeek { get; set; }
    }

    public class MonthlyFrequency : DateFrequency
    {
        public int Day { get; set; }
        public int Months { get; set; }

        public Ocurrence Ocurrence { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public MonthlyFrequencyType MonthlyFrequencyType { get; set; }
    }

    public class TimeFrequency
    {
        public TimeFrequencyType Type { get; set; }

        public TimeSpan OccursOnceAt { get; set; }
        public int OccursEverySeconds { get; set; }

        public TimeSpan StartingAt { get; set; }
        public TimeSpan EndingAt { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndingDate { get; set; }
    }

    public class TaskScheduleBuilder
    {
        static TaskSchedule _taskSchedule;
        
        public TaskScheduleBuilder NewRecurring(DateTime startDate, DateTime? endingDate = null)
        {
            _taskSchedule = new TaskSchedule { TimeFrequency = new TimeFrequency() };
            _taskSchedule.TimeFrequency.StartDate = startDate;
            _taskSchedule.TimeFrequency.EndingDate = endingDate;
            return this;
        }

        public TaskScheduleBuilder OccursOnceAt(TimeSpan date)
        {
            _taskSchedule.TimeFrequency.Type = TimeFrequencyType.Once;
            _taskSchedule.TimeFrequency.OccursOnceAt = date;
            return this;
        }

        public TaskScheduleBuilder OccursEverySeconds(int seconds, TimeSpan startingAt, TimeSpan endingAt)
        {
            _taskSchedule.TimeFrequency.Type = TimeFrequencyType.Periodically;
            _taskSchedule.TimeFrequency.OccursEverySeconds = seconds;
            _taskSchedule.TimeFrequency.StartingAt = startingAt;
            _taskSchedule.TimeFrequency.EndingAt = endingAt;
            return this;
        }

        public TaskScheduleBuilder OccursEveryMinutes(int minutes, TimeSpan startingAt, TimeSpan endingAt)
        {
            return OccursEverySeconds(minutes * 60, startingAt, endingAt);
        }

        public TaskScheduleBuilder OccursEveryHours(int hours, TimeSpan startingAt, TimeSpan endingAt)
        {
            return OccursEverySeconds(hours * 3600, startingAt, endingAt);
        }

        public void OnEveryDay(int days)
        {
            var dailyFrequency = new DailyFrequency();
            dailyFrequency.Days = days;
            _taskSchedule.DateFrequency = dailyFrequency;
        }

        public void OnEveryWeek(int weeks, DaysOfWeek daysOfWeek)
        {
            var weeklyFrequency = new WeeklyFrequency
            {
                Weeks = weeks,
                DaysOfWeek = daysOfWeek
            };
            _taskSchedule.DateFrequency = weeklyFrequency;
        }

        public void OnEveryMonthAtDay(int months, int day)
        {
            var monthlyFrequency = new MonthlyFrequency
            {
                Months = months,
                Day = day,
                MonthlyFrequencyType = MonthlyFrequencyType.Day
            };
            _taskSchedule.DateFrequency = monthlyFrequency;
        }

        public void OnEveryMonthAtDayOfWeek(int months, DayOfWeek dayOfWeek, Ocurrence the)
        {
            var monthlyFrequency = new MonthlyFrequency
            {
                Months = months,
                Ocurrence = the,
                DayOfWeek = dayOfWeek,
                MonthlyFrequencyType = MonthlyFrequencyType.DayOfWeek
            };

            _taskSchedule.DateFrequency = monthlyFrequency;
        }
    }

    public enum Ocurrence
    {
        First,
        Second,
        Third,
        Fourth,
        Last
    }

    public enum TimeFrequencyType
    {
        Once,
        Periodically
    }

    public enum MonthlyFrequencyType
    {
        Day,
        DayOfWeek
    }

    public static class DatetimeExt
    {
        public static DateTime NthOf(this DateTime CurDate, int Occurrence, DayOfWeek Day)
        {
            var fday = new DateTime(CurDate.Year, CurDate.Month, 1);

            var fOc = fday.DayOfWeek == Day ? fday : fday.AddDays(Day - fday.DayOfWeek);
            // CurDate = 2011.10.1 Occurance = 1, Day = Friday >> 2011.09.30 FIX. 
            if (fOc.Month < CurDate.Month) Occurrence = Occurrence + 1;
            return fOc.AddDays(7 * (Occurrence - 1));
        }
    }
    
}
