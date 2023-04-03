using System.ComponentModel.DataAnnotations;

namespace UxtrataTask.Dtos;

public class CourseEditDto
{
    [Required(ErrorMessage = "Course Id is required")]
    public int CourseID { get; set; }
    
    [Required(ErrorMessage = "Course Name is required")]
    [StringLength(100, ErrorMessage = "Name length must be less than or equal to 100")]
    public string CourseName { get; set; }

    [Required(ErrorMessage = "Course Cost is required")]
    [Range(0.01, 100000, ErrorMessage = "The value must be greater than zero.")]
    public decimal Cost { get; set; }
}