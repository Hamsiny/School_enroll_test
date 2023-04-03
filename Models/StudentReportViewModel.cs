namespace UxtrataTask.Models;

public class StudentReportView
{
    public int StudentID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalAmountPaid { get; set; }
    public decimal TotalAmountOwing { get; set; }

    public List<CourseSelectionView> CourseSelectionViews { get; set; }
}