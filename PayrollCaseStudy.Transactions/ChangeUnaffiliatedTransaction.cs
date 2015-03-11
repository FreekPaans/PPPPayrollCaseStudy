using PayrollCaseStudy.Affiliations;
using PayrollCaseStudy.PayrollDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Transactions {
    public class ChangeUnaffiliatedTransaction : ChangeAffiliationTransaction{
        public ChangeUnaffiliatedTransaction(int empId) : base(empId){
        }
        protected override Affiliation GetAffiliation() {
            return new NoAffiliation();
        }

        protected override void RecordMembership(Employee e) {
            var affiliation = e.Affiliation as UnionAffiliation;

            if(affiliation==null) {
                return;
            }
            Database.Instance.RemoveUnionMember(affiliation.MemberId);
        }
    }
}
