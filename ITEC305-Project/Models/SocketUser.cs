using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSockets;

namespace Maika.Models
{
    public class SocketUser
    {
		[JsonIgnore]
		public WebSocket Session { get; set; }
		public string RoomId { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
	}
}
