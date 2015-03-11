using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class TextParserTransactionSource : TransactionSource{
        readonly TextReader _reader;  

        public TextParserTransactionSource(TextReader reader) {
            _reader = reader;
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
            }
            throw new InvalidOperationException(string.Format("Cannot parse {0}", line));
        }

        private Transaction ChangeEmployee(string line,WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            switch(wordReader.Next()) {
                case "Member":
                    return ChangeEmployeeMember(empId,wordReader);
            }
            
            throw new InvalidOperationException(string.Format("Couldn't parse {0}", line));
        }

        private Transaction ChangeEmployeeMember(int empId,WordReader wordReader) {
            var memberId = wordReader.NextAsInt();
            wordReader.Skip();
            var dues = wordReader.NextAsDecimal();
            return new ChangeMemberTransaction(empId,memberId,dues);
        }

        private Transaction ServiceCharge(string line,WordReader wordReader) {
            var memberId = wordReader.NextAsInt();
            var amount = wordReader.NextAsDecimal();
            return new ServiceChargeTransaction(memberId,Date.Today,amount);
        }

        private Transaction SalesReceipt(string line,WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            var date = wordReader.NextAsDate();
            var amount = wordReader.NextAsDecimal();
            return new SalesReceiptTransaction(amount,date,empId);
        }

        private Transaction TimeCard(string line,WordReader wordReader) {
            var empId = wordReader.NextAsInt();
            var date = wordReader.NextAsDate();
            var hour = wordReader.NextAsDecimal();
            return new TimeCardTransaction(date,hour,empId);
        }

        private Transaction DelEmp(string line,WordReader wordReader) {
            return new DeleteEmployeeTransaction(wordReader.NextAsInt());
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
            return new AddCommissionedEmployee(empId,name,address, wordReader.NextAsDecimal(), wordReader.NextAsDecimal());
        }

        private Transaction AddSalariedEmployee(int empId,string name,string address,WordReader wordReader) {
            return new AddSalariedEmployee(empId,name,address,wordReader.NextAsDecimal());
        }

        private Transaction AddHourlyEmployee(int empId,string name,string address,WordReader wordReader) {
            return new AddHourlyEmployee(empId,name,address,wordReader.NextAsDecimal());
        }

        
    }
}
