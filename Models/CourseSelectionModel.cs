using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UxtrataTask.Models;

[Table("course_selection")]
public class CourseSelection
{
    [Key]
    [Column("id")]
    public int ID { get; set; }

    [Required]
    [Column("student_id")]
    [ForeignKey("Student")]
    public int StudentID { get; set; }

    [Required]
    [Column("course_id")]
    [ForeignKey("Course")]
    public int CourseID { get; set; }

    [Required]
    [Column("amount_paid", TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }

    [Required]
    [Column("payment_date")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }

    public virtual Student Student { get; set; }

    public virtual Course Course { get; set; }
}