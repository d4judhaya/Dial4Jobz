using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Mvc;

namespace Dial4Jobz.Models
{
    public class BinaryContentResult : ActionResult
    {

        public BinaryContentResult()
        {
        }

        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {

            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.ContentType = ContentType;

            context.HttpContext.Response.AddHeader("content-disposition",

            "attachment; filename=" + FileName);

            context.HttpContext.Response.BinaryWrite(Content);
            //context.HttpContext.Response.WriteFile(context.HttpContext.Server.MapPath(Path)); 
            context.HttpContext.Response.End();
        }
    }

}