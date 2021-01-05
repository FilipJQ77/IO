using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.BusinessObjects.Entities
{
    public class StudentData
    {
        [Key] public int Id { get; set; }
        [Required] public int Index { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public int Semester { get; set; }
        [Column(TypeName = "datetime2")] public DateTime? RegistrationDate { get; set; }
        [Required] public int FieldId { get; set; }
        [ForeignKey("FieldId")] public virtual Field Field { get; set; }
        [Required] public int UserId { get; set; }
        [ForeignKey("UserId")] public virtual User User { get; set; }
        public virtual List<Lesson> Lessons { get; set; }
    }
}