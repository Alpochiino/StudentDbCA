namespace StudentWebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentWebApp.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

public class StudentController : Controller
{
	private readonly HttpClient _httpClient;

	public StudentController(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IActionResult> Index()
	{
		var response = await _httpClient.GetStringAsync("https://studentsdbca.database.windows.net/api/students");
        var students = JsonConvert.DeserializeObject<List<Student>>(response);

		return View(students);
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
