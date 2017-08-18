using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookToLog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.IO;

namespace LookToLog.Controllers
{
    public class HomeController : BaseController
    {
        private IEnumerable GetSelectListForLogFiles()
        {
            var LogDropdownItems = Startup.LogSettings.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Path
            }).OrderBy(o => o.Text).ToList();

            return LogDropdownItems;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LogViewModel();

            ViewBag.DrowpdownList = new SelectList(GetSelectListForLogFiles(), "Value", "Text");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LogViewModel model)
        {
            ViewBag.DrowpdownList = new SelectList(GetSelectListForLogFiles(), "Value", "Text");

            model.LogList = new List<Log>();

            var path = Path.GetDirectoryName(model.SelectedLogPath) + "/";
            var copyFile = path + Guid.NewGuid().ToString() + ".txt";

            System.IO.File.Copy(model.SelectedLogPath, copyFile);

            try
            {
                if (IsFileClosed(copyFile, true))
                {
                    using (var sr = new StreamReader(new System.IO.FileStream(copyFile,
                                                        System.IO.FileMode.Open,
                                                        System.IO.FileAccess.Read)))
                    {

                        var line = sr.ReadLine();

                        var i = 1;

                        while (line != null)
                        {
                            var log = new Log { Row = i, Content = line };

                            model.LogList.Add(log);

                            line = sr.ReadLine();
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            System.IO.File.Delete(copyFile);

            return View(model);
        }
    }
}
