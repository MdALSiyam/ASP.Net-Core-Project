using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.DTOs;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        private readonly IWebHostEnvironment _web;
        private readonly CoreArtDbContext _db;

        public ArtsController(IWebHostEnvironment web, CoreArtDbContext db)
        {
            _web = web;
            _db = db;
        }
        [HttpGet]
        public IActionResult GetArts()
        {
            List<Art> arts = _db.Arts.Include(e => e.Exhibitions).ToList();
            string jsonstring = JsonConvert.SerializeObject(arts, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            return Content(jsonstring, "application/json");
        }
        [HttpGet("{id}")]
        public IActionResult GetArt(int id)
        {
            Art art = _db.Arts.Include(e => e.Exhibitions).SingleOrDefault(e => e.ArtId == id);
            if (id == null)
            {
                return NotFound();
            }
            string jsonstring = JsonConvert.SerializeObject(art, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            return Content(jsonstring, "application/json");
        }
        [HttpPost]
        public async Task<IActionResult> PostArt([FromForm] Common objCommon)
        {
            ImgUpload fileApi = new ImgUpload();
            //string fileName = objCommon.ImageName + ".jpg";
            string fileName = objCommon.ImageName;
            fileApi.ImgName = "\\images\\" + fileName;
            if (objCommon.ImgFile?.Length > 0)
            {
                if (!Directory.Exists(_web.WebRootPath + "\\images"))
                {
                    Directory.CreateDirectory(_web.WebRootPath + "\\images\\");
                }
                string filePath = _web.WebRootPath + "\\images\\" + fileName;
                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    objCommon.ImgFile.CopyTo(fileStream);
                    fileStream.Flush();
                }
                fileApi.ImgName = "/images/" + fileName;
            }
            Art artObj = new Art();
            artObj.ArtName = objCommon.ArtName;
            artObj.IsAvailable = objCommon.IsAvailable;
            artObj.CreationDate = objCommon.CreationDate;
            artObj.ImageName = objCommon.ImageName;
            artObj.ImageUrl = fileApi.ImgName;
            _db.Arts.Add(artObj);
            await _db.SaveChangesAsync();
            List<Exhibition> list = JsonConvert.DeserializeObject<List<Exhibition>>(objCommon.Exhibitions);
            var art = _db.Arts.FirstOrDefault(x => x.ArtName == artObj.ArtName);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    Exhibition exobj = new Exhibition
                    {
                        ArtId = art.ArtId,
                        Title = item.Title,
                        Duration = item.Duration,
                    };
                    _db.Exhibitions.Add(exobj);
                }
                await _db.SaveChangesAsync();
            }
            return Ok(artObj);

        }
        [HttpPut()]
        public async Task<IActionResult> UpdateArt(int id, [FromForm] Common objCommon)
        {
            var artObj = await _db.Arts.FindAsync(id);
            if (artObj == null) return NotFound("Art Not Found");
            ImgUpload fileApi = new ImgUpload();
            string fileName = objCommon.ImageName + ".jpg";
            fileApi.ImgName = "\\images\\" + fileName;
            if (objCommon.ImgFile?.Length > 0)
            {
                if (!Directory.Exists(_web.WebRootPath + "\\images"))
                {
                    Directory.CreateDirectory(_web.WebRootPath + "\\images\\");
                }
                string filePath = _web.WebRootPath + "\\images\\" + fileName;
                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    objCommon.ImgFile.CopyTo(fileStream);
                    fileStream.Flush();
                }
                fileApi.ImgName = "/images/" + fileName;
            }
            artObj.ArtName = objCommon.ArtName;
            artObj.IsAvailable = objCommon.IsAvailable;
            artObj.CreationDate = objCommon.CreationDate;
            artObj.ImageName = objCommon.ImageName;
            artObj.ImageUrl = fileApi.ImgName;
            List<Exhibition> list = JsonConvert.DeserializeObject<List<Exhibition>>(objCommon.Exhibitions);
            var existingExhibitions = _db.Exhibitions.Where(e => e.ArtId == id);
            _db.Exhibitions.RemoveRange(existingExhibitions);
            if (list!.Any())
            {
                foreach (var item in list)
                {
                    Exhibition exObj = new Exhibition
                    {
                        ArtId = artObj.ArtId,
                        Title = item.Title,
                        Duration = item.Duration,
                    };
                    _db.Exhibitions.Add(exObj);
                }
                await _db.SaveChangesAsync();

            }
            return Ok("Art Updated Successfully");
        }
        [HttpDelete()]
        public async Task<IActionResult> DeleteArt(int id)
        {
            var artObj = await _db.Arts.FindAsync(id);
            if (artObj == null) return NotFound("Employee not found");
            _db.Arts.Remove(artObj);
            await _db.SaveChangesAsync();
            return Ok("Art Deleted Successfully");
        }

    }
}
