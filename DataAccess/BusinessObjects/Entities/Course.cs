using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.BusinessObjects.Entities
{
    public enum CourseType
    {
        Lecture,
        Exercises,
        Laboratory,
        Project,
        Seminar
    }

    public class Course
    {
        [Key] public int Id { get; set; } // todo trzeba usunąć name
        [Required] public string Code { get; set; }
        [Required] public CourseType CourseType { get; set; }
        [Required] public int CourseGroupId { get; set; }
        [ForeignKey("CourseGroupId")] public virtual CourseGroup CourseGroup { get; set; }
        public virtual List<Lesson> Lessons { get; set; }
    }
}