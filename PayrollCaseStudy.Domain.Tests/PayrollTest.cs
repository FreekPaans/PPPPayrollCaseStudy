using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayrollCaseStudy.Domain.Tests {
    [TestClass]
    public class PayrollTest {
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
    }
}
