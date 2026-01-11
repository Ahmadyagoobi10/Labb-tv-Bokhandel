using System;
using System.Collections.Generic;

namespace Labb_två_Bokhandel.Models;

public partial class VwBooksPerStorePublisher
{
    public string StoreName { get; set; } = null!;

    public string BookTitle { get; set; } = null!;

    public string? PublisherName { get; set; }

    public int Quantity { get; set; }

    public decimal? TotalValue { get; set; }
}
