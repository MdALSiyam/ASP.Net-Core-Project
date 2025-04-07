using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
{
    public class Common
    {
        public int ArtId { get; set; }

        public string ArtName { get; set; }

        public bool IsAvailable { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreationDate { get; set; }

        public string? ImageName { get; set; }

        public IFormFile ImgFile { get; set; }

        public string Exhibitions { get; set; }
    }
}
