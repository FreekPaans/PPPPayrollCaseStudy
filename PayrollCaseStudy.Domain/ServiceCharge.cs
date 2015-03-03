using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class ServiceCharge {
        private int _forDate;

        public int Date {
            get { return _forDate; }
        }
        private decimal _charge;

        public decimal Amount {
            get { return _charge; }
        }

        public ServiceCharge(int forDate,decimal charge) {
            _forDate = forDate;
            _charge = charge;
        }
    }
}
