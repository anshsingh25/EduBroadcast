using Microsoft.AspNetCore.Mvc;
using SchoolCommApp.Data;
using SchoolCommApp.Models;

namespace SchoolCommApp.Controllers
{
	public class SchedulerController : Controller
	{
		private readonly AppData _data;
		public SchedulerController()
		{
			var basePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
			_data = new AppData(basePath);
			SeedData.EnsureSeed(_data);
		}

		public IActionResult Index()
		{
			var upcoming = _data.Slots.GetAll().OrderBy(s => s.StartUtc).ToList();
			return View(upcoming);
		}

		public IActionResult Admin()
		{
			var slots = _data.Slots.GetAll().OrderBy(s => s.StartUtc).ToList();
			return View(slots);
		}

		[HttpPost]
		public IActionResult AddSlot([FromForm] DateTime start, [FromForm] int minutes)
		{
			if (start == default || minutes <= 0)
			{
				TempData["Error"] = "Start and duration required.";
				return RedirectToAction(nameof(Admin));
			}
			var slot = new ConferenceSlot
			{
				StartUtc = new DateTimeOffset(DateTime.SpecifyKind(start, DateTimeKind.Local)).ToUniversalTime(),
				EndUtc = new DateTimeOffset(DateTime.SpecifyKind(start, DateTimeKind.Local)).AddMinutes(minutes).ToUniversalTime()
			};
			_data.Slots.Upsert(slot, x => x.Id);
			TempData["Success"] = "Slot added.";
			return RedirectToAction(nameof(Admin));
		}

		[HttpPost]
		public IActionResult DeleteSlot([FromForm] string id)
		{
			_data.Slots.Delete(id, x => x.Id);
			TempData["Success"] = "Slot deleted.";
			return RedirectToAction(nameof(Admin));
		}

		public IActionResult Book(string id)
		{
			var slot = _data.Slots.GetAll().FirstOrDefault(s => s.Id == id);
			if (slot == null || slot.IsBooked) return NotFound();
			return View(slot);
		}

		[HttpPost]
		public IActionResult Book([FromForm] string slotId, [FromForm] string parentName, [FromForm] string parentEmail, [FromForm] string studentName)
		{
			var slot = _data.Slots.GetAll().FirstOrDefault(s => s.Id == slotId);
			if (slot == null) return NotFound();
			if (slot.IsBooked)
			{
				TempData["Error"] = "Slot already booked.";
				return RedirectToAction(nameof(Index));
			}
			slot.IsBooked = true;
			_data.Slots.Upsert(slot, x => x.Id);
			_data.Bookings.Upsert(new Booking { SlotId = slotId, ParentName = parentName, ParentEmail = parentEmail, StudentName = studentName }, x => x.Id);
			TempData["Success"] = "Booking confirmed.";
			return RedirectToAction(nameof(Index));
		}
	}
}


