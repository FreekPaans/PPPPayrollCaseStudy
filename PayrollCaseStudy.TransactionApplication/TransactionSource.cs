using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.TransactionApplication {
    public interface TransactionSource {
        Transaction Next();
    }
    
}
