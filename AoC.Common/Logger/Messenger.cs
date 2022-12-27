using System;
using System.ComponentModel.Composition;

namespace AoC.Common
{
	public class Messenger : IMessenger
	{
		public event EventHandler<string> OnMessageSent;

		public void Send(object sender, string message)
		{
			OnMessageSent?.Invoke(sender, message);
		}
	}
}
