﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public abstract class ChangeAffiliationTransaction : ChangeEmployeeTransaction{
        
        public ChangeAffiliationTransaction(int empId) : base(empId){

        }

        protected abstract Affiliation GetAffiliation();
        protected abstract void RecordMembership(Employee e);


        protected override void Change(Employee e) {
            e.Affiliation = GetAffiliation();
            RecordMembership(e);
        }
    }
}
