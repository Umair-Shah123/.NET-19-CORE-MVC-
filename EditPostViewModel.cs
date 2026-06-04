using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.Models.ViewModels
{
    public class EditPostViewModel
    {

        public Post Post { get; set; }

        //SelectListitem is a class Which is use to make a dropdown
        [ValidateNever]
        public IEnumerable<SelectListItem>? Categories { get; set; }
        [ValidateNever]
        public IFormFile FeatureImage { get; set; }




    }
}
