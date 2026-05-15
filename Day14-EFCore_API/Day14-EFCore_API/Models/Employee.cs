namespace Day14_EFCore_API.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Salary { get; set; }

        //Foreign Key
        public int DepartmentId { get; set; }   

        //Navigation Property
        public Department? Department { get; set; }
    }
}
