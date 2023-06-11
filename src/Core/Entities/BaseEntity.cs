using System.Data;
using System.Runtime.InteropServices.JavaScript;

namespace Core.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public BaseEntity()
    {
        Created = DateTime.Now;
        Modified = DateTime.Now;
    }
}