using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.PayrollDatabase;
using PayrollCaseStudy.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Transactions {
    public abstract class ChangeClassificationTransaction : ChangeEmployeeTransaction{
        public ChangeClassificationTransaction(int employeeId) : base(employeeId) {
        }
        
        protected override void Change(Employee e) {
            e.Classification = GetClassification();
            e.Schedule = GetSchedule();
        }

        protected abstract PaymentClassification GetClassification();
        protected abstract PaymentSchedule GetSchedule();

    }
}
