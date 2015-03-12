using PayrollCaseStudy.PayrollDatabase;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.TransactionImplementation {
    public class ChangeUnaffiliatedTransaction : ChangeAffiliationTransaction{
        public ChangeUnaffiliatedTransaction(int empId) : base(empId){
        }
        protected override Affiliation GetAffiliation() {
            return PayrollFactory.Scope.PayrollFactory.MakeNoAffiliation();
        }

        protected override void RecordMembership(Employee e) {
            var memberId = e.Affiliation.GetMemberId();

            if(memberId==null) {
                return;
            }
            PayrollDatabase.Scope.DatabaseInstance.RemoveUnionMember(memberId.Value);
        }
    }
}
