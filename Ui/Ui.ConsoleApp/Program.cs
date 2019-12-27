namespace codingfreaks.pping.Ui.ConsoleApp
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Hosting;

    
    [HelpOption]
    [VersionOptionFromMember(MemberName = "GetVersion")]
    [Subcommand(typeof(PingLogic))]
    internal class Program
    {
        #region methods

        private string GetVersion()
        {
            return typeof(Program).Assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        }

        public static int Main(string[] args) => CommandLineApplication.Execute<PingLogic>(args);

        #endregion
    }
}