﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackWI.Models
{
    public partial class AnimalTypes
    {
        public AnimalTypes()
        {
            Animals = new HashSet<Animals>();
        }

        public Guid IdType { get; set; }
        public int NumId { get; set; }
        public string NameType { get; set; }

        public virtual ICollection<Animals> Animals { get; set; }
    }
}