using System;
using System.IO;

namespace Init
{
	class Program
	{
		public static bool Verbose;

		static int Main(string[] args)
		{
			var arguments = InitArguments.Parse(args);
			Console.ForegroundColor = ConsoleColor.White;

			if (arguments.HasErrors)
			{
				return 1;
			}

			Verbose = arguments.Verbose;

			var options = TemplateLuaSettings.GetTemplateSettings(arguments.Template);
			options.Arguments = arguments;

			ZipUnpacker.Unpack(options);

			return 0; // All is fine
		}
	}
}
