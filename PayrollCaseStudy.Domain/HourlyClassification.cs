using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class HourlyClassification : PaymentClassification{
        private decimal _hourlyRate;

        public decimal HourlyRate {
            get { return _hourlyRate; }
        }

        public HourlyClassification(decimal hourlyRate) {
            _hourlyRate = hourlyRate;
        }
        
    }
}
