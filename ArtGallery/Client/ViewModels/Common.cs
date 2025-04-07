using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    public class Common
    {
        public int ArtId { get; set; }

        public string ArtName { get; set; }

        public bool IsAvailable { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreationDate { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile ImgFile { get; set; }

        public string Exhibitions { get; set; }
    }
}
