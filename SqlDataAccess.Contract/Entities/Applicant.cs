using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.SqlDataAccess.Contract.Entities
{
    [Table("applicant")]
    public class Applicant
    {
        [Key][Column("app_id")] public int Id { get; set; }
        [Column("app_firstname")] public string? FirstName { get; set; }
        [Column("app_lastname")] public string? LastName { get; set; }
        [Column("app_birthdate")] public DateOnly BirthDate { get; set; }
        [Column("app_first_edit_date")] public DateTime? FirstEditDate { get; set; }
        [Column("app_last_edit_date")] public DateTime? LastEditDate { get; set; }
    }
}