using System;
using System.Collections.Generic;

namespace Labb_två_Bokhandel.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();
}
