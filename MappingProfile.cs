using AutoMapper;
using UxtrataTask.Dtos;
using UxtrataTask.Models;

namespace UxtrataTask;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        // student
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentEditDto, Student>();
        // courser
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseEditDto, Course>();
        // course selection
        CreateMap<CourseSelectionCreateDto, CourseSelection>();
        CreateMap<CourseSelectionEditDto, CourseSelection>();
    }
}