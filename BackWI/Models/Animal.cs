using System;
using System.Collections.Generic;

namespace BackWI.Models;

public partial class Animal
{
    public Guid IdAnimal { get; set; }

    public int IdNum { get; set; }

    public string NameAnimal { get; set; }

    public string ScientificName { get; set; } = null!;

    public int Dangerousness { get; set; }

    public string Type { get; set; }

    public int DangerOfExtinction { get; set; } 

    public byte[] Image { get; set; } = null!;

    public virtual AnimalsType TypeAnimal { get; set; } = null!;
}
