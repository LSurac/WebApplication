using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.SqlDataAccess.Contract.Entities
{
    [Table("applicantapplication")]
    public class ApplicantApplication
    {
        [Key][Column("apa_id")] public int Id { get; set; }
        [Column("apa_apl_id")] public int? ApplicationId { get; set; }
        [Column("apa_app_id")] public int? ApplicantId { get; set; }
        [Column("apa_state")] public string? State { get; set; }
        [Column("apa_first_edit_date")] public DateTime? FirstEditDate { get; set; }
        [Column("apa_last_edit_date")] public DateTime? LastEditDate { get; set; }
    }
}