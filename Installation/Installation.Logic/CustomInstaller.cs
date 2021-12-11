namespace codingfreaks.pping.Installation.Logic
{
	using Microsoft.Win32;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Configuration.Install;
	using System.Linq;
	using System.Threading.Tasks;

	[RunInstaller(true)]
	public partial class CustomInstaller : System.Configuration.Install.Installer
	{
		public CustomInstaller()
		{
			InitializeComponent();				
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);
			var targetDirectory = Context.Parameters["TargetDir"];
			var oldPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
			var newPath = $"{oldPath};{targetDirectory}";
			Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);			
		}
	}
}
