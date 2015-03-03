using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class UnionAffiliation  : Affiliation{
        private decimal _weeklyDues;
        private List<ServiceCharge> _charges = new List<ServiceCharge>();

        public UnionAffiliation(decimal weeklyDues) {
            _weeklyDues = weeklyDues;
        }

        internal void AddServiceCharge(int forDate,decimal charge) {
            _charges.Add(new ServiceCharge(forDate,charge));
        }

        public ServiceCharge GetServiceCharge(int forDate) {
            return _charges.FirstOrDefault(_=>_.Date == forDate);
        }
    }
}
