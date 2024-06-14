using System.ComponentModel.DataAnnotations;

namespace FakeShop.App.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
