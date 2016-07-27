using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TutorialSignalR.Models;

namespace TutorialSignalR
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelError.Visible = true;
            LabelLimitStatus.Text = "";
            LabelLimitStatus.Text = "";
        }

        protected void BtnGenerate_Click(object sender, EventArgs e)
        {
            GVVerifyEmail.DataSource = null;
            GVVerifyEmail.DataBind();
            GVMailBoxer.DataSource = null;
            GVMailBoxer.DataBind();

            if (CheckBoxVE.Checked == true)
            {
                VerifyEmail(generateEmail(TbName.Text, TbSurname.Text, TbDomain.Text));
            }
            if (CheckBoxMB.Checked == true)
            {
                VerifyEmailMailBoxer(generateEmail(TbName.Text, TbSurname.Text, TbDomain.Text));              
            }
        }

        protected void VerifyEmail(List<string> emailsList)
        {
            Variable.variable = 0;

            Dictionary<string, string> dictResults = new Dictionary<string, string>();
            string apiUrl = "http://api.verify-email.org/api.php?";
            string apiUsername = "rmatyszewski";
            string apiPassword = "jacekplacek";
            string _key = "";
            string _value = "";
            Variable.variable = 0;
            Variable.counter = 0;

            string emailsVerified = "";
            string emailsLimit = "";

            var context = GlobalHost.ConnectionManager.GetHubContext<HitCounterHub>();

            WebClient webClient;

            try
            {
                using (webClient = new WebClient())
                {
                    foreach (string email in emailsList)
                    {
                        LabelError.Visible = false;
                        string result = webClient.DownloadString(string.Format("{0}usr={1}&pwd={2}&check={3}", apiUrl, apiUsername, apiPassword, email));
                        JObject objJSON = default(JObject);
                        objJSON = JObject.Parse(result);

                        //if (objJSON["limit_status"] != null)
                        emailsVerified = "<strong>Emails Verified: </strong>" + (string.Format(objJSON["emails_verified"].ToString()));

                        //if (objJSON["limit_desc"] != null)
                        emailsLimit = "<strong>Emails limit: </strong>" + (string.Format(objJSON["emails_limit"].ToString()));

                        if (String.Format(objJSON["emails_limit"].ToString()) == String.Format(objJSON["emails_verified"].ToString()))
                        {
                            LabelLimitStatus.Text = emailsVerified;
                            LabelLimitDesc.Text = emailsLimit;

                            GVVerifyEmail.DataSource = null;
                            GVVerifyEmail.DataBind();
                            return;
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

                            }


                            Variable.variable += 7;
                            Variable.counter += 1;

                            context.Clients.All.onHitRecorded1(Variable.counter, Variable.variable, email, _value);
                        }
                    }
                    GVVerifyEmail.DataSource = dictResults;
                    GVVerifyEmail.DataBind();

                    LabelLimitStatus.Text = "<strong>Emails Verified: </strong>" + emailsVerified;
                    LabelLimitDesc.Text = "<strong>Emails limit: </strong>" + emailsLimit;
                }
            }


            catch (Exception e)
            {
                LabelError.Text = e.Message.ToString();
            }
        }

        protected void VerifyEmailMailBoxer(List<string> emailsList)
        {
            Variable.variable = 0;

            string accessKey = "59a37991b326fe7201f628ea15f0347d";
            string apiUrl = "https://apilayer.net/api/check?";
            List<MailBoxer> li = new List<MailBoxer>();
            Variable.variable = 0;
            Variable.counter = 0;

            var context = GlobalHost.ConnectionManager.GetHubContext<HitCounterHub>();

            WebClient webClient;
            try
            {
                using (webClient = new WebClient())
                {
                    foreach (string email in emailsList)
                    {
                        string result = webClient.DownloadString(string.Format("{0}access_key={1}&email={2}", apiUrl, accessKey, email));
                        JObject objJSON = default(JObject);
                        objJSON = JObject.Parse(result);

                        if (objJSON["smtp_check"] != null)
                        {
                            MailBoxer mb = new MailBoxer();

                            if (objJSON["email"] != null)
                                mb.email = (string.Format(objJSON["email"].ToString()));

                            if (objJSON["format_valid"] != null)
                                mb.isValid = (string.Format(objJSON["format_valid"].ToString())).ToUpper();

                            li.Add(mb);
                            Variable.variable += 7;
                            Variable.counter += 1;

                            context.Clients.All.onHitRecorded2(Variable.counter, Variable.variable, mb.email, mb.isValid);
                        }
                        else
                        {
                            GVMailBoxer.DataSource = null;
                            GVMailBoxer.DataBind();
                            break;
                        }
                    }
                    GVMailBoxer.DataSource = li;
                    GVMailBoxer.DataBind();
                }
            }
            catch(Exception e)
            {
                LabelError.Text = e.Message.ToString();
            }

        }


        protected List<string> generateEmail(string Name, string Surname, string Domain)
        {
            List<string> list = new List<string>();
            list.Add(Name + "@" + Domain);
            list.Add(Surname + "@" + Domain);
            list.Add(Name + Surname + "@" + Domain);
            list.Add(Name + "." + Surname + "@" + Domain);
            list.Add(Name[0] + Surname + "@" + Domain);
            list.Add(Name[0] + "." + Surname + "@" + Domain);
            list.Add(Name + Surname[0] + "@" + Domain);
            list.Add(Name + "." + Surname[0] + "@" + Domain);
            list.Add(Name[0] + "" + Surname[0] + "@" + Domain);
            list.Add(Surname + Name + "@" + Domain);
            list.Add(Surname + "." + Name + "@" + Domain);
            list.Add(Surname + Name[0] + "@" + Domain);
            list.Add(Surname + "." + Name[0] + "@" + Domain);
            list.Add(Name + "_" + Surname + "@" + Domain);
            list.Add(Name[0] + "_" + Surname + "@" + Domain);
            return list;
        }

        protected void GVVerifyEmail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string status = DataBinder.Eval(e.Row.DataItem, "IsValid").ToString();
            //    if (status == "TRUE")
            //    {
            //        e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            //    }
            //}
        }
    }
}