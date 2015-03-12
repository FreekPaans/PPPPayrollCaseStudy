using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.TransactionImplementation {
    public class ChangeHourlyTransaction : ChangeClassificationTransaction{
        private decimal _hourlyRate;

        public ChangeHourlyTransaction(int empId,decimal hourlyRate) : base(empId){
            _hourlyRate = hourlyRate;
        }

        protected override PaymentClassification GetClassification() {
            return PayrollFactory.Scope.PayrollFactory.MakeHourlyClassification(_hourlyRate);
        }

        protected override PaymentSchedule GetSchedule() {
            return PayrollFactory.Scope.PayrollFactory.MakeWeeklySchedule();
        }
    }
}
