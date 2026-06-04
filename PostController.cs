using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Net;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

//<--- Include() use to load related data--->
// <--- Adsync() use to insert Data--->
namespace WebApplication2.Controllers
{


    public class PostController : Controller
    {
        //<----Constructor Injuction Dependencies---->
        private readonly AppDbcontext _context;
        //<--- _webHostEnvironment Gives access to server folders like wwwroot--->

        private readonly IWebHostEnvironment _webHostEnvironment;
        //<----Constructor Injuction Dependencies---->
        private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png" };

        public PostController(AppDbcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]

        public IActionResult Index(int? categoryId)
        {
            //<--- fetch and filter data according to cayegory--->
            var postQuery = _context.Posts.Include(p => p.Categories).AsQueryable();
            if (categoryId.HasValue)
            {
                postQuery = postQuery.Where(p => p.CategoryId == categoryId);
            }
            var posts = postQuery.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(posts);
        }
        //<--- fetch and filter data according to cayegory--->
        [HttpGet]
        //<--- Detail controller--->
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = _context.Posts.Include(p => p.Categories).Include(p => p.Comments)
            .FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        //<--- Detail controller(Details.cshtml)--->


        //<---- create controller(Create.cshtml)--->
        public IActionResult Create()
        {

            var postviewmodel = new PostViewModel();
            postviewmodel.Categories = _context.Categories.Select(c =>
            new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }

            ).ToList();
            return View(postviewmodel);
        }
        //<---- create controller--->
        [HttpPost]

        // Validates if the uploaded file's extension is included in the allowed file types list.
        //<--- File uploading--->
        public async Task<IActionResult> Create(PostViewModel postViewModel)

        {
            if (ModelState.IsValid)
            {
                var inputFileExtension = Path.GetExtension(postViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = _allowedExtension.Contains(inputFileExtension);


                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid Image Format.Allowed Format are .jpg,.jpeg,.png ");
                    return View(postViewModel);
                }
                postViewModel.Post.FeatureImagePath = await UploadFiletoFOlder(postViewModel.FeatureImage);
                await _context.Posts.AddAsync(postViewModel.Post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            postViewModel.Categories = _context.Categories.Select(c =>
            new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }

            ).ToList();
            return View(postViewModel);
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)

        {
            if (id == null)
            {
                return NotFound();
            }
            var postfromdb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (postfromdb == null)
            {
                return NotFound();

            }
            EditPostViewModel editPostViewModel = new EditPostViewModel
            {
                Post = postfromdb,
                Categories = _context.Categories.Select(c =>
            new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }

            ).ToList()
            };
            return View(editPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel)
        {
            if (!ModelState.IsValid)
            {

                return View(editPostViewModel);
            }
            var postfromDb = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editPostViewModel.Post.Id);
            if (postfromDb == null)
            {
                return NotFound();
            }
            if (editPostViewModel.FeatureImage != null)
            {

                var inputFileExtension = Path.GetExtension(editPostViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = _allowedExtension.Contains(inputFileExtension);


                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid Image Format.Allowed Format are .jpg,.jpeg,.png ");
                    return View(editPostViewModel);
                }
                var existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", Path.GetFileName(postfromDb.FeatureImagePath));
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);

                }
                editPostViewModel.Post.FeatureImagePath = await UploadFiletoFOlder(editPostViewModel.FeatureImage);
            }
            else
            {
                editPostViewModel.Post.FeatureImagePath = postfromDb.FeatureImagePath;
            }
            _context.Posts.Update(editPostViewModel.Post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult>Delete(int id)
        {
            var postfromDb= await _context.Posts.FirstOrDefaultAsync(p=> p.Id == id);
            if(postfromDb == null)
            {
                return NotFound();
            }
            return View(postfromDb);
        }

        public async Task<IActionResult>DeleteConfrim(int id)
        {
            var postfromdb=await _context.Posts.FirstOrDefaultAsync(p=>p.Id== id);
            if(string.IsNullOrEmpty(postfromdb.FeatureImagePath))
            {
                var existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", Path.GetFileName(postfromdb.FeatureImagePath));
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);

                }
                
        }
            _context.Posts.Remove(postfromdb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //<--- comment controller--->
        public JsonResult AddComment([FromBody] Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return Json(new
            {
                username = comment.Username,
                commentDate = comment.CommentDate.ToString("MMM dd, yyyy"),
                content = comment.Content
            });


        }
        //<--- make root for file in folder--->
        private async Task<string> UploadFiletoFOlder(IFormFile file)
        {
            var inputFileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + inputFileExtension;
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var imagesFolderPath = Path.Combine(wwwRootPath, "images");

            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            var filePath = Path.Combine(imagesFolderPath, fileName);
            try

            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                return "Error message" + ex.Message;
            }
            return "/images/" + fileName;
        }
    }
}