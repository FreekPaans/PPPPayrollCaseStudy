using PayrollCaseStudy.Affiliations;
using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.Methods;
using PayrollCaseStudy.PayrollDomain;
using PayrollCaseStudy.TransactionApplication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.PayrollApplication {
    public class TextParserTransactionSource : TransactionSource{
        readonly TextReader _reader;
        readonly TransactionFactory.Factory _transactionFactory;

        public TextParserTransactionSource(TextReader reader, TransactionFactory.Factory factory) {
            _reader = reader;
            _transactionFactory = factory;
        }
        public Transaction Next() {
            while(true) {
                var nextLine = _reader.ReadLine();
                if(nextLine == null) {
                    return null;
                }
                if(string.IsNullOrWhiteSpace(nextLine)) {
                    continue;
                }

                return ParseLine(nextLine);
            }
        }

        private Transaction ParseLine(string line) {
            var wordReader = WordReader.FromLine(line);

            if(!wordReader.HasNext()) {
                throw new InvalidOperationException(string.Format("Invalid line: {0}", line));
            }

            switch(wordReader.Next()) {
                case "AddEmp":
                    return AddEmp(line,wordReader);
                case "DelEmp":
                    return DelEmp(line,wordReader);
                case "TimeCard":
                    return TimeCard(line,wordReader);
                case "SalesReceipt":
                    return SalesReceipt(line,wordReader);
                case "ServiceCharge":
                    return ServiceCharge(line,wordReader);
                case "ChgEmp":
                    return ChangeEmployee(line,wordReader);
                case "PayDate":
                    return PayDate(line,wordReader);
            }
            throw new InvalidOperationException(string.Format("Cannot parse {0}", line));
        }

        private Transaction PayDate(string line,WordReader wordReader) {
            return _transactionFactory.MakePaydayTransaction(wordReader.NextAsDate());
        }

        private Transaction ChangeEmployee(string line,WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            switch(wordReader.Next()) {
                case "Member":
                    return ChangeEmployeeMember(empId,wordReader);
                case "Name":
                    return ChangeEmployeeName(empId,wordReader);
                case "Address":
                    return ChangeEmployeeAddress(empId,wordReader);
                case "Hourly":
                    return ChangeHourly(empId,wordReader);
                case "Salaried":
                    return ChangeSalaried(empId,wordReader);
                case "Commissioned":
                    return ChangeCommissioned(empId,wordReader);
                case "Direct":
                    return ChangeDirect(empId,wordReader);
                case "Hold":
                    return ChangeHold(empId,wordReader);
                case "Mail":
                    return ChangeMail(empId,wordReader);
                case "NoMember":
                    return ChangeNoMember(empId,wordReader);
            }
            
            throw new InvalidOperationException(string.Format("Couldn't parse {0}", line));
        }

        private Transaction ChangeNoMember(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeUnaffiliatedTransaction(empId);
        }

        private Transaction ChangeMail(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeMailTransaction(empId,wordReader.NextQuoted());
        }

        private Transaction ChangeHold(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeHoldTransaction(empId);
        }

        private Transaction ChangeDirect(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeDirectTransaction(empId,wordReader.Next(),wordReader.Next());
        }

        private Transaction ChangeCommissioned(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeCommissionedTransaction(empId,wordReader.NextAsDecimal(),wordReader.NextAsDecimal());
        }

        private Transaction ChangeSalaried(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeSalariedTransaction(empId,wordReader.NextAsDecimal());
        }

        private Transaction ChangeHourly(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeHourlyTransaction(empId,wordReader.NextAsDecimal());
        }

        private Transaction ChangeEmployeeAddress(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeAddressTransaction(empId,wordReader.NextQuoted());
        }

        private Transaction ChangeEmployeeName(int empId,WordReader wordReader) {
            return _transactionFactory.MakeChangeNameTransaction(empId,wordReader.NextQuoted());
        }

        private Transaction ChangeEmployeeMember(int empId,WordReader wordReader) {
            var memberId = wordReader.NextAsInt();
            wordReader.Skip();
            var dues = wordReader.NextAsDecimal();
            return _transactionFactory.MakeChangeMemberTransaction(empId,memberId,dues);
        }

        private Transaction ServiceCharge(string line,WordReader wordReader) {
            var memberId = wordReader.NextAsInt();
            var amount = wordReader.NextAsDecimal();
            return _transactionFactory.MakeServiceChargeTransaction(memberId,Date.Today,amount);
        }

        private Transaction SalesReceipt(string line,WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            var date = wordReader.NextAsDate();
            var amount = wordReader.NextAsDecimal();
            return _transactionFactory.MakeSalesReceiptTransaction(amount,date,empId);
        }

        private Transaction TimeCard(string line,WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            var date = wordReader.NextAsDate();
            var hour = wordReader.NextAsDecimal();
            return _transactionFactory.MakeTimeCardTransaction(date,hour,empId);
        }

        private Transaction DelEmp(string line,WordReader wordReader) {
            return _transactionFactory.MakeDeleteEmployeeTransaction(wordReader.NextAsInt());
        }


        private Transaction AddEmp(string line, WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            var name = wordReader.NextQuoted();
            var address = wordReader.NextQuoted();
            switch(wordReader.Next()) {
                case "H":
                    return AddHourlyEmployee(empId,name,address,wordReader);
                case "S":
                    return AddSalariedEmployee(empId,name,address,wordReader);
                case "C":
                    return AddCommissionedEmployee(empId,name,address,wordReader);
            }
            throw new InvalidOperationException(string.Format("Cannot parse {0}",line));
        }

        private Transaction AddCommissionedEmployee(int empId,string name,string address,WordReader wordReader) {
            return _transactionFactory.MakeAddCommissionedEmployeeTransaction(empId,name,address, wordReader.NextAsDecimal(), wordReader.NextAsDecimal());
        }

        private Transaction AddSalariedEmployee(int empId,string name,string address,WordReader wordReader) {
            return _transactionFactory.MakeAddSalariedEmployeeTransaction(empId,name,address,wordReader.NextAsDecimal());
        }

        private Transaction AddHourlyEmployee(int empId,string name,string address,WordReader wordReader) {
            return _transactionFactory.MakeAddHourlyEmployeeTransaction(empId,name,address,wordReader.NextAsDecimal());
        }
    }
}
