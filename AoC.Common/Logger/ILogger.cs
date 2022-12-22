using System;

namespace AoC.Common
{
	public enum SeverityLevel
	{
		Verbose,
		Debug,
		Info,
		Warning,
		Error
	}

	public interface ILogger
	{
		event EventHandler<string> OnMessageSent;

		SeverityLevel Severity { get; set; }

		void Send(SeverityLevel severity, string category, string message);
	}
}
