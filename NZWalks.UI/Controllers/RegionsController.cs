using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public IHttpClientFactory HttpClientFactory { get; }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionsDto> response = new List<RegionsDto>();
            try
            {
                var client = HttpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7210/api/Regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionsDto>>());
            }
            catch(Exception ex)
            {

            }
            return View(response);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = HttpClientFactory.CreateClient();
            var httpRequestMEssage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7210/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
           var httpResponseMessage= await client.SendAsync(httpRequestMEssage);
            httpResponseMessage.EnsureSuccessStatusCode();
           var response= await httpResponseMessage.Content.ReadFromJsonAsync<RegionsDto>();
            if(response !=null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = HttpClientFactory.CreateClient();
            var httpResponseMessage=await client.GetFromJsonAsync<RegionsDto>($"https://localhost:7210/api/Regions/{id.ToString()}");
            if(httpResponseMessage != null)
            {
                return View(httpResponseMessage);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RegionsDto request)
        {
            var client = HttpClientFactory.CreateClient();
            var httpRequestMEssage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7210/api/Regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMEssage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpRequestMEssage.Content.ReadFromJsonAsync<RegionsDto>();
            if(response != null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RegionsDto request)
        {
            try
            {

                var client = HttpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7210/api/Regions/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Regions");
            }
            catch(Exception ex)
            {

            }
            return View("Edit");
        }
    }
}
