using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public abstract class PaymentClassification {
        public abstract decimal CalculatePay(Paycheck paycheck);
            
        public bool IsInPayPeriod(Date theDate, Paycheck payCheck) {
            return theDate>=payCheck.PayPeriodStartDate && theDate<=payCheck.PayPeriodEndDate;
        }
    }
}
