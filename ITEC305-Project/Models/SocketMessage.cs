using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITEC305_Project.Models
{
	public enum MessageType
	{
		Chat,
		Draw,
		Join,
		Leave,
		RoomClose
	}

    public class SocketMessage
    {
		public SocketUser User { get; set; }
		public MessageType Type { get; set; }
		public string Message { get; set; }


		public static SocketMessage FromJSON(string JSON) => JsonConvert.DeserializeObject<SocketMessage>(JSON);
		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}
