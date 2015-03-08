using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class NoAffiliation : Affiliation{
        public decimal CalculateDeductions(Paycheck paycheck) {
            return 0M;
        }
    }
}
