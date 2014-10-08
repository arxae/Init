using System;
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

			Program.ExtractDirectory = extractPath;

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

					if (entry.IsDirectory == false)
					{
						using (var ms = new MemoryStream())
						{
							entry.Extract(ms);

							ms.Position = 0;
							using (var sr = new StreamReader(ms))
							{
								var str = sr.ReadToEnd();

								// convert to byte array for testing
								byte[] bytes = new byte[str.Length*sizeof (char)];
								Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

								bool isBinary = false;
								byte previousByte = 1; // Set to 1 since we are checking for 0
								foreach (var b in bytes)
								{
									if (b == 0 && previousByte == 0)
									{
										isBinary = true;
									}

									previousByte = b;
								}

								// If it is a binary, regulary extract it and move on to the next entry
								if (isBinary && options.Arguments.Verbose)
								{
									Util.PrintWarning(string.Format("Binary file: {0}", entry.FileName));
									entry.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);
									continue;
								}

								if (str.Contains(options.AppNameMacro))
								{
									str = str.Replace(options.AppNameMacro, appName);
								}

								File.WriteAllText(Path.Combine(extractPath, entry.FileName), str);
							}
						}
					}
					else
					{
						entry.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);
					}
				}
			}

			string postScriptPath = Path.Combine(extractPath, "post.lua");
			if (File.Exists(Path.Combine(extractPath, "post.lua")))
			{
				if (Program.Verbose)
				{
					Util.PrintWarning("Executing post script...");
				}

				LuaInstance.Get.DoFile(postScriptPath);
				File.Delete(postScriptPath);
			}
			else
			{
				if (Program.Verbose)
				{
					Util.PrintWarning("No post script...");
				}
			}
		}
	}
}
