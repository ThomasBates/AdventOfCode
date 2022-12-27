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
		SeverityLevel Severity { get; set; }

		void SendVerbose(string category, string message);

		void SendDebug(string category, string message);

		void SendInfo(string category, string message);

		void SendWarning(string category, string message);

		void SendError(string category, string message);
	}
}
