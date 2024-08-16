using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.SqlDataAccess.Contract.Entities
{
    [Table("application")]
    public class Application
    {
        [Key][Column("apl_id")] public int Id { get; set; }
        [Column("apl_description")] public string? Description { get; set; }
        [Column("apl_first_edit_date")] public DateTime? FirstEditDate { get; set; }
        [Column("apl_last_edit_date")] public DateTime? LastEditDate { get; set; }
    }
}
