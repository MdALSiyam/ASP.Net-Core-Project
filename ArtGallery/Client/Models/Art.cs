using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class Art
    {
        public int ArtId { get; set; }

        public string ArtName { get; set; } = null!;

        public bool IsAvailable { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreationDate { get; set; }

        public string? ImageName { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<Exhibition> Exhibitions { get; set; } = new List<Exhibition>();
    }
}
