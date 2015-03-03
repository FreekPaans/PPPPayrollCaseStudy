using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class SalesReceipt {
        private decimal _amount;

        public decimal Amount {
            get { return _amount; }
        }
        private int _forDate;

        public int Date {
            get { return _forDate; }
        }

        public SalesReceipt(decimal amount,int forDate) {
            _amount = amount;
            _forDate = forDate;
        }

        
    }
}
