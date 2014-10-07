using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

namespace Init
{
	public class ZipUnpacker
	{
		public static void Unpack(TemplateLuaSettings options)
		{
			if (Directory.Exists(Util.GetTemplateDirectory(options.Arguments.Template)) == false)
			{
				Util.PrintError(string.Format("Template {0} not found", options.Arguments.Template));
				return;	
			}

			Console.WriteLine("Extracting {0} template...", options.Arguments.Template);

			string zipPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", options.Arguments.Template, options.Arguments.SubTemplate) + ".zip";
			string extractPath = Directory.GetCurrentDirectory();

			string appName;
			if (options.Arguments.AppName != null)
			{
				appName = options.Arguments.AppName;
			}
			else
			{
				appName = Path.GetFileName(extractPath);
			}

			if (options.Arguments.Verbose)
			{
				Console.WriteLine("Extracting to {0}", extractPath);
			}

			using (ZipFile zip = ZipFile.Read(zipPath))
			{
				foreach (ZipEntry entry in zip)
				{
					Console.Write("Extracting ");
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.WriteLine(entry.FileName);
					Console.ForegroundColor = ConsoleColor.White;

					entry.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);

					// replace <<<apname>>> with either appname, or directory name
					if (entry.IsDirectory == false)
					{
						var lines = File.ReadAllLines(Path.Combine(extractPath, entry.FileName));
						var output = new List<string>();

						foreach (var line in lines)
						{
							if (line.Contains(options.AppNameMacro))
							{
								output.Add(line.Replace(options.AppNameMacro, appName));
							}
							else
							{
								output.Add(line);
							}
						}

						File.WriteAllLines(Path.Combine(extractPath, entry.FileName), output);
					}
				}
			}
		}
	}
}
