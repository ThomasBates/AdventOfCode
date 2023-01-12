using System;

namespace AoC.Common.Logger;

public class Messenger : IMessenger
{
	public event EventHandler<string> OnMessageSent;

	public Messenger(EventHandler<string> eventHandler = null)
	{
		if (eventHandler != null)
			OnMessageSent += eventHandler;
	}

	public void Send(object sender, string message)
	{
		OnMessageSent?.Invoke(sender, message);
	}
}
