using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public abstract class ChangeMethodTransaction : ChangeEmployeeTransaction{
        
        public ChangeMethodTransaction(int empId) :base(empId){
        }
        protected override void Change(Employee e) {
            e.Method = GetMethod();
        }

        protected abstract PaymentMethod GetMethod();
    }
}
