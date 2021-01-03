using System.ComponentModel.DataAnnotations;

namespace DataAccess.BusinessObjects.Entities
{
    public enum Rank
    {
        Administrator,
        Student
    }

    public class User
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] public string Login { get; set; }
        [Required] [MaxLength(255)] public string Password { get; set; }
        [Required] public Rank Rank { get; set; }
    }
}