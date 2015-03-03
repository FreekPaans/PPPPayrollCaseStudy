using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayrollCaseStudy.Domain.Tests {
    [TestClass]
    public class PayrollTest {
        [TestMethod]
        public void TestAddSalariedEmployee() {
            int empId = 1;
            var t = new AddSalariedEmployee(empId, "Bob", "Home", 1000.0);

            t.Execute();

            var e = GpayrollDatabase.GetEmployee(empId);

            Assert.AreEqual("Bob", e.Name);

            PaymentClassification pc = e.GetClassification();

            SalariedClassification sc = (SalariedClassification)pc;

            Assert.AreEqual(sc.GetSalary(), 1000.00);

            PaymentSchedule ps = e.GetSchedule();

            Assert.IsInstanceOfType(ps, typeof(MonthlySchedule));

            PaymentMethod pm = e.GetMethod();

            Assert.IsInstanceOfType(pm, typeof(HoldMethod));
        }
    }
}
