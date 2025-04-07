using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Exhibition
{
    public int ExhibitionId { get; set; }

    public string? Title { get; set; }

    public int Duration { get; set; }

    public int? ArtId { get; set; }

    public virtual Art? Art { get; set; }
}
