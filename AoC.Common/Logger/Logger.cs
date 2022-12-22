﻿using System;
using System.ComponentModel.Composition;

namespace AoC.Common
{
	[Export]
	public class Logger : ILogger
	{
		public event EventHandler<string> OnMessageSent;

		public Logger(SeverityLevel severity)
		{
			Severity = severity;
		}

		public SeverityLevel Severity { get; set; }

		public void Send(SeverityLevel severity, string category, string message)
		{
			if (severity >= Severity) 
				OnMessageSent?.Invoke(this, $"[{severity,-7}] - [{category,-15}] - {message}");
		}
	}
}
