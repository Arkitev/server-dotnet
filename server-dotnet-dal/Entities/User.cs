using server_dotnet_dal.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server_dotnet_dal.Entities;

public class User : IEntity
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    public DateTime DateCreated { get; set; }

    [Required]
    public int OrganizationId { get; set; }
    [ForeignKey("OrganizationId")]
    public Organization Organization { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = [];
}
