using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Books.Models;

public partial class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? Year { get; set; }
    public string? Publisher { get; set; }
    public string? Description { get; set; }
}
