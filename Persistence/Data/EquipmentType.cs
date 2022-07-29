using System.ComponentModel.DataAnnotations;

namespace Persistence.Data;

public class EquipmentType
{
    [Key]
    public string Type { get; set; }
}