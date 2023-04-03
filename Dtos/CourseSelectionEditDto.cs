using System.ComponentModel.DataAnnotations;

namespace UxtrataTask.Dtos;

public class CourseSelectionEditDto
{
    [Required(ErrorMessage = "Id is required")]
    public int ID { get; set; }
    
    [Required(ErrorMessage = "Student Id is required")]
    public int StudentID { get; set; }

    [Required(ErrorMessage = "Course Id is required")]
    public int CourseID { get; set; }

    [Required(ErrorMessage = "Paid Amount is required")]
    [Range(0.01, 100000, ErrorMessage = "The value must be greater than zero.")]
    public decimal AmountPaid { get; set; }

    [Required(ErrorMessage = "Payment Date is required")]
    public DateTime PaymentDate { get; set; }
}