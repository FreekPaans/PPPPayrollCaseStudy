using PayrollCaseStudy.CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Schedules {
    public interface PaymentSchedule {
        bool IsPayDate(Date date);

        Date GetPayPeriodStartDate(Date payPeriod);
    }
}
