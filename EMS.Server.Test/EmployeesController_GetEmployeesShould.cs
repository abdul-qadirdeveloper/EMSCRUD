using Ems.Controllers.Emsdb;
using Ems.Data;
using Ems.Models.Emsdb;
using Moq;
using Moq.EntityFrameworkCore;

namespace EMS.Server.Test
{
    public class EmployeesController_GetEmployeesShould
    {

        private readonly Mock<EmsdbContext> _mockEmsDbContext;
        public EmployeesController_GetEmployeesShould()
        {                
            _mockEmsDbContext = new Mock<EmsdbContext>();
        }

        [Fact]
        public void GetEmployees_EmployeesExists_ReturnsEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee{ Id = 1},
                new Employee{ Id = 2},
                new Employee{ Id = 3},
            };
            
            _mockEmsDbContext.Setup(x => x.Employees).ReturnsDbSet(employees);

            var employeesController = new EmployeesController(_mockEmsDbContext.Object);
            var returnedEmployees = employeesController.GetEmployees();
            Assert.Equal(employees.Count, returnedEmployees.Count());
        }

        [Fact]
        public void GetEmployee_EmployeeExist_ReturnsSameEmployee()
        {
            var employees = new List<Employee>
            {
                new Employee{ Id = 1}
            };

            _mockEmsDbContext.Setup(_ => _.Employees).ReturnsDbSet(employees);

            var employeesController = new EmployeesController(_mockEmsDbContext.Object);
            var returnedEmployee = employeesController.GetEmployee(1);
            Assert.Equal(employees[0], returnedEmployee.Queryable.FirstOrDefault());
        }
    }
}