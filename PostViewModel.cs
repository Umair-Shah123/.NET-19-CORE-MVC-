using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }

        //SelectListitem is a class Which is use to make a dropdown
       [ValidateNever]
        public IEnumerable<SelectListItem>? Categories { get; set; }

        public IFormFile FeatureImage { get; set; }


    }
}
