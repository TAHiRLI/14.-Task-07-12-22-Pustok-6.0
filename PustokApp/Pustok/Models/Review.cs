using System;
using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        [MaxLength(200)]
        public string Text { get; set; }
        public byte Rate { get; set; }
        public DateTime Date { get; set; }

        public Book Book { get; set; }

        public User User { get; set; }
    }
}
