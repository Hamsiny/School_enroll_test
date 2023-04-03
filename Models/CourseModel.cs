using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UxtrataTask.Models;

[Table("course")]
public class Course
{
    [Key]
    [Column("course_id")]
    public int CourseID { get; set; }

    [Required]
    [Column("course_name")]
    public string CourseName { get; set; }
    
    [Required]
    [Column("cost", TypeName = "decimal(18,2)")]
    public decimal Cost { get; set; }

    public List<CourseSelection> CourseSelections { get; set; }
}