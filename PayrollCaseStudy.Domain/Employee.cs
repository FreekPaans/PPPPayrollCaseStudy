using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class Employee {
        private int _employeeId;

        public int EmployeeId {
            get { return _employeeId; }
        }
        private string _name;

        public string Name {
            get { return _name; }
        }
        private string _address;

        private Affiliation _affiliation = new NoAffiliation();

        public Affiliation Affiliation {
            get { return _affiliation; }
            set { _affiliation = value; }
        }

        public Employee(int employeeId,string name,string address) {
            _employeeId = employeeId;
            _name = name;
            _address = address;
        }
        internal PaymentClassification Classification { get; set; }

        internal PaymentSchedule Schedule { get; set; }

        internal PaymentMethod Method { get; set; }

        

        public PaymentClassification GetClassification() {
            return Classification;
        }

        public PaymentSchedule GetSchedule() {
            return Schedule;
        }

        public PaymentMethod GetMethod() {
            return Method;
        }


        internal bool IsPayDate(Date date) {
            return Schedule.IsPayDate(date);
        }

        internal void Payday(Paycheck paycheck) {
            var grosspay = Classification.CalculatePay(paycheck);
            var deductions = Affiliation.CalculateDeductions(paycheck);
            var netPay = grosspay - deductions;

            paycheck.GrossPay = grosspay;
            paycheck.NetPay = netPay;
            paycheck.Deductions = deductions;
            Method.Pay(paycheck);
        }
    }
}
