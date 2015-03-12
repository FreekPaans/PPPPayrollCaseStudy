using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollDomain {
    public class NoAffiliation : Affiliation{
        public decimal CalculateDeductions(Paycheck paycheck) {
            return 0M;
        }


        public int? GetMemberId() {
            return null;
        }

        public void AddServiceCharge(CommonTypes.Date forDate,decimal charge) {
            
        }
    }
}
