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
            }
            throw new InvalidOperationException(string.Format("Cannot parse {0}", line));
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
