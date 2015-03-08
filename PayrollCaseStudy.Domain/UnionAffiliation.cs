using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class UnionAffiliation  : Affiliation{
        private decimal _weeklyDues;

        public decimal Dues {
            get { return _weeklyDues; }
        }
        private List<ServiceCharge> _charges = new List<ServiceCharge>();
        private int _memberId;

        public int MemberId {
            get { return _memberId; }
        }

        public UnionAffiliation(int memberId, decimal weeklyDues) {
            _weeklyDues = weeklyDues;
            _memberId = memberId;
        }

        internal void AddServiceCharge(int forDate,decimal charge) {
            _charges.Add(new ServiceCharge(forDate,charge));
        }

        public ServiceCharge GetServiceCharge(int forDate) {
            return _charges.FirstOrDefault(_=>_.Date == forDate);
        }

        public decimal CalculateDeductions(Paycheck paycheck) {
            var fridays = NumberOfFridaysInPayPeriod(paycheck.PayPeriodStartDate,paycheck.PayPeriodEndDate);

            return fridays * _weeklyDues;
        }

        private int NumberOfFridaysInPayPeriod(Date startDate,Date endDate) {
            var res = 0;
            for(var i = startDate;i<=endDate;i = i.AddDays(1)) {
                if(i.DayOfWeek == DayOfWeek.Friday) {
                    res++;
                }
            }

            return res;
        }
    }
}
