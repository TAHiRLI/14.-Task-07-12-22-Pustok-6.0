using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class BtmPromotion
    {
        public int Id { get; set; }
        [MaxLength(80)]
        public string Title1 { get; set; }
        [MaxLength(80)]
        public string Title2 { get; set; }
        [MaxLength(80)]
        public string Title3 { get; set; }

        [MaxLength(80)]
        public string BtnText { get; set; }
        [MaxLength(100)]
        public string RedirectedUrl { get; set; }
    }
}
