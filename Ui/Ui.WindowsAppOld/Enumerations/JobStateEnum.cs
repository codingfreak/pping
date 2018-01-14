namespace codingfreaks.pping.Ui.WindowsApp.Enumerations
{
    using System;
    using System.Linq;

    using Logic;

    /// <summary>
    /// Defines possible states a <see cref="PingJob"/> can obtain.
    /// </summary>
    public enum JobStateEnum
    {
        Unkown = 0,

        Defined = 1,

        Starting = 2,

        Waiting = 3,

        Running = 4,

        Stopping = 5,

        Finished = 5
    }
}