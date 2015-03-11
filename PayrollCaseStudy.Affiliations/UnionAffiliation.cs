using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Affiliations {
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

        public void AddServiceCharge(Date forDate,decimal charge) {
            _charges.Add(new ServiceCharge(forDate,charge));
        }

        public ServiceCharge GetServiceCharge(Date forDate) {
            return _charges.FirstOrDefault(_=>_.Date == forDate);
        }

        public decimal CalculateDeductions(Paycheck paycheck) {
            var fridays = NumberOfFridaysInPayPeriod(paycheck.PayPeriodStartDate,paycheck.PayPeriodEndDate);

            var dues  = fridays * _weeklyDues;

            var serviceCharges = _charges.Where(_=>Date.IsBetween(_.Date, paycheck.PayPeriodStartDate,paycheck.PayPeriodEndDate)).Sum(_=>_.Amount);

            return dues + serviceCharges;
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
