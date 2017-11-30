using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace ITEC305_Project.Models
{
    public class DBCredentials
    {
		public string DB_Name { get; set; }
		public string DB_User { get; set; }
		public string DB_Pass { get; set; }

		[JsonIgnore]
		public string ConntectionString => $"Host={Maika.HOST};Username={DB_User};Password={DB_Pass};Database={DB_Name}";

		public static DBCredentials FromJSON(string file) => JsonConvert.DeserializeObject<DBCredentials>(File.ReadAllText(file));
		public void Save(string file) => File.WriteAllText(file, JsonConvert.SerializeObject(this));
	}
}
