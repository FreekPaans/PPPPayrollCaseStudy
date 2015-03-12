using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollApplication {
    public interface TransactionSource {
        Transaction Next();
    }
    
}
