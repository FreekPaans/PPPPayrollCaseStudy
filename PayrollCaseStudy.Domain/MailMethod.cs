using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    public class MailMethod : PaymentMethod{
        private string _address;

        public string Address {
            get { return _address; }
        }

        public MailMethod(string address) {
            this._address = address;
        }
    }
}
