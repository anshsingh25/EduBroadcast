using Microsoft.AspNetCore.Mvc;
using SchoolCommApp.Data;
using SchoolCommApp.Models;

namespace SchoolCommApp.Controllers
{
	public class AbsenceController : Controller
	{
		private readonly AppData _data;
		public AbsenceController()
		{
			var basePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
			_data = new AppData(basePath);
			SeedData.EnsureSeed(_data);
		}

		public IActionResult Index()
		{
			ViewBag.Classes = _data.Classes.GetAll();
			var logs = _data.Absences.GetAll().OrderByDescending(a => a.Date).ToList();
			return View(logs);
		}

		[HttpPost]
		public IActionResult Create([FromForm] string classId, [FromForm] DateTime date, [FromForm] string covered, [FromForm] string handouts, [FromForm] string homework)
		{
			if (string.IsNullOrWhiteSpace(classId))
			{
				TempData["Error"] = "Class is required.";
				return RedirectToAction(nameof(Index));
			}
			var log = new AbsenceLog
			{
				ClassId = classId,
				Date = DateOnly.FromDateTime(date == default ? DateTime.UtcNow : date),
				WhatWeCovered = covered ?? string.Empty,
				Handouts = handouts ?? string.Empty,
				Homework = homework ?? string.Empty
			};
			_data.Absences.Upsert(log, x => x.Id);
			TempData["Success"] = "Entry saved.";
			return RedirectToAction(nameof(Index));
		}

		[HttpGet("Absence/Class/{id}")]
		public IActionResult Class(string id)
		{
			var cls = _data.Classes.GetAll().FirstOrDefault(c => c.Id == id);
			if (cls == null) return NotFound();
			var logs = _data.Absences.GetAll().Where(a => a.ClassId == id).OrderByDescending(a => a.Date).ToList();
			ViewBag.Class = cls;
			return View("ClassPublic", logs);
		}
	}
}


