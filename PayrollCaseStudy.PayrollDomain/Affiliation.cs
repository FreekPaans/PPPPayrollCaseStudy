using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollDomain {
    public interface Affiliation {
        decimal CalculateDeductions(Paycheck paycheck);
    }
}
