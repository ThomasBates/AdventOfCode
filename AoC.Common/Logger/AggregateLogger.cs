using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace AoC.Common.Logger;

[Export]
public class AggregateLogger : ILogger
{
	private readonly IEnumerable<ILogger> loggers;
	private SeverityLevel severity;

	public AggregateLogger(IEnumerable<ILogger> loggers)
	{
		this.loggers = loggers;
	}

	public SeverityLevel Severity
	{
		get => loggers.First().Severity;
		set
		{
			foreach (var logger in loggers)
				logger.Severity = value;
		}
	}

	public void SendVerbose(string category, string message)
	{
		foreach (var logger in loggers)
			logger.SendVerbose(category, message);
	}

	public void SendDebug(string category, string message)
	{
		foreach (var logger in loggers)
			logger.SendDebug(category, message);
	}

	public void SendInfo(string category, string message)
	{
		foreach (var logger in loggers)
			logger.SendInfo(category, message);
	}

	public void SendWarning(string category, string message)
	{
		foreach (var logger in loggers)
			logger.SendWarning(category, message);
	}

	public void SendError(string category, string message)
	{
		foreach (var logger in loggers)
			logger.SendError(category, message);
	}
}
