using ChetuFinalProject.data;
using ChetuFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace ChetuFinalProject.Controllers
{
    [Authorize]
    public class TraineeController : Controller
    {
        private readonly ApplicationDatabContext _context;
        private readonly IWebHostEnvironment _webHost;

        public TraineeController(ApplicationDatabContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var res = _context.trainees.ToList();
            return View(res);

        }
        public IActionResult AddTrainee()
        {
            List<SelectListItem> item = new()
           {
               new SelectListItem{Value = "Rajasthan", Text = "Rajasthan"},
               new SelectListItem{Value = "Assam", Text = "Assam"},
               new SelectListItem{Value = "Arunachal Pradesh", Text = "Arunachal Pradesh"},
               new SelectListItem{Value = "Bihar", Text = "Bihar"},
               new SelectListItem{Value = "Bengal", Text = "Bengal"},
               new SelectListItem{Value = "Delhi", Text = "Delhi"},
               new SelectListItem{Value = "Punjab", Text = "Punjab"},
               new SelectListItem{Value = "Haryana", Text = "Haryana"},
               new SelectListItem{Value = "Jharkhand", Text = "Jharkhand"},
               new SelectListItem{Value = "Uttar Pradesh", Text = "Uttar Pradesh"}

           };
            ViewBag.State = item;
            return View();
        }
        [HttpPost]
        public IActionResult AddTrainee(Trainee n)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uploadFileName = UploadImg(n);
                    var data = new Trainee()
                    {
                        name = n.name,
                        gender = n.gender,
                        dob = n.dob,
                        Add = n.Add,
                        state = n.state,
                        path = uploadFileName
                    };
                    _context.trainees.Add(data);
                    _context.SaveChanges();
                    TempData["Success"] = "Record has been Successful";
                    return RedirectToAction("Index", "Trainee");
                }
                ModelState.AddModelError(string.Empty, "Model Propert is not valid");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(n);
        }

        private string UploadImg(Trainee n)
        {
            string uniqueFileName = " ";
            if (n.Image != null)
            {
                string uploadFolder = Path.Combine(_webHost.WebRootPath, "Images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + n.Image.FileName;
                string filepath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    n.Image.CopyTo(fileStream);
                }

            }
            return uniqueFileName;

        }

        [HttpGet]
        

        public IActionResult Edit(int id)
        {
            List<SelectListItem> item = new()
           {
               new SelectListItem{Value = "Rajasthan", Text = "Rajasthan"},
               new SelectListItem{Value = "Assam", Text = "Assam"},
               new SelectListItem{Value = "Arunachal Pradesh", Text = "Arunachal Pradesh"},
               new SelectListItem{Value = "Bihar", Text = "Bihar"},
               new SelectListItem{Value = "Bengal", Text = "Bengal"},
               new SelectListItem{Value = "Delhi", Text = "Delhi"},
               new SelectListItem{Value = "Punjab", Text = "Punjab"},
               new SelectListItem{Value = "Haryana", Text = "Haryana"},
               new SelectListItem{Value = "Jharkhand", Text = "Jharkhand"},
               new SelectListItem{Value = "Uttar Pradesh", Text = "Uttar Pradesh"}

           };
            ViewBag.State = item;
            if (id == 0 )
            {
                return NotFound();
            }
            var res = _context.trainees.Where(e => e.id == id).FirstOrDefault();
            if (res != null)
            {
                return View("Edit", res);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(Trainee n)
        {
            try
            {
                if (ModelState.IsValid)
                {
                
                    var result = _context.trainees.Where(x => x.id == n.id).SingleOrDefault();
                    if (result == null) { return NotFound(); }
                    string filename = string.Empty;
                    if (n.Image != null)
                    {
                        if (n.path != null)
                        {
                            string filepath = Path.Combine(_webHost.WebRootPath, "Images", result.path);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                        filename = UploadImg(n);
                    }
                    result.name = n.name;
                    result.gender = n.gender;
                    result.dob = n.dob;
                    result.Add = n.Add;
                    result.state = n.state;
                if (n.Image != null)
                {
                    result.path = filename;
                }
                    
                    _context.trainees.Update(result);
                    _context.SaveChanges();

                }
                else
                {
                    return View(n);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Index");
        }


        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var res = _context.trainees.FirstOrDefault(x => x.id == id);
            if (res == null) { return NotFound(); }
            return View(res);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || _context.trainees == null)
            {
                return BadRequest();
            }
            var res = (_context.trainees.FirstOrDefault(x => x.id == id));
            if (res == null) { return NotFound(); };
            return View(res);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = _context.trainees.Where(x => x.id == id).SingleOrDefault();
                
                if (data != null)
                {
                    string deletefile = Path.Combine(_webHost.WebRootPath, "Images");
                    string currImg = Path.Combine(Directory.GetCurrentDirectory(), deletefile, data.path);
                    if (currImg != null)
                    {
                        if (System.IO.File.Exists(currImg))
                        {
                            System.IO.File.Delete(currImg);
                        }
                    }
                    _context.trainees.Remove(data);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult GridView()
        {
            var res = _context.trainees.ToList();
            return View(res);
        }

    }
}
