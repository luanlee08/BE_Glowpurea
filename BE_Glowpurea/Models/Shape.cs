using System;
using System.Collections.Generic;

namespace BE_Glowpurea.Models;

public partial class Shape
{
    public int ShapesId { get; set; }

    public string ShapesName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
