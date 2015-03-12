using PayrollCaseStudy.Employees;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.EmployeeDataTransactions {
    public class ChangeAddressTransaction : ChangeEmployeeTransaction{
        private string _address;

        public ChangeAddressTransaction(int empId,string address) : base(empId){
            _address = address;
        }
        protected override void Change(Employee e) {
            e.Address = _address;
        }
    }
}
