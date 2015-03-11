using PayrollCaseStudy.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Transactions {
    public class ChangeMailTransaction : ChangeMethodTransaction{
        private string _address;

        public ChangeMailTransaction(int empId,string address) :base(empId){
            _address = address;
        }
        protected override PaymentMethod GetMethod() {
            return new MailMethod(_address);
        }
    }
}
