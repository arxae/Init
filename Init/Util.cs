using System;
using System.IO;

namespace Init
{
	public class Util
	{
		/// <summary>
		/// Returns the path to the "root" template directory
		/// </summary>
		/// <returns></returns>
		public static string GetTemplateDirectory()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
		}

		/// <summary>
		/// Returns the path to a specific template directory
		/// </summary>
		/// <param name="templateName"></param>
		/// <returns></returns>
		public static string GetTemplateDirectory(string templateName)
		{
			return Path.Combine(GetTemplateDirectory(), templateName);
		}

		public static void PrintWarning(string text)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void PrintError(string text)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}