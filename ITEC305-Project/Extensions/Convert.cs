using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ITEC305_Project.Extensions
{
	public static class Convert
	{
		public static string ToBase62(this Guid Id)
		{
			var number = new BigInteger(Id.ToByteArray());
			var sb = new StringBuilder();
			if (number < 0)
			{
				sb.Append("~");
				number *= -1;
			}
			do
			{
				sb.Append("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"[(int)(number % 62)]);
				number /= 62;

			} while (number > 0);
			return sb.ToString();
		}
	}
}
