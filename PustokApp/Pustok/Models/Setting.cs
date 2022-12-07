using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Key { get; set; }
        [MaxLength(200)]
        public string Value { get; set; }
    }
}
