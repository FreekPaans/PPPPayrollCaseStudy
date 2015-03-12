using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollCaseStudy.PayrollDatabase;
using PayrollCaseStudy.Classifications;
using PayrollCaseStudy.Methods;
using PayrollCaseStudy.CommonTypes;
using PayrollCaseStudy.Affiliations;
using PayrollCaseStudy.PayrollDomain;
using PayrollCaseStudy.TransactionImplementation;

namespace PayrollCaseStudy.Domain.Tests {
    [TestClass]
    public class PayrollTest {
        readonly static InMemPayrollDatbase.Database Database = InMemPayrollDatbase.Database.Instance;

        [ClassInitialize]
        public static void InitClass(TestContext t) {
            PayrollDatabase.Scope.DatabaseInstance = Database;
        }

        [TestInitialize]
        public void Init() {
            Database.Clear();
        }

        [TestMethod]
        public void TestAddSalariedEmployee() {
            int empId = 1;
            var t = new AddSalariedEmployee(empId, "Bob", "Home", 1000.0M);

            t.Execute();

            var e = Database.GetEmployee(empId);

            Assert.AreEqual("Bob", e.Name);

            PaymentClassification pc = e.GetClassification();

            SalariedClassification sc = (SalariedClassification)pc;

            Assert.AreEqual(sc.Salary, 1000.00M);

            PaymentSchedule ps = e.GetSchedule();

            Assert.IsInstanceOfType(ps, typeof(MonthlySchedule));

            PaymentMethod pm = e.GetMethod();

            Assert.IsInstanceOfType(pm, typeof(HoldMethod));
        }

        [TestMethod]
        public void TestAddHourlyEmployee() {
            int empId = 1;
            var t = new AddHourlyEmployee(empId, "Bob", "Home", 10.0M);

            t.Execute();

            var e = Database.GetEmployee(empId);

            Assert.AreEqual("Bob", e.Name);

            PaymentClassification pc = e.GetClassification();

            HourlyClassification sc = (HourlyClassification)pc;

            Assert.AreEqual(sc.HourlyRate, 10.00M);

            PaymentSchedule ps = e.GetSchedule();

            Assert.IsInstanceOfType(ps, typeof(WeeklySchedule));

            PaymentMethod pm = e.GetMethod();

            Assert.IsInstanceOfType(pm, typeof(HoldMethod));
        }

        [TestMethod]
        public void TestAddCommisionedEmployee() {
            int empId = 1;
            var t = new AddCommissionedEmployee(empId, "Bob", "Home", 1000.0M, 50.0M);

            t.Execute();

            var e = Database.GetEmployee(empId);

            Assert.AreEqual("Bob", e.Name);

            PaymentClassification pc = e.GetClassification();

            CommissionedClassification sc = (CommissionedClassification)pc;

            Assert.AreEqual(sc.Salary, 1000.00M);
            Assert.AreEqual(sc.CommissionRate, 50.0M);

            PaymentSchedule ps = e.GetSchedule();

            Assert.IsInstanceOfType(ps, typeof(BiweeklySchedule));

            PaymentMethod pm = e.GetMethod();

            Assert.IsInstanceOfType(pm, typeof(HoldMethod));
        }

        [TestMethod]
        public void TestDeleteEmployee() {
            int empId = 3;
            var addTx = new AddCommissionedEmployee(empId,"Lance", "Home", 2500, 3.2M);
            addTx.Execute();

            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee);

            var deleteTx = new DeleteEmployeeTransaction(empId);
            deleteTx.Execute();

            employee = Database.GetEmployee(empId);

            Assert.IsNull(employee);
        }

        [TestMethod]
        public void TestTimeCardTransaction() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill", "Home", 15.25M);
            addTx.Execute();

            
            var timeCardTransaction = new TimeCardTransaction(new Date(10,31,2001), 8.0M, empId);
            timeCardTransaction.Execute();

            var employee = Database.GetEmployee(empId);
            Assert.IsNotNull(employee);

            PaymentClassification classification = employee.GetClassification();
            var hourlyClassification = (HourlyClassification)classification;

