using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class TimeCard {
        private int _forDate;

        public int Date {
            get { return _forDate; }
        }
        private decimal _hours;

        public decimal Hours {
            get { return _hours; }
        }

        public TimeCard(int forDate,decimal hours) {
            _forDate = forDate;
            _hours = hours;
        }
        
    }
}
