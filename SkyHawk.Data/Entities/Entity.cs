using System.ComponentModel.DataAnnotations;

namespace SkyHawk.Data.Entities;

public class Entity
{
    [Key]
    public int Id { get; set; }
}
