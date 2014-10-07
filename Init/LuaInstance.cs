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

			Lua.error = new Action<string>(Util.PrintError);
			Lua.warning = new Action<string>(Util.PrintWarning);
			Lua.print = new Action<string>(Console.WriteLine);
			Lua.printf = new Action<string, string[]>(Console.WriteLine);
			Lua.setForecolor = new Action<string>((colorStr) =>
			{
				Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorStr);
			});

			Lua.setBackColor = new Action<string>((colorStr) =>
			{
				Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorStr);
			});

			Lua.resetColor = new Action(() =>
			{
				Console.ResetColor();
				Console.ForegroundColor = ConsoleColor.White;
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
