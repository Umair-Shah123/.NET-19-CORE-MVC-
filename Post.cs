using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="This Field is Required")]
        [MaxLength(400,ErrorMessage ="The Title cannot exceed 400 characters")]
        public string Title { get; set; }

        
         [Required(ErrorMessage = "This Field is Required")]
          public  string Content { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [MaxLength(100, ErrorMessage = "The Title cannot exceed 100 characters")]
        public string Author { get; set; }

        public string? FeatureImagePath { get; set; }

       [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }= DateTime.Now;

        [ForeignKey("CategoryId")]

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        
        public Category? Categories { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}
