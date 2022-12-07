using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class TopPromotion
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Image { get; set; }
        [MaxLength(100)]
        public string RedirectedUrl { get; set; }
    }
}
