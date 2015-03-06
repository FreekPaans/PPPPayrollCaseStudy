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
            var perDay = _timeCards.Where(_=>IsInPayPeriod(_, paycheck.PayDate)).GroupBy(_=>_.Date).Select(_=>new { Date = _.Key, Hours = _.Sum(x=>x.Hours)}).ToList();

            return perDay.Sum(_=>CalculatePayForDay(_.Hours));
        }

        private bool IsInPayPeriod(TimeCard tc,Date payPeriod) {
            var payPeriodEndDate = payPeriod;
            var payPeriodStartDate = payPeriod.AddDays(-5);

            return tc.Date>=payPeriodStartDate && tc.Date<=payPeriodEndDate;
        }

        private decimal CalculatePayForDay(decimal hoursForDay) {
            if(hoursForDay<=8.0M) {
                return hoursForDay  * _hourlyRate;
            }

            return hoursForDay * _hourlyRate + (0.5M * _hourlyRate * (hoursForDay - 8.0M));
        }
    }
}
