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
		_logger.LogInformation("Attempting to create a new student.");

		if (ModelState.IsValid)
		{
			var json = JsonConvert.SerializeObject(student);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			_logger.LogInformation($"Sending POST request to API with data: {json}");

			var response = await _httpClient.PostAsync("http://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students", content);

			if (response.IsSuccessStatusCode)
			{
				_logger.LogInformation("Successfully created student.");
				return RedirectToAction("Index");
			}
			else
			{
				_logger.LogError($"Failed to create student. HTTP Status: {response.StatusCode}");
				ModelState.AddModelError(string.Empty, "Error while creating student.");
				return View(student);
			}
		}

		_logger.LogError("Model state is invalid. Could not create student.");
		return View(student);
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
