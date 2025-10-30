using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using EduBroadcast.Models;

namespace EduBroadcast.Data
{
        public interface IJsonRepository<T>
        {
                IReadOnlyList<T> GetAll();
                T? Get(string id);
                void Upsert(T entity, Func<T, string> idSelector);
                void Delete(string id, Func<T, string> idSelector);
        }

        public sealed class JsonRepository<T> : IJsonRepository<T>
        {
                private readonly string _filePath;
                private readonly JsonSerializerOptions _options = new()
                {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                public JsonRepository(string basePath, string fileName)
                {
                        Directory.CreateDirectory(basePath);
                        _filePath = Path.Combine(basePath, fileName);
                        if (!File.Exists(_filePath))
                        {
                                File.WriteAllText(_filePath, "[]");
                        }
                }

                public IReadOnlyList<T> GetAll()
                {
                        var json = File.ReadAllText(_filePath);
                        var list = JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
                        return list;
                }

                public T? Get(string id)
                {
                        var all = GetAll();
                        return all.FirstOrDefault();
                }

                public void Upsert(T entity, Func<T, string> idSelector)
                {
                        var list = GetAll().ToList();
                        var id = idSelector(entity);
                        var idx = list.FindIndex(x => idSelector(x) == id);
                        if (idx >= 0)
                        {
                                list[idx] = entity;
                        }
                        else
                        {
                                list.Add(entity);
                        }
                        File.WriteAllText(_filePath, JsonSerializer.Serialize(list, _options));
                }

                public void Delete(string id, Func<T, string> idSelector)
                {
                        var list = GetAll().Where(x => idSelector(x) != id).ToList();
                        File.WriteAllText(_filePath, JsonSerializer.Serialize(list, _options));
                }
        }

        public sealed class AppData
        {
                public IJsonRepository<ClassRoom> Classes { get; }
                public IJsonRepository<MessageTemplate> Templates { get; }
                public IJsonRepository<AbsenceLog> Absences { get; }
                public IJsonRepository<ConferenceSlot> Slots { get; }
                public IJsonRepository<Booking> Bookings { get; }

                public AppData(string basePath)
                {
                        Classes = new JsonRepository<ClassRoom>(basePath, "classes.json");
                        Templates = new JsonRepository<MessageTemplate>(basePath, "templates.json");
                        Absences = new JsonRepository<AbsenceLog>(basePath, "absences.json");
                        Slots = new JsonRepository<ConferenceSlot>(basePath, "slots.json");
                        Bookings = new JsonRepository<Booking>(basePath, "bookings.json");
                }
        }

        public static class SeedData
        {
                public static void EnsureSeed(AppData data)
                {
                        if (!data.Classes.GetAll().Any())
                        {
                                var a = new ClassRoom { Name = "Algebra I", Students = new List<Student>{ new Student{ FullName = "Alex Kim", ParentEmail = "parent1@example.com", ParentPhone = "+15550000001" }, new Student{ FullName = "Sam Patel", ParentEmail = "parent2@example.com", ParentPhone = "+15550000002" } } };
                                var b = new ClassRoom { Name = "Biology", Students = new List<Student>{ new Student{ FullName = "Jamie Lee", ParentEmail = "parent3@example.com", ParentPhone = "+15550000003" } } };
                                data.Classes.Upsert(a, x => x.Id);
                                data.Classes.Upsert(b, x => x.Id);
                        }

                        if (!data.Templates.GetAll().Any())
                        {
                                data.Templates.Upsert(new MessageTemplate{ Title = "Permission Slip Reminder", Body = "Don't forget to return the permission slip tomorrow." }, x => x.Id);
                                data.Templates.Upsert(new MessageTemplate{ Title = "Homework Reminder", Body = "Please complete the assigned homework for next class." }, x => x.Id);
                        }
                }
        }
}


