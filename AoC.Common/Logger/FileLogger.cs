using System;
using System.ComponentModel.Composition;
using System.IO;

namespace AoC.Common
{
	[Export]
	public class FileLogger : ILogger
	{
		private string location;

		public FileLogger(SeverityLevel severity)
		{
			Severity = severity;
		}

		public SeverityLevel Severity { get; set; }

		public void SendVerbose(string category, string message)
		{
			Send(SeverityLevel.Verbose, category, message);
		}

		public void SendDebug(string category, string message)
		{
			Send(SeverityLevel.Debug, category, message);
		}

		public void SendInfo(string category, string message)
		{
			Send(SeverityLevel.Info, category, message);
		}

		public void SendWarning(string category, string message)
		{
			Send(SeverityLevel.Warning, category, message);
		}

		public void SendError(string category, string message)
		{
			Send(SeverityLevel.Error, category, message);
		}

		private void Send(SeverityLevel severity, string category, string message)
		{
			if (severity < Severity)
				return;

			if (string.IsNullOrEmpty(location))
			{
				var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AdventOfCode\\Logging");
				var fileName = String.Format($"AoC.{DateTime.Now:yyyyMMdd.HHmmss}.Debug.txt");
				location = Path.Combine(path, fileName);
			}

			using var writer = new StreamWriter(location, true);

			writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {severity,-7} | {category,-8} | {message}");
		}
	}
}
