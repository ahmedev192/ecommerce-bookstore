using System.ComponentModel.DataAnnotations;

namespace ecommerce_bookstore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Display Order field is required.")]
        [Range(1,100)]
        public int DisplayOrder { get; set; }
    }
}
