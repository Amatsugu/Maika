using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maika.Models
{
	public enum MessageType
	{
		Chat,
		Draw,
		Join,
		Leave,
		RoomClose,
		JoinInfo,
		RoomInfo
	}

    public class SocketMessage
    {
		public SocketUser User { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public MessageType Type { get; set; }
		public string Message { get; set; }


		public static SocketMessage FromJSON(string JSON) => JsonConvert.DeserializeObject<SocketMessage>(JSON);
		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}
