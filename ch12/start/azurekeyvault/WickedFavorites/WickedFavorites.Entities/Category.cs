﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Wicked.Favorites
{
    public partial class Category
    {
        public Category()
        {
            Favorites = new HashSet<Favorite>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}