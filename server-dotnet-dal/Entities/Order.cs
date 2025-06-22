using server_dotnet_dal.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server_dotnet_dal.Entities;

public class Order : IEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than 0")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    public int? OrganizationId { get; set; }
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }
}
