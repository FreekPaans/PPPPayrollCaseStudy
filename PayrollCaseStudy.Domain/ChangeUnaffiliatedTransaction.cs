using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
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
            PayrollDatabase.Instance.RemoveUnionMember(affiliation.MemberId);
        }
    }
}
