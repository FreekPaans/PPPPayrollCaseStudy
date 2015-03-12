using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.PayrollDomain;
using PayrollCaseStudy.TransactionApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.ClassificationTransactions {
    public class TimeCardTransaction : Transaction{
        private int _empId;
        private decimal _hours;
        private Date _forDate;

        public TimeCardTransaction(Date date,decimal hours,int empId) {
            _forDate = date;
            _hours = hours;
            _empId = empId;
        }

        public void Execute() {
            var employee = PayrollDatabase.Scope.DatabaseInstance.GetEmployee(_empId);
            if(employee == null) {
                throw new Exception("No such employee");
            }

            var hourlyClassification = employee.GetClassification() as HourlyClassification;
            
            if(hourlyClassification == null) {
                throw new Exception("Tried to add timecard to non-hourly employee");
            }            

            hourlyClassification.AddTimeCard(new TimeCard(_forDate,_hours));
        }
    }
}
