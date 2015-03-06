using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class Date {
        private DateTime _date;
        
        public Date(int month,int day,int year) {
            _date = new DateTime(year,month,day);            
        }

        Date(DateTime newDate) {
            _date = newDate;
        }

        internal Date AddDays(int days) {
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
    }
}
