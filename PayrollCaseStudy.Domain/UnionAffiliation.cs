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
            throw new NotImplementedException();
        }



        
    }
}
