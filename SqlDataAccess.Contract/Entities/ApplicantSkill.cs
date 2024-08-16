using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.SqlDataAccess.Contract.Entities
{
    [Table("applicantskill")]
    public class ApplicantSkill
    {
        [Key][Column("ask_id")] public int Id { get; set; }
        [Column("ask_app_id")] public int ApplicantId { get; set; }
        [Column("ask_skl_id")] public int SkillId { get; set; }
        [Column("ask_first_edit_date")] public DateTime? FirstEditDate { get; set; }
        [Column("ask_last_edit_date")] public DateTime? LastEditDate { get; set; }
    }
}