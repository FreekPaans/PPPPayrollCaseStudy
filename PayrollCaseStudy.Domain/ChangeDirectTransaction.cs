using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class ChangeDirectTransaction : ChangeMethodTransaction{
        public ChangeDirectTransaction(int empId)  :base(empId){
        }

        protected override PaymentMethod GetMethod() {
            return new DirectMethod();
        }
    }
}
