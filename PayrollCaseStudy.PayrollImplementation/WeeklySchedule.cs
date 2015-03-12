using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.PayrollImplementation {
    public class WeeklySchedule : PaymentSchedule{
        public bool IsPayDate(Date date) {
            if(date.DayOfWeek== DayOfWeek.Friday) {
                return true;
            }

            return false;
        }


        public Date GetPayPeriodStartDate(Date payPeriod) {
            return payPeriod.AddDays(-6);
        }
    }
}
