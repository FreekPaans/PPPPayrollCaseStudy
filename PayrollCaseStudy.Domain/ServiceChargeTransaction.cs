﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class ServiceChargeTransaction : Transaction{
        private decimal _charge;
        private Date _forDate;
        private int _memberId;
        
        public ServiceChargeTransaction(int memberId,Date forDate,decimal charge) {
            _memberId = memberId;
            _forDate = forDate;
            _charge = charge;
        }
        public void Execute() {
            Employee e = PayrollDatabase.Instance.GetUnionMember(_memberId);

            var unionAffiliation = e.Affiliation as UnionAffiliation;

            if(unionAffiliation!=null) {
                unionAffiliation.AddServiceCharge(_forDate,_charge);
            }
        }
    }
}
