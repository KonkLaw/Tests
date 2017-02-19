using System;
using System.IO;

namespace Tests
{
	class Log
	{
		static string Path = @"D:\log.txt";
		static string NewLine = Environment.NewLine;

		public static void Write(string message)
		{
			File.WriteAllText(Path, message + NewLine);
		}

		public static void Write(Exception exception)
		{
			File.WriteAllText(Path, exception.ToString() + NewLine);
		}
	}
}
