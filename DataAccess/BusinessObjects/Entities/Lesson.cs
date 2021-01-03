using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.BusinessObjects.Entities
{
    public class Lesson
    {
        [Key] public int Id { get; set; }
        [Required] public string Tutor { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public string Classroom { get; set; }
        [Required] public int CourseId { get; set; }
        [ForeignKey("CourseId")] public virtual Course Course { get; set; }
        public virtual List<StudentData> StudentDatas { get; set; }
    }
}