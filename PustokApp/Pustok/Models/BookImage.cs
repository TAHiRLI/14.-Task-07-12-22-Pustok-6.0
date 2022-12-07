using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class BookImage
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        public bool? PosterStatus { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
