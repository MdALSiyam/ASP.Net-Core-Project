namespace Client.Models
{
    public class Exhibition
    {
        public int ExhibitionId { get; set; }

        public int ArtId { get; set; }

        public string Title { get; set; } = null!;

        public int Duration { get; set; }
        public virtual Art Art { get; set; } = null!;
    }
}
