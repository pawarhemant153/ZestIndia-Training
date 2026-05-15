using System.ComponentModel.DataAnnotations;

namespace Day14_EFCore_API.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }   

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string DepartmentName { get; set; }
    }
}
