using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.BusinessObjects.Entities
{
    public class CourseGroup
    {
        [Key] public int Id { get; set; }
        [Required] public string Code { get; set; }
        [Required] public int NumberOfEcts { get; set; }
        [Required] public int Semester { get; set; }
        [Required] public int FieldId { get; set; }
        [Required] public string Name { get; set; } // trzeba dodać
        [ForeignKey("FieldId")] public virtual Field Field { get; set; }
        public virtual List<Course> Courses { get; set; }
    }
}