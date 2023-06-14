using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices.JavaScript;

namespace Core.Entities;

public class BaseEntity
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public DateTime Modified { get; set; }

    public BaseEntity()
    {
        Created = DateTime.Now;
        Modified = DateTime.Now;
    }
}