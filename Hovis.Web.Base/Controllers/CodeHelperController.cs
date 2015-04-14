using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hovis.Web.Base.Models;
using System.Net.Mail;
using System.Net;

namespace Hovis.Web.Base.Controllers
{
    public class CodeHelperController : Controller
    {

        private StarterAndLeaverEntities db = new StarterAndLeaverEntities();

        // GET: CodeHelper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendNewMail(string ReturnController, string ReturnAction, string FromSetting, string ToSetting, string EmailBody, string EmailSubject)
        {
            MailMessage mail = new MailMessage();

            // set subject and body an variables
            mail.Subject = EmailSubject;
            mail.Body = EmailBody;
            string emailfrom = "";
            string emailport = "0";
            string authuser = "";
            string authpass = "";

            IQueryable<t_Application_Standard_Settings> authusersetting = db.t_Application_Standard_Settings
                    .Where(x => x.Setting.Contains("AuthUser"));
            authusersetting = authusersetting.Take(1);
            foreach (var item in authusersetting)
            {
                if (item.Setting == "AuthUser")
                {
                    authuser = Convert.ToString(item.SettingValue);
                }
            }

            IQueryable<t_Application_Standard_Settings> authpasssetting = db.t_Application_Standard_Settings
                    .Where(x => x.Setting.Contains("AuthPass"));
            authpasssetting = authpasssetting.Take(1);
            foreach (var item in authpasssetting)
            {
                if (item.Setting == "AuthPass")
                {
                    authpass = Convert.ToString(item.SettingValue);
                }
            }

            //get the from email server port (may not need it)
            IQueryable<t_Application_Standard_Settings> portsetting = db.t_Application_Standard_Settings
                                .Where(x => x.Setting == "EmailServerPort");
            portsetting = portsetting.Take(1);
            foreach (var item in portsetting)
            {
                if (item.SettingValue != "")
                {
                    emailport = Convert.ToString(item.SettingValue);
                }
            }

            //get the from email address
            IQueryable<t_Application_Standard_Settings> fromsetting = db.t_Application_Standard_Settings
                                .Where(x => x.Setting == FromSetting);
            fromsetting = fromsetting.Take(1);
            foreach (var item in fromsetting)
            {
                mail.From = new MailAddress(item.SettingValue);
            }

            //get the to email addresses
            IQueryable<t_Application_Standard_Settings> tosetting = db.t_Application_Standard_Settings
                                .Where(x => x.Setting == ToSetting);
            foreach (var item in tosetting)
            {
                mail.To.Add(item.SettingValue);
            }

            //get the from email server address and send email if it exists
            IQueryable<t_Application_Standard_Settings> serversetting = db.t_Application_Standard_Settings
                                .Where(x => x.Setting == "EmailServer");
            serversetting = serversetting.Take(1);
            foreach (var item in serversetting)
            {
                SmtpClient smtpServer = new SmtpClient(item.SettingValue);
                smtpServer.Port = Convert.ToInt32(emailport);
                smtpServer.EnableSsl = true;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.Credentials = new System.Net.NetworkCredential(authuser, authpass);
                smtpServer.Send(mail);
                if (ReturnAction == "CreateStarter" || ReturnAction == "CreateRequest")
                {
                    return RedirectToAction(ReturnAction, ReturnController, new { status = "Success" });
                }
                else
                {
                    return RedirectToAction(ReturnAction, ReturnController);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }
    }
}