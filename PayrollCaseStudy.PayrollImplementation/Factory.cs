using PayrollCaseStudy.PayrollDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.PayrollImplementation {
    public class Factory : PayrollFactory.Factory{
        
        public PaymentSchedule MakeBiweeklySchedule() {
            return new BiweeklySchedule();
        }

        public PaymentClassification MakeCommissionedClassification(decimal salary,decimal commissionRate) {
            return new CommissionedClassification(salary,commissionRate);
        }

        public PaymentSchedule MakeWeeklySchedule() {
            return new WeeklySchedule();
        }

        public PaymentClassification MakeHourlyClassification(decimal hourlyRate) {
            return new HourlyClassification(hourlyRate);
        }

        public PaymentSchedule MakeMonthlySchedule() {
            return new MonthlySchedule();
        }

        public PaymentClassification MakeSalariedClassification(decimal itsSalary) {
            return new SalariedClassification(itsSalary);
        }

        public PaymentMethod MakeDirectMethod(string bank, string account) {
            return new DirectMethod(account,bank);
        }

        public PaymentMethod MakeHoldMethod() {
            return new HoldMethod();
        }

        public PaymentMethod MakeMailMethod(string address) {
            return new MailMethod(address);
        }

        public Affiliation MakeUnionAffiliation(int memberId,decimal weeklyDues) {
            return new UnionAffiliation(memberId,weeklyDues);
        }

        public Affiliation MakeNoAffiliation() {
            return new NoAffiliation();
        }
    }
}
