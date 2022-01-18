using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class EmailModelBulk
    {
        public List<string> EmailList { get; set; }
        public string Subject { get; set; }
        public string CC { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        [UIHint("File")]
        public HttpPostedFileBase[] UploadFiles { get; set; }

    }
}