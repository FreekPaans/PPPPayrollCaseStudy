using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Classifications {
    public abstract class PaymentClassification {
        public abstract decimal CalculatePay(Paycheck paycheck);
            
        public bool IsInPayPeriod(Date theDate, Paycheck payCheck) {
            return Date.IsBetween(theDate,payCheck.PayPeriodStartDate,payCheck.PayPeriodEndDate);
        }
    }
}