            var timeCard = hourlyClassification.GetTimeCard(new Date(10,31,2001));
            Assert.IsNotNull(timeCard);
            Assert.AreEqual(8.0M, timeCard.Hours);
        }

        [TestMethod]
        public void TestSalesReceiptTransaction() {
            int empId = 2;
            var addTx = new AddCommissionedEmployee(empId,"Bill", "Home", 1000M,3.2M);
            addTx.Execute();

            var salesReceiptTX = new SalesReceiptTransaction(1000M, new Date(10,31,2001), empId);

            salesReceiptTX.Execute();

            var employee = Database.GetEmployee(empId);
            Assert.IsNotNull(employee);

            PaymentClassification classification = employee.GetClassification();
            var commissionedClassification = (CommissionedClassification)classification;

            var receipts = commissionedClassification.GetSalesReceiptsForDate(new Date(10,31,2001));
            Assert.AreEqual(1,receipts.Count, "Receipt count for date is not 1");
            var firstReceipt = receipts.First();
            Assert.AreEqual(1000M, firstReceipt.Amount);
        }

        [TestMethod]
        public void TestAddServiceCharge() {
            int empId = 2;
            int memberId = 7734;
            var addTx = new AddHourlyEmployee(empId,"Bill", "Home", 15.25M);
            addTx.Execute();

            var employee = Database.GetEmployee(empId);

            var unionAffiliation = new UnionAffiliation(memberId,12.5M);

            employee.Affiliation = unionAffiliation;

            Database.AddUnionMember(memberId,employee);

            var serviceChargeTransaction = new ServiceChargeTransaction(memberId, new Date(11,01,2001),12.95M);
            serviceChargeTransaction.Execute();

            var serviceCharge = unionAffiliation.GetServiceCharge(new Date(11,01,2001));
            Assert.IsNotNull(serviceCharge);
            Assert.AreEqual(12.95M,serviceCharge.Amount);
        }

        [TestMethod]
        public void TestPaySingleSalariedEmployee() {
            int empId = 1;

            var addTx = new AddSalariedEmployee(empId,"Bob", "Home", 1000);
            addTx.Execute();

            var payDate = new Date(11,30,2001);

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            Assert.IsNotNull(paycheck);
            Assert.AreEqual(payDate,paycheck.PayPeriodEndDate);
            Assert.AreEqual(1000M,paycheck.GrossPay);
            Assert.AreEqual("Hold", paycheck.GetField("Disposition"));
            Assert.AreEqual(0M,paycheck.Deductions);
            Assert.AreEqual(1000M, paycheck.NetPay);
        }

        [TestMethod]
        public void TestPaySingleSalariedEmployeeOnWrongDate() {
            int empId = 1;

            var addTx = new AddSalariedEmployee(empId,"Bob", "Home", 1000);
            addTx.Execute();

            var payDate = new Date(11,29,2001);

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            Assert.IsNull(paycheck);
        }

        [TestMethod]
        public void TestPaySingleHourlyEmployeeNoTimeCards(){
            int empId = 2;

            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var payDate = new Date(11,9,2001);
            Assert.AreEqual(DayOfWeek.Friday,payDate.DayOfWeek);

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();


            ValidateHourlyPaycheck(paydayTx,empId,payDate,0.0M);
        }

        [TestMethod]
        public void TestPaySingleHourlyEmployeeOneTimeCard() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var payDate = new Date(11,9,2001);
            Assert.AreEqual(DayOfWeek.Friday,payDate.DayOfWeek);
            var timecardTx = new TimeCardTransaction(payDate,2.0M,empId);
            timecardTx.Execute();

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();
            ValidateHourlyPaycheck(paydayTx,empId,payDate,30.5M);
        }

        [TestMethod]
        public void TestPaySingleHourlyEmployeeOvertimeOneTimeCard() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var payDate = new Date(11,9,2001);
            Assert.AreEqual(DayOfWeek.Friday,payDate.DayOfWeek);

            var timecardTx = new TimeCardTransaction(payDate,9.0M,empId);
            timecardTx.Execute();

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            ValidateHourlyPaycheck(paydayTx,empId,payDate,(8+1.5M)*15.25M);
        }

        [TestMethod]
        public void TestPaySingleHourlyEmployeeOnWrongDate() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var payDate = new Date(11,8,2001);
            Assert.AreEqual(DayOfWeek.Thursday,payDate.DayOfWeek);

            var timecardTx = new TimeCardTransaction(payDate,9.0M,empId);
            timecardTx.Execute();

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            Assert.IsNull(paycheck, "paycheck is available when payday is calculated on a thursday, should only be on friday");
        }

        [TestMethod]
        public void TestPaySingleHourlyEmployeeTwoTimeCards() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var payDate = new Date(11,9,2001);
            Assert.AreEqual(DayOfWeek.Friday,payDate.DayOfWeek);
            var timecardTx1 = new TimeCardTransaction(payDate,2.0M,empId);
            timecardTx1.Execute();
            var timecardTx2 = new TimeCardTransaction(new Date(11,8,2001),5.0M,empId);
            timecardTx2.Execute();

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();
            ValidateHourlyPaycheck(paydayTx,empId,payDate,7*15.25M);
        }

        [TestMethod]
        public void TestPaySingleHourlyEmployeeWithTimeCardsSpanningTwoPayPeriods() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var payDate = new Date(11,9,2001);
            var dateInPreviousPeriod= new Date(11,2,2001);
            Assert.AreEqual(DayOfWeek.Friday,payDate.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Friday,dateInPreviousPeriod.DayOfWeek);

            var timecardTx1 = new TimeCardTransaction(payDate,2.0M,empId);
            timecardTx1.Execute();
            var timecardTx2 = new TimeCardTransaction(dateInPreviousPeriod,5.0M,empId);
            timecardTx2.Execute();

            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();
            ValidateHourlyPaycheck(paydayTx,empId,payDate,2*15.25M);
        }

        [TestMethod]
        public void TestPaySingleCommissionedEmployee() {
            int empId = 1;

            var addTx = new AddCommissionedEmployee(empId,"Bob", "Home", 1000,0.2M);
            addTx.Execute();

            var payDate = new Date(11,16,2001);
            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            
            ValidateCommisionedPaycheck(paydayTx,empId,payDate,1000M);
        }

        

        [TestMethod]
        public void TestPaySingleCommissionedEmployeeWrongDate() {
            int empId = 1;

            var addTx = new AddCommissionedEmployee(empId,"Bob", "Home", 1000,0.2M);
            addTx.Execute();

            var payDate = new Date(11,9,2001);
            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            Assert.IsNull(paycheck, "paycheck expected null because it's not a paydate");
        }

        [TestMethod]
        public void TestPaySingleCommissionedEmployeeWithSingleSale() {
            int empId = 1;

            var addTx = new AddCommissionedEmployee(empId,"Bob", "Home", 1000,0.2M);
            addTx.Execute();

            var saleTx = new SalesReceiptTransaction(100, new Date(11,9,2001),empId);
            saleTx.Execute();

            var payDate = new Date(11,16,2001);
            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            ValidateCommisionedPaycheck(paydayTx,empId,payDate,1020M);
        }

        [TestMethod]
        public void TestPaySingleCommissionedEmployeeWithTwoSales() {
            int empId = 1;

            var addTx = new AddCommissionedEmployee(empId,"Bob", "Home", 1000,0.2M);
            addTx.Execute();

            var saleTx1 = new SalesReceiptTransaction(100, new Date(11,9,2001),empId);
            saleTx1.Execute();

            var saleTx2 = new SalesReceiptTransaction(500, new Date(11,9,2001),empId);
            saleTx2.Execute();

            var payDate = new Date(11,16,2001);
            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            ValidateCommisionedPaycheck(paydayTx,empId,payDate,1120M);
        }

        [TestMethod]
        public void TestPaySingleCommissionedEmployeeNotForThisPayperiod() {
            int empId = 1;

            var addTx = new AddCommissionedEmployee(empId,"Bob", "Home", 1000,0.2M);
            addTx.Execute();

            var saleTx = new SalesReceiptTransaction(100, new Date(11,2,2001),empId);
            saleTx.Execute();


            var payDate = new Date(11,16,2001);
            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            ValidateCommisionedPaycheck(paydayTx,empId,payDate,1000M);
        }

        [TestMethod]
        public void TestChangeNameTransaction() {
            var empId = 1;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var changeNameTx = new ChangeNameTransaction(empId,"Bob");
            changeNameTx.Execute();

            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");
            Assert.AreEqual("Bob", employee.Name);
        }

        [TestMethod]
        public void TestChangeAddressTransaction() {
            var empId = 1;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.25M);
            addTx.Execute();
            var changeNameTx = new ChangeAddressTransaction(empId,"Work");
            changeNameTx.Execute();

            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");
            Assert.AreEqual("Work", employee.Address);
        }

        [TestMethod]
        public void TestChangeHourlyTransaction() {
            var empId = 1;
            var addTx = new AddCommissionedEmployee(empId,"Lance","Home",2500,3.2M);
            addTx.Execute();
            
            var changeHourlyTx = new ChangeHourlyTransaction(empId,27.25M);
            changeHourlyTx.Execute();
            
            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var classification = employee.GetClassification();

            Assert.IsInstanceOfType(classification, typeof(HourlyClassification),"employee does not have hourly classification");

            var hourlyClassification = (HourlyClassification)classification;

            Assert.AreEqual(27.25M,hourlyClassification.HourlyRate);

            var schedule = employee.GetSchedule();

            Assert.IsInstanceOfType(schedule, typeof(WeeklySchedule),"schedule is not weekly");
        }

        [TestMethod]
        public void TestChangeSalariedTransaction() {
            var empId = 1;
            var addTx = new AddCommissionedEmployee(empId,"Lance","Home",2500,3.2M);
            addTx.Execute();
            
            var changeSalariedTx = new ChangeSalariedTransaction(empId,2000M);
            changeSalariedTx.Execute();
            
            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var classification = employee.GetClassification();

            Assert.IsInstanceOfType(classification, typeof(SalariedClassification),"employee does not have salaried classification");

            var salariedClassification = (SalariedClassification)classification;

            Assert.AreEqual(2000M,salariedClassification.Salary);

            var schedule = employee.GetSchedule();

            Assert.IsInstanceOfType(schedule, typeof(MonthlySchedule),"schedule is not monthly");
        }

        [TestMethod]
        public void TestChangeCommissionedTransaction() {
            var empId = 1;
            var addTx = new AddSalariedEmployee(empId,"Lance","Home",2500);
            addTx.Execute();
            
            var changeCommisionedTx = new ChangeCommissionedTransaction(empId,2000M,0.2M);
            changeCommisionedTx.Execute();
            
            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var classification = employee.GetClassification();

            Assert.IsInstanceOfType(classification, typeof(CommissionedClassification),"employee does not have commissioned classification");

            var salariedClassification = (CommissionedClassification)classification;

            Assert.AreEqual(2000M,salariedClassification.Salary);
            Assert.AreEqual(0.2M,salariedClassification.CommissionRate);

            var schedule = employee.GetSchedule();

            Assert.IsInstanceOfType(schedule, typeof(BiweeklySchedule),"schedule is not biweekly");
        }

        [TestMethod]
        public void TestChangeDirectTransaction() {
            var empId = 1;
            var addTx = new AddSalariedEmployee(empId,"Lance","Home",2500);
            addTx.Execute();
            
            var changeDirectTx = new ChangeDirectTransaction(empId,"Citigroup", "12345678");
            changeDirectTx.Execute();
            
            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var method = employee.GetMethod();

            Assert.IsInstanceOfType(method, typeof(DirectMethod),"employee does not have correct payment method");

            var directMethod = (DirectMethod)method;

            Assert.AreEqual("Citigroup", directMethod.Bank);
            Assert.AreEqual("12345678", directMethod.Account);
        }

        [TestMethod]
        public void TestChangeMailTransaction() {
            var empId = 1;
            var addTx = new AddSalariedEmployee(empId,"Lance","Home",2500);
            addTx.Execute();
            
            var changeMailTx = new ChangeMailTransaction(empId,"Home");
            changeMailTx.Execute();
            
            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var method = employee.GetMethod();

            Assert.IsInstanceOfType(method, typeof(MailMethod),"employee does not have correct payment method");

            var mailMethod = (MailMethod)method;

            Assert.AreEqual("Home", mailMethod.Address);
        }

        [TestMethod]
        public void TestChangeHoldTransaction() {
            var empId = 1;
            var addTx = new AddSalariedEmployee(empId,"Lance","Home",2500);
            addTx.Execute();
            
            var changeMailTx = new ChangeMailTransaction(empId,"Home");
            changeMailTx.Execute();

            var changeHoldTx = new ChangeHoldTransaction(empId);
            changeHoldTx.Execute();

            var employee = Database.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var method = employee.GetMethod();

            Assert.IsInstanceOfType(method, typeof(HoldMethod),"employee does not have correct payment method");
        }

        [TestMethod]
        public void TestChangeMemberTransaction() {
            int empId = 2;
            int memberId = 7734;

            var addTx = new AddHourlyEmployee(empId,"Bill", "Home", 15.25M);
            addTx.Execute();
            var changeMemberTx = new ChangeMemberTransaction(empId,memberId,99.42M);
            changeMemberTx.Execute();

            var employee = Database.GetEmployee(empId);
            Assert.IsNotNull(employee, "Employee not found");

            var affiliation = employee.Affiliation;

            Assert.IsInstanceOfType(affiliation,typeof(UnionAffiliation), "Not union affiliation");
            var unionAffiliation = (UnionAffiliation)affiliation;
            Assert.AreEqual(99.42M, unionAffiliation.Dues);

            var member = Database.GetUnionMember(memberId);
            Assert.IsNotNull(member,"Member not found");
            Assert.AreEqual(employee,member);
        }

        [TestMethod]
        public void TestUnaffiliatedTransaction() {
            int empId = 2;
            int memberId = 7734;

            var addTx = new AddHourlyEmployee(empId,"Bill", "Home", 15.25M);
            addTx.Execute();
            var changeMemberTx = new ChangeMemberTransaction(empId,memberId,99.42M);
            changeMemberTx.Execute();


            var changeUnaffiliatedTx = new ChangeUnaffiliatedTransaction(empId);
            changeUnaffiliatedTx.Execute();

            var employee = Database.GetEmployee(empId);
            Assert.IsNotNull(employee, "Employee not found");

            var affiliation = employee.Affiliation;

            Assert.IsInstanceOfType(affiliation,typeof(NoAffiliation), "Has a union affiliation");
            
            var member = Database.GetUnionMember(memberId);
            Assert.IsNull(member,"Membership was not removed from database");
        }

        [TestMethod]
        public void TestSalariedUnionMemberDues() {
            int empId = 1;

            var addTx = new AddSalariedEmployee(empId,"Bob", "Home", 1000);
            addTx.Execute();

            var memberId = 7734;
            var changeTx = new ChangeMemberTransaction(empId,memberId,9.42M);
            changeTx.Execute();

            var payDate = new Date(11,30,2001);
            var paydayTx = new PaydayTransaction(payDate);
            paydayTx.Execute();

            var paycheck = paydayTx.GetPaycheck(empId);

            ValidatePaycheck(paydayTx,empId,payDate, 1000M, 5 * 9.42M);
        }

        [TestMethod]
        public void TestHourlyUnionMemberServiceCharge() {
            var empId = 1;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.24M);
            addTx.Execute();
            int memberId = 7734;
            var memberTx = new ChangeMemberTransaction(empId,memberId,9.42M);
            memberTx.Execute();
            var payDate = new Date(11,9,2001);
            var serviceChargeTx = new ServiceChargeTransaction(memberId,payDate,19.42M);
            serviceChargeTx.Execute();
            var timecardTx = new TimeCardTransaction(payDate,8,empId);
            timecardTx.Execute();
            var payDayTx = new PaydayTransaction(payDate);
            payDayTx.Execute();
            var paycheck = payDayTx.GetPaycheck(empId);

            Assert.IsNotNull(paycheck, "No paycheck available");
            Assert.AreEqual(payDate,paycheck.PayPeriodEndDate);
            Assert.AreEqual(8*15.24M,paycheck.GrossPay);
            Assert.AreEqual("Hold", paycheck.GetField("Disposition"));
            Assert.AreEqual(9.42M+19.42M,paycheck.Deductions);
            Assert.AreEqual((8*15.24M) - (9.42M+19.42M), paycheck.NetPay);
        }

        [TestMethod]
        public void TestServiceChargesSpanningMultiplePayPeriods() {
            var empId = 1;
            var addTx = new AddHourlyEmployee(empId,"Bill","Home",15.24M);
            addTx.Execute();
            int memberId = 7734;
            var memberTx = new ChangeMemberTransaction(empId,memberId,9.42M);
            memberTx.Execute();
            var earlyDate = new Date(11,2,2001); //previous friday
            var payDate = new Date(11,9,2001);
            var lateDate = new Date(11,16,2001); //this friday

            var serviceChargeTx1 = new ServiceChargeTransaction(memberId,payDate,19.42M);
            serviceChargeTx1.Execute();
            var serviceChargeTx2 = new ServiceChargeTransaction(memberId,earlyDate,100M);
            serviceChargeTx2.Execute();
            var serviceChargeTx3 = new ServiceChargeTransaction(memberId,lateDate,200M);
            serviceChargeTx3.Execute();

            var timecardTx = new TimeCardTransaction(payDate,8,empId);
            timecardTx.Execute();

            var payDayTx = new PaydayTransaction(payDate);
            payDayTx.Execute();
            var paycheck = payDayTx.GetPaycheck(empId);

            Assert.IsNotNull(paycheck, "No paycheck available");
            Assert.AreEqual(payDate,paycheck.PayPeriodEndDate);
            Assert.AreEqual(8*15.24M,paycheck.GrossPay);
            Assert.AreEqual("Hold", paycheck.GetField("Disposition"));
            Assert.AreEqual(9.42M+19.42M,paycheck.Deductions, "Deductions are incorrect");
            Assert.AreEqual((8*15.24M) - (9.42M+19.42M), paycheck.NetPay);
        }

        private void ValidatePaycheck(PaydayTransaction paydayTx,int empId,Date payDate,decimal grosspay, decimal deductions) {
            var paycheck = paydayTx.GetPaycheck(empId);
            Assert.IsNotNull(paycheck);
            Assert.AreEqual(payDate,paycheck.PayPeriodEndDate);
            Assert.AreEqual(grosspay,paycheck.GrossPay);
            Assert.AreEqual("Hold",paycheck.GetField("Disposition"));
            Assert.AreEqual(deductions,paycheck.Deductions);
            Assert.AreEqual(grosspay - deductions,paycheck.NetPay);
        }

        private static void ValidateCommisionedPaycheck(PaydayTransaction paydayTx,int empId,Date payDate,decimal pay) {
            var paycheck = paydayTx.GetPaycheck(empId);
            Assert.IsNotNull(paycheck);
            Assert.AreEqual(payDate,paycheck.PayPeriodEndDate);
            Assert.AreEqual(pay,paycheck.GrossPay);
            Assert.AreEqual("Hold",paycheck.GetField("Disposition"));
            Assert.AreEqual(0M,paycheck.Deductions);
            Assert.AreEqual(pay,paycheck.NetPay);
        }

        private void ValidateHourlyPaycheck(PaydayTransaction paydayTx,int empId,Date payDate,decimal pay) {
            var paycheck = paydayTx.GetPaycheck(empId);
            Assert.IsNotNull(paycheck);
            Assert.AreEqual(payDate,paycheck.PayPeriodEndDate);
            Assert.AreEqual(pay,paycheck.GrossPay);
            Assert.AreEqual("Hold", paycheck.GetField("Disposition"));
            Assert.AreEqual(0.0M, paycheck.Deductions);
            Assert.AreEqual(pay,paycheck.NetPay);
        }
    }
}
