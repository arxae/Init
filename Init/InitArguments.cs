using System.Linq;
using CommandLine;

namespace Init
{
	public class InitArguments
	{
		[Option('n', "name", Required = false, HelpText = "Name of the app you are instantiating")]
		public string Name { get; set; }

		[Option('t', "template", Required = true, HelpText = "Initializes a new template")]
		public string Template { get; set; }

		[Option('s', "subtemplate", Required = false, DefaultValue = "default", HelpText = "Initializes a specific subtype of the template")]
		public string SubTemplate { get; set; }

		[Option('v', "verbose", Required = false, DefaultValue = false, HelpText = "Outputs more text then needed")]
		public bool Verbose { get; set; }

		[Value(0, DefaultValue="App")]
		public string AppName { get; set; }

		public bool HasErrors;

		public static InitArguments Parse(string[] args)
		{
			var parsed = Parser.Default.ParseArguments<InitArguments>(args);
			parsed.Value.HasErrors = parsed.Errors.Any();

			return parsed.Value;
		}
	}
}