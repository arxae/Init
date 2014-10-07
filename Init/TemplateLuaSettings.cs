using System;
using System.IO;

namespace Init
{
	public class TemplateLuaSettings
	{
		public string AppNameMacro;
		public InitArguments Arguments;

		public static TemplateLuaSettings GetTemplateSettings(string templateName)
		{
			// Setup default values
			LuaInstance.Get.Do("template={}");
			LuaInstance.Get.Lua.template.appNameMacro = "<<<appname>>>";

			string scriptPath = Path.Combine(Util.GetTemplateDirectory(templateName), "template.lua");

			if (File.Exists(scriptPath))
			{
				LuaInstance.Get.DoFile(scriptPath);
			}
			else
			{
				if (Program.Verbose)
				{
					Util.PrintWarning("template.lua file not found, using default settings");
				}
			}

			var templateSettings = new TemplateLuaSettings
			{
				AppNameMacro = LuaInstance.Get.Lua.template.appNameMacro
			};

			return templateSettings;
		}
	}
}