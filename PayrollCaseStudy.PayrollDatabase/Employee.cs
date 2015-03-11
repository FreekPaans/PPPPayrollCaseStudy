using PayrollCaseStudy.Affiliations;
using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.Methods;
using PayrollCaseStudy.Pay;
using PayrollCaseStudy.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.PayrollDatabase {
    public class Employee {
        private int _employeeId;

        public int EmployeeId {
            get { return _employeeId; }
        }
        private string _name;

        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        private string _address;

        public string Address {
            get { return _address; }
            set { _address = value; }
        }

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
        public PaymentClassification Classification { get; set; }

        public PaymentSchedule Schedule { get; set; }

        public PaymentMethod Method { get; set; }

        

        public PaymentClassification GetClassification() {
            return Classification;
        }

        public PaymentSchedule GetSchedule() {
            return Schedule;
        }

        public PaymentMethod GetMethod() {
            return Method;
        }


        public bool IsPayDate(Date date) {
            return Schedule.IsPayDate(date);
        }

        public void Payday(Paycheck paycheck) {
            var grosspay = Classification.CalculatePay(paycheck);
            var deductions = Affiliation.CalculateDeductions(paycheck);
            var netPay = grosspay - deductions;

            paycheck.GrossPay = grosspay;
            paycheck.NetPay = netPay;
            paycheck.Deductions = deductions;
            Method.Pay(paycheck);
        }

        public Date GetPayPeriodStartDate(Date payPeriod) {
            return Schedule.GetPayPeriodStartDate(payPeriod);
        }

    }
}
