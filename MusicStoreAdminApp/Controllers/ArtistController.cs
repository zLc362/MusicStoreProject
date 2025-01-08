using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using MusicStoreAdminApp.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace MusicStoreAdminApp.Controllers
{
    public class ArtistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportArtists(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream); ;
                fileStream.Flush();
            }

            List<Artist> artists = getAllArtistsFromFile(file.FileName);

            HttpClient client = new HttpClient();

            string URL = "https://localhost:44337/api/Admin/ImportAllArtists";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(artists), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<bool>().Result;
            return RedirectToAction("Index", "Artist");

        }

        private List<Artist> getAllArtistsFromFile(string fileName)
        {
            List<Artist> artists = new List<Artist>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {


                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        artists.Add(new Artist
                        {
                            Name = reader.GetValue(0).ToString(),
                            Biography = reader.GetValue(1).ToString(),
                            Image = reader.GetValue(2).ToString()
                        });

                    }
                }
            }
            return artists;
        }
    }
}
