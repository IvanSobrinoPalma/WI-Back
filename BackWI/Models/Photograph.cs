using System;
using System.Collections.Generic;

namespace BackWI.Models;

public partial class Photograph
{
    public Guid IdUser { get; set; }

    public Guid IdAnimal { get; set; }

    public int NumId { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual Animal IdAnimalNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
