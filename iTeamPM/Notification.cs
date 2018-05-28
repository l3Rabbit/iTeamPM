using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace iTeamPM
{
	public class Notification : Hub
	{
		public void MyChatSend(string name, string to, string message)
		{
			Clients.All.broadcastMessage(name, to, message);
		}
	}
}