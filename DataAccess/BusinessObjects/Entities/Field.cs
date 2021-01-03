using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.BusinessObjects.Entities
{
    public class Field
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public virtual List<CourseGroup> CourseGroups { get; set; }
        public virtual List<StudentData> StudentDatas { get; set; }
    }
}