using System;
using System.IO;

namespace Init
{
	public class LuaInstance
	{
		#region Singleton
		private static LuaInstance _instance;
		public static LuaInstance Get
		{
			get { return _instance ?? (_instance = new LuaInstance()); }
		}
		#endregion

		public dynamic Lua;

		public LuaInstance()
		{
			Lua = new DynamicLua.DynamicLua();
			HookLuaFunctions();
		}

		private void HookLuaFunctions()
		{
			// Set C# => Lua functions
			Lua.error = new Action<string>(Util.PrintError);
			Lua.warning = new Action<string>(Util.PrintWarning);
			Lua.print = new Action<string>(Console.WriteLine);
			Lua.printf = new Action<string, string[]>(Console.WriteLine);
			Lua.write = new Action<string>(Console.Write);

			Lua.debugmsg = new Action<string>(msg =>
			{
				if (Program.Verbose)
				{
					Console.WriteLine(msg);
				}
			});

			Lua.setForecolor = new Action<string>(colorStr =>
			{
				Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorStr, true);
			});

			Lua.setBackcolor = new Action<string>(colorStr =>
			{
				Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorStr, true);
			});

			Lua.resetColors = new Action(() =>
			{
				Console.ResetColor();
				Console.ForegroundColor = ConsoleColor.White;
			});

			Lua.replaceInFile = new Action<string, string, string>((file, find, replace) =>
			{
				string path = Path.Combine(Program.ExtractDirectory, file);
				string contents = File.ReadAllText(path);
				contents = contents.Replace(find, replace);
				File.WriteAllText(file, contents);
			});
		}

		public void Do(string luaSnippet)
		{
			Lua(luaSnippet);
		}

		public void DoFile(string path)
		{
			Do(File.ReadAllText(path));
		}
	}
}
