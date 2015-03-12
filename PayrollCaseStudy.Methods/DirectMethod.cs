using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Methods {
    public class DirectMethod :PaymentMethod{
        private string _account;

        public string Account {
            get { return _account; }
        }
        private string _bank;

        public string Bank {
            get { return _bank; }
        }

        public DirectMethod(string account,string bank) {
            _account = account;
            _bank = bank;
        }

        public void Pay(Paycheck paycheck) {
            
        }
    }
}
