using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollImplementation {
    public class CommissionedClassification : PaymentClassification{
        private decimal _salary;
        

        public decimal Salary {
            get { return _salary; }
        }
        private decimal _commissionRate;
        readonly List<SalesReceipt> _salesReceipts;

        public decimal CommissionRate {
            get { return _commissionRate; }
        }

        public CommissionedClassification(decimal salary,decimal commissionRate) {
            _salary = salary;
            _commissionRate = commissionRate;
            _salesReceipts = new List<SalesReceipt>();
        }

        public ICollection<SalesReceipt> GetSalesReceiptsForDate(Date forDate) {
            return _salesReceipts.Where(_=>_.Date == forDate).ToList();
        }

        public override void AddSalesReceipt(decimal amount, Date forDate) {
            _salesReceipts.Add(new SalesReceipt(amount,forDate));
        }

        public override decimal CalculatePay(Paycheck paycheck) {
            return _salary + _salesReceipts.Where(_=>IsInPayPeriod(_.Date, paycheck)).Sum(_=>_.Amount * _commissionRate);
        }

    }
}
