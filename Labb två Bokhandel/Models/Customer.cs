using System;
using System.Collections.Generic;

namespace Labb_två_Bokhandel.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhOne { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
