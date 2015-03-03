using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class Employee {
        private int _employeeId;
        private string _name;

        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        private string _address;

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
    }
}
