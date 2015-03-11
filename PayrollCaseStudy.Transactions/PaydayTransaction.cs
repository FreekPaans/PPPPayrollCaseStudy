using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.Pay;
using PayrollCaseStudy.PayrollDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Transactions {
    public class PaydayTransaction : Transaction{
        private Date _forPayDate;
        Dictionary<int,Paycheck> _paychecks = new Dictionary<int,Paycheck>();

        public PaydayTransaction(Date payDate) {
            _forPayDate = payDate;
        }

        public void Execute() {
            var empIds = Database.Instance.GetAllEmployeeIds();

            foreach(var empId in empIds) {
                var employee = Database.Instance.GetEmployee(empId);
                if(employee == null) {
                    continue;
                }
                if(!employee.IsPayDate(_forPayDate)) {
                    continue;
                }
                var paycheck = new Paycheck(employee.GetPayPeriodStartDate(_forPayDate), _forPayDate);
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
