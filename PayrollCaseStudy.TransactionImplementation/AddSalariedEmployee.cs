using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.TransactionImplementation {
    public class AddSalariedEmployee : AddEmployeeTransaction{
        private decimal _itsSalary;

        public AddSalariedEmployee(int employeeId, string name, string address, decimal salary) : base(employeeId,name,address) {
            _itsSalary = salary;
        }

        protected override PaymentSchedule GetSchedule() {
            return new MonthlySchedule();
        }

        protected override PaymentClassification GetClassification() {
            return new SalariedClassification(_itsSalary);
        }
    }
}
