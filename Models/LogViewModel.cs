using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

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