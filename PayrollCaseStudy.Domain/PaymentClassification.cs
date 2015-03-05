using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public interface PaymentClassification {
        decimal CalculatePay(Paycheck paycheck);
    }
}
