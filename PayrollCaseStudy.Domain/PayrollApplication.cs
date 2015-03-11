using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class PayrollApplication {
        readonly TransactionSource _source;
        public PayrollApplication(TransactionSource transactionSource) {
            _source= transactionSource;
            
        }

        public void Process() {
            while(true) { 
                var transaction = _source.Next();
                if(transaction == null) {
                    return;
                }
                transaction.Execute();
            }
        }
    }
}
