namespace Day14_EFCore_API.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }


        //Navigation Property
        public List<Employee>? Employees { get; set; }
    }
}
