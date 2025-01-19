namespace StudentWebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentWebApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

public class StudentController(HttpClient httpClient) : Controller
{
	private readonly HttpClient _httpClient = httpClient;

    // GET: /Student/Index
    public async Task<IActionResult> Index()
	{
		var response = await _httpClient.GetStringAsync("https://studentapi-app.yellowpond-a7234cf9.northeurope.azurecontainerapps.io/api/students");
		var students = JsonConvert.DeserializeObject<List<Student>>(response);

		return View(students);
	}
	
	// GET: /Student/Create
	[HttpGet]
	public IActionResult Create()
	{
		return View();
	}

	// POST: /Student/Create
	[HttpPost]
	public async Task<IActionResult> Create(Student student)
	{

		if (ModelState.IsValid)
		{
			var json = JsonConvert.SerializeObject(student);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("https://studentapi-app.yellowpond-a7234cf9.northeurope.azurecontainerapps.io/api/students/", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(student);
			}	
		}

		return View(student);
	}
	
	// GET: /Student/Edit/{id}
	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var response = await _httpClient.GetAsync($"https://studentapi-app.yellowpond-a7234cf9.northeurope.azurecontainerapps.io/api/students/{id}");
		if (response.IsSuccessStatusCode)
		{
			var studentJson = await response.Content.ReadAsStringAsync();
			var student = JsonConvert.DeserializeObject<Student>(studentJson);
			return View(student);
		}

		return RedirectToAction("Index");
	}

	// POST: /Student/Edit/{id}
	[HttpPost]
	public async Task<IActionResult> Edit(int id, Student student)
	{
		if (id != student.Id)
		{
			return BadRequest("ID mismatch.");
		}

		if (ModelState.IsValid)
		{
			var jsonContent = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync($"https://studentapi-app.yellowpond-a7234cf9.northeurope.azurecontainerapps.io/api/students/{id}", jsonContent);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
		}

		return View(student);
	}
	
	// POST: /Student/Delete/{id}
	[HttpPost]
	public async Task<IActionResult> DeleteStudent(int id)
	{
		var response = await _httpClient.DeleteAsync($"https://studentapi-app.yellowpond-a7234cf9.northeurope.azurecontainerapps.io/api/students/{id}");

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index");
		}

		return RedirectToAction("Index");
	}
}
