using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class SalariedClassification : PaymentClassification{
        private decimal _itsSalary;

        public decimal Salary {
            get { return _itsSalary; }
        }

        public SalariedClassification(decimal itsSalary) {
            _itsSalary = itsSalary;
        }
    }
}
