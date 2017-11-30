using System;
using System.Collections.Generic;
using System.Text;

namespace ITEC305_Project.Models
{
    class RoomModel
    {
		public string Id { get; set; }
		public string Name { get; set; }
		public string Owner { get; set; }
		public UserModel[] Members { get; set; }
	}
}
