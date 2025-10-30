using System;
using System.Collections.Generic;

namespace SchoolCommApp.Models
{
	public class Student
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");
		public string FullName { get; set; } = string.Empty;
		public string ParentEmail { get; set; } = string.Empty;
		public string ParentPhone { get; set; } = string.Empty;
	}

	public class ClassRoom
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");
		public string Name { get; set; } = string.Empty;
		public List<Student> Students { get; set; } = new();
	}

	public class MessageTemplate
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");
		public string Title { get; set; } = string.Empty;
		public string Body { get; set; } = string.Empty;
	}

	public class MessageDispatchRequest
	{
		public string TemplateId { get; set; } = string.Empty;
		public string? ClassId { get; set; }
		public List<string> StudentIds { get; set; } = new();
		public bool SendSms { get; set; }
		public bool SendEmail { get; set; }
	}

	public class AbsenceLog
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");
		public string ClassId { get; set; } = string.Empty;
		public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
		public string WhatWeCovered { get; set; } = string.Empty;
		public string Handouts { get; set; } = string.Empty;
		public string Homework { get; set; } = string.Empty;
	}

	public class ConferenceSlot
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");
		public DateTimeOffset StartUtc { get; set; }
		public DateTimeOffset EndUtc { get; set; }
		public bool IsBooked { get; set; }
	}

	public class Booking
	{
		public string Id { get; set; } = Guid.NewGuid().ToString("N");
		public string SlotId { get; set; } = string.Empty;
		public string ParentName { get; set; } = string.Empty;
		public string ParentEmail { get; set; } = string.Empty;
		public string StudentName { get; set; } = string.Empty;
	}
}


