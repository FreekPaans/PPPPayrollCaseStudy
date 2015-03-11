using PayrollCaseStudy.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Affiliations {
    public interface Affiliation {
        decimal CalculateDeductions(Paycheck paycheck);
    }
}
