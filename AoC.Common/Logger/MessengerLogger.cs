using System;
using System.ComponentModel.Composition;

namespace AoC.Common.Logger;

[Export]
public class MessengerLogger : ILogger
{
	private readonly IMessenger logMessenger;

	public MessengerLogger(IMessenger logMessenger, SeverityLevel severity = SeverityLevel.Info)
	{
		this.logMessenger = logMessenger;
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
		if (severity >= Severity)
			logMessenger.Send(this, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {severity,-7} | {category,-8} | {message}");
	}
}
