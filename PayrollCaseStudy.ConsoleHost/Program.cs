using PayrollCaseStudy.PayrollApplication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.ConsoleHost {
    class Program {
        static void Main(string[] args) {
            PayrollDatabase.Scope.DatabaseInstance = InMemPayrollDatbase.Database.Instance;
            var reader = new StreamReader(new FileStream("TestTransactions.txt",FileMode.Open,FileAccess.Read));
            var parser = new TextParserTransactionSource(reader);
            var app = new Application(parser);
            app.Process();
            return;
        }
    }
}
