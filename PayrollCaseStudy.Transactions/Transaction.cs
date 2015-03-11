using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Transactions {
    public interface Transaction {
        void Execute();
    }
}
