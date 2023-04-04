using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WebForms;
using UxtrataTask.Context;
using UxtrataTask.Models;
using UxtrataTask.Repository;

namespace UxtrataTask.Controllers;

public class ReportController : Controller
{
    private readonly IGenericMySqlAccessRepository<Student> _studentRepo;
    private readonly IGenericMySqlAccessRepository<Course> _courseRepo;

    public ReportController(IGenericMySqlAccessRepository<Student> studentRepo, IGenericMySqlAccessRepository<Course> courseRepop)
    {
        _studentRepo = studentRepo;
        _courseRepo = courseRepop;
    }

    public IActionResult StudentReport()
    {
        var studentReportData = GetStudentReportData();

        var reportViewer = new ReportViewer
        {
            ProcessingMode = ProcessingMode.Local,
            SizeToReportContent = true,
            // Width = Unit.Percentage(100),
            // Height = Unit.Percentage(100), // need to modify later
        };
        
        var localReport = new LocalReport
        {
            ReportPath = @"Reports\StudentReport.rdlc"
        };
        
        var reportDataSource = new ReportDataSource
        {
            Name = "StudentReportData",
            Value = studentReportData
        };
        
        reportViewer.LocalReport.DataSources.Add(reportDataSource);
        reportViewer.LocalReport.ReportPath = localReport.ReportPath;
        
        var reportType = "PDF";
        string mimeType;
        string encoding;
        string fileNameExtension;
        var deviceInfo = $@"<DeviceInfo><OutputFormat>{reportType}</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight><MarginTop>0.5in</MarginTop><MarginLeft>1in</MarginLeft><MarginRight>1in</MarginRight><MarginBottom>0.5in</MarginBottom></DeviceInfo>";
        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;
        
        renderedBytes = reportViewer.LocalReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        
        // return File(renderedBytes, mimeType);
        
        return new FileStreamResult(new MemoryStream(renderedBytes), "application/pdf")
        {
            FileDownloadName = "StudentReport.pdf"
        };
    }

    public IActionResult ReportView()
    {
        var studentReportData = GetStudentReportData();

        return View(studentReportData);
    }

    private List<StudentReportView> GetStudentReportData()
    {
        var students = _studentRepo.GetQueryable().Include(s => s.CourseSelections).ThenInclude(cs => cs.Course).ToList();

        var studentReportData = new List<StudentReportView>();

        foreach (var student in students)
        {
            var totalCost = student.CourseSelections.Sum(cs => cs.Course.Cost);
            var totalAmountPaid = student.CourseSelections.Sum(cs => cs.AmountPaid);
            var totalAmountOwing = totalCost - totalAmountPaid;

            studentReportData.Add(new StudentReportView
            {
                StudentID = student.StudentID,
                Name = student.Name,
                Age = student.Age,
                TotalCost = totalCost,
                TotalAmountPaid = totalAmountPaid,
                TotalAmountOwing = totalAmountOwing,
                CourseSelectionViews = student.CourseSelections.Select(cs => new CourseSelectionView
                {
                    CourseName = cs.Course.CourseName,
                    Cost = cs.Course.Cost,
                    AmountPaid = cs.AmountPaid,
                    PaymentDate = cs.PaymentDate
                }).ToList()
            });
        }

        return studentReportData;
    }
    
    public IActionResult ReportCourseView()
    {
        var courseReportData = GetCourseReportData();

        return View(courseReportData);
    }

    private List<CourseReportView> GetCourseReportData()
    {
        var courses = _courseRepo.GetQueryable().Include(c => c.CourseSelections).ThenInclude(cs => cs.Student)
            .ToList();
        var courseReportData = new List<CourseReportView>();
        
        foreach (var course in courses)
        {
            // var totalCost = student.CourseSelections.Sum(cs => cs.Course.Cost);
            // var totalAmountPaid = student.CourseSelections.Sum(cs => cs.AmountPaid);
            // var totalAmountOwing = totalCost - totalAmountPaid;

            courseReportData.Add(new CourseReportView()
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                Cost = course.Cost,
                // TotalCost = totalCost,
                // TotalAmountPaid = totalAmountPaid,
                // TotalAmountOwing = totalAmountOwing,
                StudentSelectionViews = course.CourseSelections.Where(cs => cs.CourseID == course.CourseID).Select(cs => new StudentSelectionView()
                {
                    Name = cs.Student.Name,
                    Age = cs.Student.Age,
                    Cost = cs.Course.Cost,
                    AmountPaid = cs.AmountPaid,
                    PaymentDate = cs.PaymentDate
                }).ToList()
            });
        }

        return courseReportData;
    }
}