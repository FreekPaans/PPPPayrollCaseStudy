using PayrollCaseStudy.CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollImplementation {
    public class TimeCard {
        private Date _forDate;

        public Date Date {
            get { return _forDate; }
        }
        private decimal _hours;

        public decimal Hours {
            get { return _hours; }
        }

        public TimeCard(Date forDate,decimal hours) {
            _forDate = forDate;
            _hours = hours;
        }
        
    }
}
