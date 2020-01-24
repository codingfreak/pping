namespace codingfreaks.pping.Ui.ConsoleApp
{
    using System;
    using System.Linq;

    using McMaster.Extensions.CommandLineUtils;

    /// <summary>
    /// Main entry point of the console application.
    /// </summary>
    [HelpOption]
    [Subcommand(typeof(PingLogic))]
    internal class Program
    {
        #region methods

        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>The result of the program.</returns>
        public static int Main(string[] args)
        {
            var result = CommandLineApplication.Execute<PingLogic>(args);
            Environment.ExitCode = result;
            return result;
        }

        #endregion
    }
}