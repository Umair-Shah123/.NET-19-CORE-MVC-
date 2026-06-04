using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="This Feild is Required")]
        [MaxLength(100,ErrorMessage ="The Name Field is not exceed 100 character")]
        public string Name { get; set; } 

        //? for acceptance of Null values
        public string? Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
