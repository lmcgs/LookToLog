using System.Collections.Generic;

namespace LookToLog.Models
{
    public class LogViewModel
    {
        public string SelectedLogPath { get; set; }

        public List<Log> LogList { get; set; }
    }

    public class Log
    {
        public int Row { get; set; }

        public string Content { get; set; }
    }
}