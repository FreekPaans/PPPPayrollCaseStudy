using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class ChangeHourlyTransaction : ChangeEmployeeTransaction{
        private decimal _hourlyRate;

        public ChangeHourlyTransaction(int empId,decimal p) : base(empId){
            _hourlyRate = p;
        }

        protected override void Change(Employee e) {
            e.Classification = new HourlyClassification(_hourlyRate);
            e.Schedule = new WeeklySchedule();
        }
    }
}
