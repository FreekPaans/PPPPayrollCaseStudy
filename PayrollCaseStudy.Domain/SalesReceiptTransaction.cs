using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class SalesReceiptTransaction : Transaction{
        private decimal _amount;
        private Date _forDate;
        private int _employeeId;
        
        public SalesReceiptTransaction(decimal amount,Date forDate, int employeeId) {
            _forDate = forDate;    
            _amount = amount;
            _employeeId = employeeId;
        }
        public void Execute() {
            var employee = PayrollDatabase.Instance.GetEmployee(_employeeId);

            if(employee == null) {
                throw new Exception("Employee not found");
            }

            var commissioned = employee.GetClassification() as CommissionedClassification;
            if(commissioned == null) {
                throw new Exception("Employee not commissioned");
            }

            commissioned.AddSalesReceipt(new SalesReceipt(_amount,_forDate));
        }
    }
}
