using PayrollCaseStudy.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Transactions {
    public class ChangeHoldTransaction : ChangeMethodTransaction{
        
        public ChangeHoldTransaction(int empId) :base(empId){
        }
        protected override PaymentMethod GetMethod() {
            return new HoldMethod();
        }
    }
}
