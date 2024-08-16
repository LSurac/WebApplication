using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.SqlDataAccess.Contract.Entities
{
    [Table("skill")]
    public class Skill
    {
        [Key][Column("skl_id")] public int Id { get; set; }
        [Column("skl_description")] public string? Description { get; set; }
        [Column("skl_first_edit_date")] public DateTime? FirstEditDate { get; set; }
        [Column("skl_last_edit_date")] public DateTime? LastEditDate { get; set; }
    }
}