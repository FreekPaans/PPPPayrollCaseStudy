using System;
namespace PayrollCaseStudy.PayrollFactory {
    public interface Factory {
        PayrollCaseStudy.PayrollDomain.PaymentSchedule MakeBiweeklySchedule();
        PayrollCaseStudy.PayrollDomain.PaymentClassification MakeCommissionedClassification(decimal salary,decimal commissionRate);
        PayrollCaseStudy.PayrollDomain.PaymentMethod MakeDirectMethod(string bank, string account);
        PayrollCaseStudy.PayrollDomain.PaymentMethod MakeHoldMethod();
        PayrollCaseStudy.PayrollDomain.PaymentClassification MakeHourlyClassification(decimal hourlyRate);
        PayrollCaseStudy.PayrollDomain.PaymentMethod MakeMailMethod(string address);
        PayrollCaseStudy.PayrollDomain.PaymentSchedule MakeMonthlySchedule();
        PayrollCaseStudy.PayrollDomain.Affiliation MakeNoAffiliation();
        PayrollCaseStudy.PayrollDomain.PaymentClassification MakeSalariedClassification(decimal itsSalary);
        PayrollCaseStudy.PayrollDomain.Affiliation MakeUnionAffiliation(int memberId,decimal weeklyDues);
        PayrollCaseStudy.PayrollDomain.PaymentSchedule MakeWeeklySchedule();
    }
}
