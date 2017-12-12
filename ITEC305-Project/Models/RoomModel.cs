using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maika.Models
{
    class RoomModel
    {
		public string Id { get; set; }
		public string Name { get; set; }
		public UserModel Owner { get; set; }
		public List<UserModel> Members { get; set; }
		public bool IsPublic { get; set; }

		public bool IsMember(UserPrincipal user) => Members.Any(x => x.Id == user.Id);

		public bool Join(UserPrincipal user) => MaikaCore.JoinRoom(user, Id);
	}
}
