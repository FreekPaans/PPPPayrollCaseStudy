using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.TransactionFactory {
    public interface Factory {
        TransactionApplication.Transaction MakeDeleteEmployeeTransaction(int employeeId);

        TransactionApplication.Transaction MakeTimeCardTransaction(CommonTypes.Date date,decimal hour,int empId);

        TransactionApplication.Transaction MakeSalesReceiptTransaction(decimal amount,CommonTypes.Date date,int empId);

        TransactionApplication.Transaction MakeServiceChargeTransaction(int memberId,CommonTypes.Date date,decimal amount);

        TransactionApplication.Transaction MakeChangeMemberTransaction(int empId,int memberId,decimal dues);

        TransactionApplication.Transaction MakeChangeNameTransaction(int empId,string newName);

        TransactionApplication.Transaction MakeChangeAddressTransaction(int empId,string newAddress);

        TransactionApplication.Transaction MakeChangeSalariedTransaction(int empId,decimal salary);

        TransactionApplication.Transaction MakeChangeCommissionedTransaction(int empId,decimal salary,decimal commissionRate);

        TransactionApplication.Transaction MakeChangeHourlyTransaction(int empId,decimal hourlyRate);

        TransactionApplication.Transaction MakeChangeDirectTransaction(int empId,string bank,string account);

        TransactionApplication.Transaction MakeChangeHoldTransaction(int empId);

        TransactionApplication.Transaction MakeChangeMailTransaction(int empId,string address);

        TransactionApplication.Transaction MakeChangeUnaffiliatedTransaction(int empId);

        TransactionApplication.Transaction MakePaydayTransaction(CommonTypes.Date date);

        TransactionApplication.Transaction MakeAddCommissionedEmployeeTransaction(int empId,string name,string address,decimal salary,decimal commissionrate);

        TransactionApplication.Transaction MakeAddSalariedEmployeeTransaction(int empId,string name,string address,decimal salary);

        TransactionApplication.Transaction MakeAddHourlyEmployeeTransaction(int empId,string name,string address,decimal hourlyRate);

        
    }
}
