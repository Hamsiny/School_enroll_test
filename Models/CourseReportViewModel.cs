namespace UxtrataTask.Models;

public class CourseReportView
{
    public int CourseID { get; set; }
    public string CourseName { get; set; }
    public decimal Cost { get; set; }
    public List<StudentSelectionView> StudentSelectionViews { get; set; }
}