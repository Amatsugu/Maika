using System;
using Nancy;
using Nancy.Hosting.Self;

namespace ITEC305_Project
{
    class Program
    {
        static void Main(string[] args)
        {
			var uri = new Uri("http://localhost:4321");
			var host = new NancyHost(uri);
			host.Start();
			Console.WriteLine($"Hosting on {uri}...");
			Console.ReadLine();
			host.Stop();
        }
    }
}
