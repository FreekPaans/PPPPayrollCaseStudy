﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class ChangeHourlyTransaction : ChangeClassificationTransaction{
        private decimal _hourlyRate;

        public ChangeHourlyTransaction(int empId,decimal p) : base(empId){
            _hourlyRate = p;
        }

        protected override PaymentClassification GetClassification() {
            return new HourlyClassification(_hourlyRate);
        }

        protected override PaymentSchedule GetSchedule() {
            return new WeeklySchedule();
        }
    }
}
