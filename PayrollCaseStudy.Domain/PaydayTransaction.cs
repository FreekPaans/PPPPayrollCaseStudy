using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class PaydayTransaction {
        private Date _forPayDate;
        Dictionary<int,Paycheck> _paychecks = new Dictionary<int,Paycheck>();

        public PaydayTransaction(Date payDate) {
            _forPayDate = payDate;
        }

        public void Execute() {
            var empIds = PayrollDatabase.Instance.GetAllEmployeeIds();

            foreach(var empId in empIds) {
                var employee = PayrollDatabase.Instance.GetEmployee(empId);
                if(employee == null) {
                    continue;
                }
                if(!employee.IsPayDate(_forPayDate)) {
                    continue;
                }
                var paycheck = new Paycheck(_forPayDate);
                _paychecks[empId] = paycheck;
                employee.Payday(paycheck);

            }
        }

        public Paycheck GetPaycheck(int empId) {
            if(!_paychecks.ContainsKey(empId)) {
                return null;
            }
            return _paychecks[empId];
        }
    }
}
