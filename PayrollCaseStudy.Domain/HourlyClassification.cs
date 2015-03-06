using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class HourlyClassification : PaymentClassification{
        private decimal _hourlyRate;
        readonly List<TimeCard> _timeCards;

        public decimal HourlyRate {
            get { return _hourlyRate; }
        }

        public HourlyClassification(decimal hourlyRate) {
            _hourlyRate = hourlyRate;
            _timeCards = new List<TimeCard>();
        }


        public TimeCard GetTimeCard(Date date) {
            return _timeCards.Single(_=>_.Date==date);
        }

        internal void AddTimeCard(TimeCard timeCard) {
            _timeCards.Add(timeCard);
        }

        public decimal CalculatePay(Paycheck paycheck) {
            return _timeCards.Sum(_=>_.Hours) * _hourlyRate;
        }
    }
}
