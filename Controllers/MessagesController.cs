using Microsoft.AspNetCore.Mvc;
using EduBroadcast.Data;
using EduBroadcast.Models;
using EduBroadcast.Services;

namespace EduBroadcast.Controllers
{
        public class MessagesController : Controller
        {
                private readonly AppData _data;
                private readonly ISmsSender _sms;
                private readonly IEmailSender _email;

                public MessagesController()
                {
                        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
                        _data = new AppData(basePath);
                        SeedData.EnsureSeed(_data);
                        _sms = new FileLogSmsSender(Path.Combine(basePath, "outbox.log"));
                        _email = new FileLogEmailSender(Path.Combine(basePath, "outbox.log"));
                }

                public IActionResult Templates()
                {
                        return View(_data.Templates.GetAll());
                }

                [HttpPost]
                public IActionResult AddTemplate([FromForm] string title, [FromForm] string body)
                {
                        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
                        {
                                TempData["Error"] = "Title and body are required.";
                                return RedirectToAction(nameof(Templates));
                        }
                        _data.Templates.Upsert(new MessageTemplate { Title = title, Body = body }, x => x.Id);
                        TempData["Success"] = "Template saved.";
                        return RedirectToAction(nameof(Templates));
                }

                [HttpPost]
                public IActionResult DeleteTemplate([FromForm] string id)
                {
                        _data.Templates.Delete(id, x => x.Id);
                        TempData["Success"] = "Template deleted.";
                        return RedirectToAction(nameof(Templates));
                }

                public IActionResult Broadcast()
                {
                        ViewBag.Classes = _data.Classes.GetAll();
                        ViewBag.Templates = _data.Templates.GetAll();
                        return View();
                }

                [HttpPost]
                public async Task<IActionResult> Broadcast([FromForm] MessageDispatchRequest req)
                {
                        var template = _data.Templates.GetAll().FirstOrDefault(t => t.Id == req.TemplateId);
                        if (template == null)
                        {
                                TempData["Error"] = "Template not found.";
                                return RedirectToAction(nameof(Broadcast));
                        }

                        var recipients = new List<Student>();
                        if (!string.IsNullOrEmpty(req.ClassId))
                        {
                                var cls = _data.Classes.GetAll().FirstOrDefault(c => c.Id == req.ClassId);
                                if (cls != null) recipients.AddRange(cls.Students);
                        }
                        if (req.StudentIds?.Any() == true)
                        {
                                var all = _data.Classes.GetAll().SelectMany(c => c.Students);
                                recipients.AddRange(all.Where(s => req.StudentIds.Contains(s.Id)));
                        }
                        recipients = recipients.DistinctBy(s => s.Id).ToList();

                        foreach (var r in recipients)
                        {
                                if (req.SendSms && !string.IsNullOrWhiteSpace(r.ParentPhone))
                                {
                                        await _sms.SendAsync(r.ParentPhone, template.Body);
                                }
                                if (req.SendEmail && !string.IsNullOrWhiteSpace(r.ParentEmail))
                                {
                                        await _email.SendAsync(r.ParentEmail, template.Title, template.Body);
                                }
                        }

                        TempData["Success"] = $"Sent {recipients.Count} messages.";
                        return RedirectToAction(nameof(Broadcast));
                }
        }
}


