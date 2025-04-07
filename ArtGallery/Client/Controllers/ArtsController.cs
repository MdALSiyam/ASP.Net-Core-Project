using Client.Models;
using Client.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ArtsController : Controller
    {
        private string apiUrl = "http://localhost:5059/api/Arts";
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Art> artList = new List<Art>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    artList = JsonConvert.DeserializeObject<List<Art>>(apiResponse);
                }
            }
            return View(artList);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Art art = new Art();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{apiUrl}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    art = JsonConvert.DeserializeObject<Art>(apiResponse);
                }
            }
            return View(art);
        }
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://localhost:5059/api/Arts?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ArtViewModel art = new ArtViewModel();
            return View(art);
        }
        [HttpPost]

        public async Task<IActionResult> Create(ArtViewModel art)

        {
            Common obj = new Common();
            obj.ArtId = art.ArtId;
            obj.ArtName = art.ArtName;
            obj.IsAvailable = art.IsAvailable;
            obj.CreationDate = art.CreationDate;
            obj.ImgFile = art.ProfileFile;
            obj.Exhibitions = JsonConvert.SerializeObject(art.Exhibitions, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            using var content = new MultipartFormDataContent();

            // Add string properties
            content.Add(new StringContent(obj.ArtId.ToString()), "ArtId");
            content.Add(new StringContent(obj.ArtName), "ArtName");
            content.Add(new StringContent(obj.IsAvailable.ToString()), "IsAvailable");
            content.Add(new StringContent(obj.CreationDate.ToString("yyyy-MM-dd")), "CreationDate");

            content.Add(new StringContent(obj.Exhibitions), "Exhibitions");

            // Handle IFormFile
            if (obj.ImgFile != null && obj.ImgFile.Length > 0)
            {
                content.Add(new StreamContent(obj.ImgFile.OpenReadStream()), "ImgFile", obj.ImgFile.FileName); // Use file name
                content.Add(new StringContent(obj.ImgFile.FileName), "ImageName");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(apiUrl, content); // API URL

                    if (response.IsSuccessStatusCode)
                    {
                        //var result = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("Index"); // Or RedirectToAction, etc.
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode); // Return error with details
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"http://localhost:5059/api/Arts/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var art = JsonConvert.DeserializeObject<Art>(apiResponse);

                        if (art != null)
                        {
                            var artViewModel = new ArtViewModel
                            {
                                ArtId = art.ArtId,
                                ArtName = art.ArtName,
                                IsAvailable = art.IsAvailable,
                                CreationDate = art.CreationDate,
                                ImageUrl = art.ImageUrl,
                                Exhibitions = art.Exhibitions.ToList()
                            };
                            return View(artViewModel);
                        }

                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode);
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ArtViewModel art)
        {
            Common obj = new Common();

            obj.ArtId = art.ArtId;
            obj.ArtName = art.ArtName;
            obj.IsAvailable = art.IsAvailable;
            obj.CreationDate = art.CreationDate;

            obj.ImgFile = art.ProfileFile;
            obj.Exhibitions = JsonConvert.SerializeObject(art.Exhibitions, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            using var content = new MultipartFormDataContent();

            // Add string properties
            content.Add(new StringContent(obj.ArtId.ToString()), "ArtId");
            content.Add(new StringContent(obj.ArtName), "ArtName");
            content.Add(new StringContent(obj.IsAvailable.ToString()), "IsAvailable");
            content.Add(new StringContent(obj.CreationDate.ToString("yyyy-MM-dd")), "CreationDate");

            content.Add(new StringContent(obj.Exhibitions), "Exhibitions");

            // Handle IFormFile
            if (obj.ImgFile != null && obj.ImgFile.Length > 0)
            {
                content.Add(new StreamContent(obj.ImgFile.OpenReadStream()), "ImgFile", obj.ImgFile.FileName); // Use file name
                content.Add(new StringContent(obj.ImgFile.FileName), "ImageName");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync($"http://localhost:5059/api/Arts?id={art.ArtId}", content); // API URL

                    if (response.IsSuccessStatusCode)
                    {
                        //var result = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, errorContent); // Return error with details
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Handle exceptions
            }
        }
    }
}
