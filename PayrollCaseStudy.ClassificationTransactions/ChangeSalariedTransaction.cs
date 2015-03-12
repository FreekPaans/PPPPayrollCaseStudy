using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.ClassificationTransactions {
    public class ChangeSalariedTransaction : ChangeClassificationTransaction{
        private decimal _monthlySalary;
        
        public ChangeSalariedTransaction(int empId,decimal monthlySalary) : base(empId) {
            _monthlySalary = monthlySalary;
        }

        protected override PaymentClassification GetClassification() {
            return new SalariedClassification(_monthlySalary);
        }

        protected override PaymentSchedule GetSchedule() {
            return new MonthlySchedule();
        }
    }
}
