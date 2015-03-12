using PayrollCaseStudy.Methods;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.TransactionImplementation {
    public class ChangeDirectTransaction : ChangeMethodTransaction{
        private string _account;
        private string _bank;
        
        public ChangeDirectTransaction(int empId,string bank,string account) :base(empId) {
            _bank = bank;
            _account =  account;
        }

        protected override PaymentMethod GetMethod() {
            return new DirectMethod(_account,_bank);
        }
    }
}
