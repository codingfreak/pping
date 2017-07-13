namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the model to store and retrieve all options from the data sink.
    /// </summary>
    public class OptionsModel : BaseModel
    {
        #region properties

        /// <summary>
        /// The list of job definitions.
        /// </summary>
        public IEnumerable<JobModel> JobDefinitions { get; set; }

        #endregion
    }
}