using System.ComponentModel.DataAnnotations;

namespace UxtrataTask.Dtos;

public class StudentEditDto
{
    [Required(ErrorMessage = "Student Id is required")]
    public int StudentID { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name length must be less than or equal to 100")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Age is required")]
    [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
    public int Age { get; set; }
}