using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.TransactionImplementation {
    public class AddCommissionedEmployee : AddEmployeeTransaction{
        private decimal _salary;
        private decimal _commissionRate;
        
        public AddCommissionedEmployee(int empId,string name,string address,decimal salary,decimal commissionRate)  : base(empId, name,address) {
            _salary = salary;
            _commissionRate = commissionRate;
        }

        protected override PaymentSchedule GetSchedule() {
            return new BiweeklySchedule();
        }

        protected override PaymentClassification GetClassification() {
            return new CommissionedClassification(_salary,_commissionRate);
        }
    }
}
