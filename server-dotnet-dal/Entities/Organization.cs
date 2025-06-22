using server_dotnet_dal.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace server_dotnet_dal.Entities;

public class Organization : IEntity
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
    public string? Industry { get; set; }
    public DateTime DateFounded { get; set; }

    public ICollection<User> Users { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
