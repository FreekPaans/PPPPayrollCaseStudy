using PayrollCaseStudy.PayrollDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Transactions {
    public abstract class ChangeEmployeeTransaction : Transaction{
        private int _empId;

        protected abstract void Change(Employee e);

        public ChangeEmployeeTransaction(int empId) {
            _empId = empId;
        }

        public void Execute() {
            var employee = Database.Instance.GetEmployee(_empId);
            if(employee!=null) {
                Change(employee);
            }
        }
    }
}
