using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class HomeFeature:BaseEntity
    {
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string IconClass { get; set; }
    }
}
