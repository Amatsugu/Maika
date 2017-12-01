using Newtonsoft.Json;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITEC305_Project.Models
{
    public class SocketUser
    {
		[JsonIgnore]
		public WebSocketSession Session { get; set; }
		public string RoomId { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
	}
}
