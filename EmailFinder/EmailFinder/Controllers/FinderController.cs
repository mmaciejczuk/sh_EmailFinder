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

        //TODO: Do obiektu verifyResult przypisać prawdziwą wartość IsValid, Random tylko chwilowy
        protected VerifyResult VerifyEmail(string email)
        {
            var verifyResult = new VerifyResult() {Email = email, Service = EmailDiscoveryService.VerifyEmail.ToString()};
            //TEMP RANDOM
            var rand = new Random(DateTime.Now.Millisecond).Next(0, int.MaxValue);
            if (rand % 2 == 0)
            {
                verifyResult.IsValid = true;
            }
            //TEMP RANDOM
            Dictionary<string, string> dictResults = new Dictionary<string, string>();
            string apiUrl = "http://api.verify-email.org/api.php?";
            string apiUsername = "rmatyszewski";
            string apiPassword = "jacekplacek";
            string _key = "";
            string _value = "";

            string emailsVerified = "";
            string emailsLimit = "";

            WebClient webClient;

            try
            {
                using (webClient = new WebClient())
                {
                        string result = webClient.DownloadString(string.Format("{0}usr={1}&pwd={2}&check={3}", apiUrl, apiUsername, apiPassword, email));
                        JObject objJSON = default(JObject);
                        objJSON = JObject.Parse(result);

                        //if (objJSON["limit_status"] != null)
                        emailsVerified = "<strong>Emails Verified: </strong>" + (string.Format(objJSON["emails_verified"].ToString()));

                        //if (objJSON["limit_desc"] != null)
                        emailsLimit = "<strong>Emails limit: </strong>" + (string.Format(objJSON["emails_limit"].ToString()));

                        if (String.Format(objJSON["emails_limit"].ToString()) == String.Format(objJSON["emails_verified"].ToString()))
                        {
                            //LabelLimitStatus.Text = emailsVerified;
                            //LabelLimitDesc.Text = emailsLimit;

                            //GVVerifyEmail.DataSource = null;
                            //GVVerifyEmail.DataBind();
                            return verifyResult;
                        }
                        else
                        {

                            if (objJSON["verify_status"] != null && !dictResults.ContainsKey(email))
                            {
                                _key = string.Format(email);
                                _value = string.Format(Convert.ToBoolean(Convert.ToInt32(objJSON["verify_status"].ToString())) ? "TRUE" : "FALSE");
                                if (_value == "")
                                    _value = "Limit reached";
                                dictResults.Add(_key, _value);
                                return verifyResult;
                            }



                        }
                    
                    //GVVerifyEmail.DataSource = dictResults;
                    //GVVerifyEmail.DataBind();

                    //LabelLimitStatus.Text = "<strong>Emails Verified: </strong>" + emailsVerified;
                    //LabelLimitDesc.Text = "<strong>Emails limit: </strong>" + emailsLimit;
                }
            }
            catch (Exception e)
            {
                return verifyResult;
            }
            return verifyResult;
        }

        //TODO: Do obiektu verifyResult przypisać prawdziwą wartość IsValid, random tylko chwilowy
        protected VerifyResult MailboxLayer(string email)
        {
            var verifyResult = new VerifyResult() { Email = email, Service = EmailDiscoveryService.MailboxLayer.ToString() };
            //TEMP RANDOM
            var rand = new Random(DateTime.Now.Millisecond).Next(0, int.MaxValue);
            if (rand %2 == 0)
            {
                verifyResult.IsValid = true;
            }
            //TEMP RANDOM
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

                        if (objJSON["smtp_check"] != null)
                        {
                            //MailBoxer mb = new MailBoxer();

                            //if (objJSON["email"] != null)
                            //    mb.email = (string.Format(objJSON["email"].ToString()));

                            //if (objJSON["format_valid"] != null)
                            //    mb.isValid = (string.Format(objJSON["format_valid"].ToString())).ToUpper();

                            //li.Add(mb);
                            //Variable.variable += 7;
                            //Variable.counter += 1;

                            //context.Clients.All.onHitRecorded2(Variable.counter, Variable.variable, mb.email, mb.isValid);
                        }
                        else
                        {
                            //GVMailBoxer.DataSource = null;
                            //GVMailBoxer.DataBind();
                            //break;
                        }
                }
            }
            catch (Exception e)
            {
                return verifyResult;
            }
            return verifyResult;
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