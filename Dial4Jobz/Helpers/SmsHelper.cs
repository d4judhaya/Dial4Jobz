using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Dial4Jobz.Models;
using Dial4Jobz.Models.Repositories;

namespace Dial4Jobz.Helpers
{
    public class SmsHelper
    {
        protected static bool smsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["SmsEnabled"]);

        public static void Sendsms(string Userid, string Password, string Message, string Type, string Senderid, string To)
        {

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://www.smsgatewaycenter.com/library/send_sms_2.php?UserName=" + Userid + "&Password=" + Password + "&Type=" + Type + "&To=" + To + "&Mask=" + Senderid + "&Message=" + Message);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }


        public static void SendSecondarySms(string Userid, string Password,string Message, string Type,string Source, string Dlr, string Destination )
        {

            //string Userid = "dial4jobzt";
            //string password = "wldfslkj";
            //string Type = "0";
            //string dlr = "1";
            //string destination = "9710426588";
            //string Message = "Hai, welcome";
            //string source = "UPDATE";


            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://sms6.routesms.com:8080/bulksms/bulksms?username=" + Userid + "&password=" + Password + "&type=" + Type + "&dlr=" + Dlr + "&destination=" + Destination + "&source=" + Source + "&message=" + Message);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();  
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }

    }
}