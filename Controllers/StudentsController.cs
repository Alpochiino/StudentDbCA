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

	public StudentController(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IActionResult> Index()
	{
		var response = await _httpClient.GetStringAsync("https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/api/students");
		var students = JsonConvert.DeserializeObject<List<Student>>(response);

		return View(students);
	}
	
/* 	[HttpGet]
	public IActionResult Create()
	{
		return View();
	} */

	[HttpPost]
	public async Task<IActionResult> Create(Student student)
	{
		if (ModelState.IsValid)
		{
			var json = JsonConvert.SerializeObject(student);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("api/students", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
		}

		return View(student);
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
