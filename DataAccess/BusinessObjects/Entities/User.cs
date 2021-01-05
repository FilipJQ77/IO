using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.BusinessObjects.Entities
{
    public enum Rank
    {
        Administrator,
        Student,
        None,
    }

    public class User
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] [Index(IsUnique = true)] public string Login { get; set; }
        [Required] [MaxLength(255)] public string Password { get; set; }
        [Required] public Rank Rank { get; set; }
    }
}