using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UxtrataTask.Models;

[Table("student")]
public class Student
{
    [Key]
    [Column("student_id")]
    public int StudentID { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("age")]
    public int Age { get; set; }
    
    public List<CourseSelection> CourseSelections { get; set; }
        
}