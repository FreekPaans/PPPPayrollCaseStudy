using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.CommonTypes {
    public class Date {
        private DateTime _date;
        
        public Date(int month,int day,int year) {
            _date = new DateTime(year,month,day);            
        }

        Date(DateTime newDate) {
            _date = newDate;
        }

        public Date AddDays(int days) {
            return new Date(_date.AddDays(days));
        }

        public int Month {
            get {
                return _date.Month;
            }
        }

        public override string ToString() {
            return _date.ToShortDateString();
        }

        public DayOfWeek DayOfWeek {
            get {
                return _date.DayOfWeek;
            }
        }


        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }

            if(!(obj is Date)) {
                return false;
            }

            return _date== ((Date)obj)._date;
        }

        public override int GetHashCode() {
            return _date.GetHashCode();
        }

        public static bool operator ==(Date d1,Date d2) {
            return d1.Equals(d2);
        }

        public static bool operator !=(Date d1,Date d2) {
            return !d1.Equals(d2);
        }

        public static bool operator >=(Date d1, Date d2) {
            return d1._date >= d2._date;
        }

        public static bool operator <=(Date d1, Date d2) {
            return d1._date <= d2._date;
        }

        public static bool operator <(Date d1, Date d2) {
            return d1._date < d2._date;
        }

        public static bool operator >(Date d1, Date d2) {
            return d1._date > d2._date;
        }

        public int DaySince(Date date) {
            return (int)(_date - date._date).TotalDays;
        }

        public Date AddMonth(int months) {
            return new Date(_date.AddMonths(months));
        }

        public static bool IsBetween(Date theDate,Date startDate,Date endDate) {
            return theDate>=startDate && theDate<=endDate;
        }

        public static Date Parse(string p) {
            return new Date(DateTime.ParseExact(p,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.AssumeUniversal));
        }

        public static Date Today {
            get {
                return new Date(DateTime.Today);
            }
        }
    }
}
