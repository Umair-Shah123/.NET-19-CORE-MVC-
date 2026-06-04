using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="This Field is Required")]
        [MaxLength(100,ErrorMessage ="This Field is not exceed 100 character")]
        public string Username { get; set; }

        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [MaxLength(100, ErrorMessage = "This Field is not exceed 100 character")]
        public string Content {  get; set; }

        public int PostID { get; set; } 


    }
}
