using System;

namespace AoC.Common
{
	public interface IMessenger
	{
		event EventHandler<string> OnMessageSent;

		void Send(object sender, string message);
	}
}
