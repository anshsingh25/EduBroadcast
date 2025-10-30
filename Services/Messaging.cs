using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EduBroadcast.Models;

namespace EduBroadcast.Services
{
        public interface ISmsSender
        {
                Task SendAsync(string toPhone, string body);
        }

        public interface IEmailSender
        {
                Task SendAsync(string toEmail, string subject, string body);
        }

        public sealed class FileLogSmsSender : ISmsSender
        {
                private readonly string _logPath;
                public FileLogSmsSender(string logPath)
                {
                        _logPath = logPath;
                        Directory.CreateDirectory(Path.GetDirectoryName(_logPath) ?? ".");
                }
                public Task SendAsync(string toPhone, string body)
                {
                        File.AppendAllText(_logPath, $"SMS to {toPhone}: {body}{Environment.NewLine}");
                        return Task.CompletedTask;
                }
        }

        public sealed class FileLogEmailSender : IEmailSender
        {
                private readonly string _logPath;
                public FileLogEmailSender(string logPath)
                {
                        _logPath = logPath;
                        Directory.CreateDirectory(Path.GetDirectoryName(_logPath) ?? ".");
                }
                public Task SendAsync(string toEmail, string subject, string body)
                {
                        File.AppendAllText(_logPath, $"Email to {toEmail} | {subject}: {body}{Environment.NewLine}");
                        return Task.CompletedTask;
                }
        }
}


