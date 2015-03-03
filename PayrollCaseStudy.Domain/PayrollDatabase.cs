using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollCaseStudy.Domain {
    public class PayrollDatabase {
        readonly Dictionary<int, Employee> _itsEmployees = new Dictionary<int,Employee>();

        public Employee GetEmployee(int employeeId) {
            return _itsEmployees[employeeId];
        }
        public void AddEmployee(int employeeId, Employee employee) {
            _itsEmployees[employeeId] = employee;
        }
        public void Clear() {
            _itsEmployees.Clear();
        }

        PayrollDatabase(){}

        public readonly static PayrollDatabase Instance = new PayrollDatabase();
    }
}
