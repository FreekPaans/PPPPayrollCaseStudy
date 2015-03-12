using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollDomain {
    public interface PaymentMethod {
        void Pay(Paycheck paycheck);
    }
}
