using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public abstract class ChangeEmployeeTransaction {
        private int _empId;

        protected abstract void Change(Employee e);

        public ChangeEmployeeTransaction(int empId) {
            _empId = empId;
        }

        public void Execute() {
            var employee = PayrollDatabase.Instance.GetEmployee(_empId);
            if(employee!=null) {
                Change(employee);
            }
        }
    }
}
