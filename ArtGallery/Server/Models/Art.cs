using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Art
{
    public int ArtId { get; set; }

    public string ArtName { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public DateTime CreationDate { get; set; }

    public string? ImageName { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Exhibition> Exhibitions { get; set; } = new List<Exhibition>();
}
