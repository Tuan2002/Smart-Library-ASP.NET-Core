using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    [Table("Workspaces")]
    public class Workspace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkspaceId { get; set; }
        public string? WorkspaceName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}