using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class ChangeMemberTransaction : ChangeAffiliationTransaction{
        private int _empId;
        private int _memberId;
        private decimal _weeklyDues;

        public ChangeMemberTransaction(int empId,int memberId,decimal weeklyDues)  :base(empId){
            _empId = empId;
            _memberId = memberId;
            _weeklyDues = weeklyDues;
        }


        protected override Affiliation GetAffiliation() {
            return new UnionAffiliation(_memberId,_weeklyDues);
        }

        protected override void RecordMembership(Employee e) {
            PayrollDatabase.Instance.AddUnionMember(_memberId,e);
        }
    }
}
