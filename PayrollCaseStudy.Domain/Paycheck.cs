using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class Paycheck {
        private Date _forPayDate;

        public Paycheck(Date forPayDate) {
            _forPayDate = forPayDate;
        }

        public decimal Deductions { get; set; }

        public decimal NetPay { get; set; }

        public decimal GrossPay { get; set; }

        public Date PayDate {
            get {
                return _forPayDate;
            }
        }

        public string GetField(string fieldName) {
            return "Hold";
        }
    }
}
