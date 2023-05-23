using System;
using System.Collections.Generic;

namespace BackWI.Models;

public partial class AnimalsType
{
    public Guid IdType { get; set; }

    public int NumId { get; set; }

    public string NameType { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
