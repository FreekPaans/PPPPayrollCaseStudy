using PayrollCaseStudy.PayrollDomain;
using PayrollCaseStudy.TransactionApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.TransactionImplementation {
    public class DeleteEmployeeTransaction : Transaction{
        private int _employeeId;

        public DeleteEmployeeTransaction(int empId) {
            this._employeeId = empId;
        }

        public void Execute() {
            PayrollDatabase.Scope.DatabaseInstance.DeleteEmployee(_employeeId);
        }
    }
}
