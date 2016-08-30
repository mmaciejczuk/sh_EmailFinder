using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web.Helpers;
using System.Web.Mvc;
using EmailFinder.Models;
using Newtonsoft.Json.Linq;

namespace EmailFinder.Controllers
{
    public class FinderController : Controller
    {
        [HttpGet]
        public ActionResult GetEmails(string name, string surname, string domain)
        {
            return PartialView("EmailList", this.GenerateEmails(name, surname, domain));
        }

        [HttpGet]
        public JsonResult EmailExists(string email, EmailDiscoveryService service)
        {
            switch (service)
            {
                case EmailDiscoveryService.MailboxLayer:
                    return Json(MailboxLayer(email), JsonRequestBehavior.AllowGet);
                case EmailDiscoveryService.VerifyEmail:
                    return Json(VerifyEmail(email), JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        protected VerifyResult VerifyEmail(string email)
        {
            var verifyResult = new VerifyResult() {Email = email, Service = EmailDiscoveryService.VerifyEmail.ToString()};

            string apiUrl = "http://api.verify-email.org/api.php?";
            string apiUsername = "rmatyszewski";
            string apiPassword = "jacekplacek";

            WebClient webClient;
            try
            {
                using (webClient = new WebClient())
                {
                        string result = webClient.DownloadString(string.Format("{0}usr={1}&pwd={2}&check={3}", apiUrl, apiUsername, apiPassword, email));
                        JObject objJSON = default(JObject);
                        objJSON = JObject.Parse(result);

                        if (objJSON["verify_status"] != null)
                        verifyResult.IsValid = (Convert.ToBoolean(Convert.ToInt32(objJSON["verify_status"].ToString())));                        
                }
                return verifyResult;
            }
            catch (Exception e)
            {
                return verifyResult;
            }
        }


        protected VerifyResult MailboxLayer(string email)
        {
            var verifyResult = new VerifyResult() { Email = email, Service = EmailDiscoveryService.MailboxLayer.ToString() };

            string accessKey = "59a37991b326fe7201f628ea15f0347d";
            string apiUrl = "https://apilayer.net/api/check?";
            WebClient webClient;
            try
            {
                using (webClient = new WebClient())
                {
                        string result = webClient.DownloadString(string.Format("{0}access_key={1}&email={2}", apiUrl, accessKey, email));
                        JObject objJSON = default(JObject);
                        objJSON = JObject.Parse(result);

                        if (objJSON["format_valid"] != null)
                            verifyResult.IsValid = (Convert.ToBoolean(objJSON["format_valid"].ToString()));                    
                }
                return verifyResult;
            }
            catch (Exception e)
            {
                return verifyResult;
            }
        }

        protected List<string> GenerateEmails(string name, string surname, string domain)
        {
            List<string> list = new List<string>
            {
                name + "@" + domain,
                surname + "@" + domain,
                name + surname + "@" + domain,
                name + "." + surname + "@" + domain,
                name[0] + surname + "@" + domain,
                name[0] + "." + surname + "@" + domain,
                name + surname[0] + "@" + domain,
                name + "." + surname[0] + "@" + domain,
                name[0] + "" + surname[0] + "@" + domain,
                surname + name + "@" + domain,
                surname + "." + name + "@" + domain,
                surname + name[0] + "@" + domain,
                surname + "." + name[0] + "@" + domain,
                name + "_" + surname + "@" + domain,
                name[0] + "_" + surname + "@" + domain
            };
            return list;
        }


    }
}