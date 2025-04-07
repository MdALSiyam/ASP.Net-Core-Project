using Client.Models;
using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    public class ArtViewModel
    {
        public int ArtId { get; set; }
        [Required]
        [Display(Name = "Art Name")]
        public string ArtName { get; set; } = null!;
        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string? ImageName { get; set; }

        public string? ImageUrl { get; set; }

        public int ExhibitionId { get; set; }

        public string Title { get; set; } = null!;

        public int Duration { get; set; }

        public IFormFile ProfileFile { get; set; }
        public List<Exhibition> Exhibitions { get; set; } = new List<Exhibition>();
    }
}
