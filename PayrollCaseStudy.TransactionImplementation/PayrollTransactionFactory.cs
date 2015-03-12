using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.TransactionImplementation {
    public class PayrollTransactionFactory : TransactionFactory.Factory{
        public TransactionApplication.Transaction MakeDeleteEmployeeTransaction(int employeeId) {
            return new DeleteEmployeeTransaction(employeeId);
        }

        public TransactionApplication.Transaction MakeTimeCardTransaction(PayrollCaseStudy.CommonTypes.Date date,decimal hour,int empId) {
            return new TimeCardTransaction(date,hour,empId);
        }

        public TransactionApplication.Transaction MakeSalesReceiptTransaction(decimal amount,PayrollCaseStudy.CommonTypes.Date date,int empId) {
            return new SalesReceiptTransaction(amount,date,empId);
        }

        public TransactionApplication.Transaction MakeServiceChargeTransaction(int memberId,PayrollCaseStudy.CommonTypes.Date date,decimal amount) {
            return new ServiceChargeTransaction(memberId,date,amount);
        }

        public TransactionApplication.Transaction MakeChangeMemberTransaction(int empId,int memberId,decimal dues) {
            return new ChangeMemberTransaction(empId,memberId,dues);
        }

        public TransactionApplication.Transaction MakeChangeNameTransaction(int empId,string newName) {
            return new ChangeNameTransaction(empId,newName);
        }

        public TransactionApplication.Transaction MakeChangeAddressTransaction(int empId,string newAddress) {
            return new ChangeAddressTransaction(empId,newAddress);
        }

        public TransactionApplication.Transaction MakeChangeSalariedTransaction(int empId,decimal salary) {
            return new ChangeSalariedTransaction(empId,salary);
        }

        public TransactionApplication.Transaction MakeChangeCommissionedTransaction(int empId,decimal salary,decimal commissionRate) {
            return new ChangeCommissionedTransaction(empId,salary,commissionRate);
        }

        public TransactionApplication.Transaction MakeChangeHourlyTransaction(int empId,decimal hourlyRate) {
            return new ChangeHourlyTransaction(empId,hourlyRate);
        }

        public TransactionApplication.Transaction MakeChangeDirectTransaction(int empId,string bank,string account) {
            return new ChangeDirectTransaction(empId,bank,account);
        }

        public TransactionApplication.Transaction MakeChangeHoldTransaction(int empId) {
            return new ChangeHoldTransaction(empId);
        }

        public TransactionApplication.Transaction MakeChangeMailTransaction(int empId,string address) {
            return new ChangeMailTransaction(empId,address);
        }

        public TransactionApplication.Transaction MakeChangeUnaffiliatedTransaction(int empId) {
            return new ChangeUnaffiliatedTransaction(empId);
        }

        public TransactionApplication.Transaction MakePaydayTransaction(PayrollCaseStudy.CommonTypes.Date date) {
            return new PaydayTransaction(date);
        }

        public TransactionApplication.Transaction MakeAddCommissionedEmployeeTransaction(int empId,string name,string address,decimal salary,decimal commissionrate) {
            return new AddCommissionedEmployee(empId,name,address,salary,commissionrate);
        }

        public TransactionApplication.Transaction MakeAddSalariedEmployeeTransaction(int empId,string name,string address,decimal salary) {
            return new AddSalariedEmployee(empId,name,address,salary);
        }

        public TransactionApplication.Transaction MakeAddHourlyEmployeeTransaction(int empId,string name,string address,decimal hourlyRate) {
            return new AddHourlyEmployee(empId,name,address,hourlyRate);
        }
    }
}
