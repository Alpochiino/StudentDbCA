namespace StudentWebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentWebApp.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

public class StudentController : Controller
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<StudentController> _logger;

	public StudentController(HttpClient httpClient, ILogger<StudentController> logger)
	{
		_httpClient = httpClient;
		_logger = logger;
	}

	// GET: /Student/Index
	public async Task<IActionResult> Index()
	{
		var response = await _httpClient.GetStringAsync("https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students");
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

			var response = await _httpClient.PostAsync("https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students/", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Error while creating student.");
				return View(student);
			}	
		}

		return View(student);
	}
	
	// GET: /Student/Edit/{id}
	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var response = await _httpClient.GetAsync($"https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students/{id}");
		if (response.IsSuccessStatusCode)
		{
			var studentJson = await response.Content.ReadAsStringAsync();
			var student = JsonConvert.DeserializeObject<Student>(studentJson);
			return View(student);
		}

		_logger.LogError($"Failed to load student with ID {id}. Status: {response.StatusCode}");
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
			var response = await _httpClient.PutAsync($"https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students/{id}", jsonContent);

			if (response.IsSuccessStatusCode)
			{
				_logger.LogInformation($"Successfully updated student with ID {student.Id}.");
				return RedirectToAction("Index");
			}

			_logger.LogError($"Failed to update student with ID {student.Id}. Status: {response.StatusCode}");
			ModelState.AddModelError(string.Empty, "Error while updating student.");
		}

		return View(student);
	}
	
	// POST: /Student/Delete/{id}
	[HttpPost]
	public async Task<IActionResult> DeleteStudent(int id)
	{
		var response = await _httpClient.DeleteAsync($"https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students/{id}");

		if (response.IsSuccessStatusCode)
		{
			_logger.LogInformation($"Successfully deleted student with ID {id}.");
			return RedirectToAction("Index");
		}

		_logger.LogError($"Failed to delete student with ID {id}. Status: {response.StatusCode}");
		ModelState.AddModelError(string.Empty, "Error while deleting student.");
		return RedirectToAction("Index");
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
