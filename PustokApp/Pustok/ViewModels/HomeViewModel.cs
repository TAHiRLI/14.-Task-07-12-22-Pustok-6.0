using Pustok.Models;
using System.Collections.Generic;

namespace Pustok.ViewModels
{
    public class HomeViewModel
    {
        public List <Book> SpecialBooks{ get; set; }
        public List<Book> NewBooks { get; set; }    
        public List <Book> DiscountedBooks { get; set; }
        public List<Slider> HeroSlider { get; set; }
        public List<HomeFeature> HomeFeatures { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
