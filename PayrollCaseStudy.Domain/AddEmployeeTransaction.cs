using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public abstract class AddEmployeeTransaction: Transaction {
        private string _address;
        private string _name;
        private int _employeeId;

        public AddEmployeeTransaction(int employeeId, string name, string address) {
            _employeeId = employeeId;
            _name = name;
            _address = address;
        }

        public virtual void Execute() {
            PaymentClassification classification = GetClassification();
            PaymentSchedule paymentSchedule = GetSchedule();
            PaymentMethod method = new HoldMethod();
            var employee=  new Employee(_employeeId,_name,_address);
            employee.Classification = classification;
            employee.Schedule = paymentSchedule;
            employee.Method = method;
            PayrollDatabase.Instance.AddEmployee(_employeeId, employee);
        }

        protected abstract PaymentSchedule GetSchedule(); 
        protected abstract PaymentClassification GetClassification();
    }
}
