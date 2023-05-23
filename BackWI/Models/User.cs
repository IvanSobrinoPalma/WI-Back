using System;
using System.Collections.Generic;

namespace BackWI.Models;

public partial class User
{
    public Guid IdUser { get; set; }

    public int IdNum { get; set; }

    public string Nick { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NameUser { get; set; } = null!;

    public string FirtsSurname { get; set; } = null!;

    public string SecondSurname { get; set; } = null!;
}
