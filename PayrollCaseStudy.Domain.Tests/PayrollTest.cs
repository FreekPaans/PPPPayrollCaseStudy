using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayrollCaseStudy.Domain.Tests {
    [TestClass]
    public class PayrollTest {
        [TestInitialize]
        public void Init() {
            PayrollDatabase.Instance.Clear();
        }

        [TestMethod]
        public void TestAddSalariedEmployee() {
            int empId = 1;
            var t = new AddSalariedEmployee(empId, "Bob", "Home", 1000.0M);

            t.Execute();

            var e = PayrollDatabase.Instance.GetEmployee(empId);

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

            var e = PayrollDatabase.Instance.GetEmployee(empId);

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

            var e = PayrollDatabase.Instance.GetEmployee(empId);

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

            var employee = PayrollDatabase.Instance.GetEmployee(empId);

            Assert.IsNotNull(employee);

            var deleteTx = new DeleteEmployeeTransaction(empId);
            deleteTx.Execute();

            employee = PayrollDatabase.Instance.GetEmployee(empId);

            Assert.IsNull(employee);
        }

        [TestMethod]
        public void TestTimeCardTransaction() {
            int empId = 2;
            var addTx = new AddHourlyEmployee(empId,"Bill", "Home", 15.25M);
            addTx.Execute();

            
            var timeCardTransaction = new TimeCardTransaction(new Date(10,31,2001), 8.0M, empId);
            timeCardTransaction.Execute();

            var employee = PayrollDatabase.Instance.GetEmployee(empId);
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

            var employee = PayrollDatabase.Instance.GetEmployee(empId);
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
            var addTx = new AddHourlyEmployee(empId,"Bill", "Home", 15.25M);
            addTx.Execute();

            var employee = PayrollDatabase.Instance.GetEmployee(empId);

            var unionAffiliation = new UnionAffiliation(12.5M);

            employee.Affiliation = unionAffiliation;

            int memberId = 86;
            
            PayrollDatabase.Instance.AddUnionMember(memberId,employee);

            var serviceChargeTransaction = new ServiceChargeTransaction(memberId, 20011101,12.95M);
            serviceChargeTransaction.Execute();

            var serviceCharge = unionAffiliation.GetServiceCharge(20011101);
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

            var employee = PayrollDatabase.Instance.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");
            Assert.AreEqual("Bob", employee.Name);
        }

        [TestMethod]
        public void TestChangeHourlyTransaction() {
            var empId = 1;
            var addTx = new AddCommissionedEmployee(empId,"Lance","Home",2500,3.2M);
            addTx.Execute();
            
            var changeHourlyTx = new ChangeHourlyTransaction(empId,27.25M);
            changeHourlyTx.Execute();
            
            var employee = PayrollDatabase.Instance.GetEmployee(empId);

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
            
            var changeHourlyTx = new ChangeSalariedTransaction(empId,2000M);
            changeHourlyTx.Execute();
            
            var employee = PayrollDatabase.Instance.GetEmployee(empId);

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
            
            var changeHourlyTx = new ChangeCommissionedTransaction(empId,2000M,0.2M);
            changeHourlyTx.Execute();
            
            var employee = PayrollDatabase.Instance.GetEmployee(empId);

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
            
            var changeHourlyTx = new ChangeDirectTransaction(empId,"Citigroup", "12345678");
            changeHourlyTx.Execute();
            
            var employee = PayrollDatabase.Instance.GetEmployee(empId);

            Assert.IsNotNull(employee, "employee not found in database");

            var method = employee.GetMethod();

            Assert.IsInstanceOfType(method, typeof(DirectMethod),"employee does not have correct payment method");

            var directMethod = (DirectMethod)method;

            Assert.AreEqual("Citigroup", directMethod.Bank);
            Assert.AreEqual("12345678", directMethod.Account);
        }

        //[TestMethod]
        //public void TestSalariedUnionMemberDues() {
        //    int empId = 1;

        //    var addTx = new AddSalariedEmployee(empId,"Bob", "Home", 1000);
        //    addTx.Execute();

        //    var membedId = 7734;
        //    var changeTx = new ChangeMemberTransaction


        //    var payDate = new Date(11,16,2001);
        //    var paydayTx = new PaydayTransaction(payDate);
        //    paydayTx.Execute();

        //    var paycheck = paydayTx.GetPaycheck(empId);

        //    ValidateCommisionedPaycheck(paydayTx,empId,payDate,1000M);
        //}

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
