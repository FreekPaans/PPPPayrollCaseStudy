using PayrollCaseStudy.PayrollApplication;
using PayrollCaseStudy.TransactionApplication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.ConsoleHost {
    class PayrollApplication : TransactionApplication.Application{
        public PayrollApplication(TextParserTransactionSource source)  : base(source){
        }

        static void Main(string[] args) {
            PayrollDatabase.Scope.DatabaseInstance = InMemPayrollDatbase.Database.Instance;
            TransactionFactory.Scope.TransactionFactory = new TransactionImplementation.PayrollTransactionFactory();
            PayrollFactory.Scope.PayrollFactory = new PayrollImplementation.Factory();

            var reader = new StreamReader(new FileStream("TestTransactions.txt",FileMode.Open,FileAccess.Read));
            var parser = new TextParserTransactionSource(reader);
            var app = new PayrollApplication(parser);
            app.Process();
            return;
        }
    }
}
