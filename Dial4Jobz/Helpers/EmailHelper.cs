using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Collections;
using System.Net;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using System.Net.Mime;
using System.IO;

namespace Dial4Jobz.Helpers
{
    public class EmailHelper
    {
       
        protected static bool emailEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailEnabled"]);


        public static void SendEmailSBCC(string from, string to, string bcc,string sbcc,string fbcc, string subject, string body)
        {
            var employersupport = Constants.EmailSender.EmployerSupport;
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.ReplyToList.Add(from);
            mailMessage.Bcc.Add(new MailAddress(bcc));
            mailMessage.Bcc.Add(new MailAddress(sbcc));
            //mailMessage.Bcc.Add(new MailAddress(tbcc));
            if (!string.IsNullOrWhiteSpace(fbcc))
            {
                mailMessage.Bcc.Add(new MailAddress(fbcc));
            }

            mailMessage.Sender = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.Default;
            mailMessage.SubjectEncoding = Encoding.Default;
            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = false;
            smtpClient.Host = "smtp.dial4jobz.in";
            if (employersupport == from)
            {
                //smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2009");
                smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2014");
            }
            else
            {
                // smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2009");
                smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2014");
            }
            smtpClient.Port = 25;
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            smtpClient.SendAsync(mailMessage, mailMessage);
        }

        public static void SendEmailBCC(string from, string to, string bcc, string subject, string body)
        {
           
            var employersupport = Constants.EmailSender.EmployerSupport;
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.ReplyToList.Add(from);
            mailMessage.Bcc.Add(new MailAddress(bcc));
            //mailMessage.Bcc.Add(new MailAddress(sbcc));

            mailMessage.Sender = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.Default;
            mailMessage.SubjectEncoding = Encoding.Default;
            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = false;
            smtpClient.Host = "smtp.dial4jobz.in";
            if (employersupport == from)
            {
               //smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2009");
               smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2014");
            }
            else
            {
              // smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2009");
              smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2014");
            }
            smtpClient.Port = 25;
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            smtpClient.SendAsync(mailMessage, mailMessage); 
        }

        public static void SendEmailWithAttachment(string from, string to, string subject, string body, string replyTo, string attachmentFilename)
        {
            var employersupport = Constants.EmailSender.EmployerSupport;
            MailMessage mailMessage = new MailMessage(from, to);
            //mail.ReplyToList.Add("test@test.com");
            mailMessage.ReplyToList.Add(replyTo);
            mailMessage.Sender = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.Default;
            mailMessage.SubjectEncoding = Encoding.Default;
            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = body;

            if (attachmentFilename != null)
            {
                Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                disposition.FileName = Path.GetFileName(attachmentFilename);
                disposition.Size = new FileInfo(attachmentFilename).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                mailMessage.Attachments.Add(attachment);
            }

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = false;
            smtpClient.Host = "smtp.dial4jobz.in";
            if (employersupport == from)
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2014");
            }
            else
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2014");
            }
            smtpClient.Port = 25;
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            smtpClient.SendAsync(mailMessage, mailMessage);
        }
        public static void SendEmailReply(string from, string to, string subject, string body, string replyTo)
        {
            var employersupport = Constants.EmailSender.EmployerSupport;
            MailMessage mailMessage = new MailMessage(from, to);
            //mail.ReplyToList.Add("test@test.com");
            mailMessage.ReplyToList.Add(replyTo);
            mailMessage.Sender = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.Default;
            mailMessage.SubjectEncoding = Encoding.Default;
            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = false;
            smtpClient.Host = "smtp.dial4jobz.in";
            if (employersupport == from)
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2014");
            }
            else
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2014");
            }
            smtpClient.Port = 25;
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            smtpClient.SendAsync(mailMessage, mailMessage);
        }

                
        public static void SendEmail(string from, string to,  string subject, string body )
        {
            //string replyTo="";
            var employersupport = Constants.EmailSender.EmployerSupport;
            MailMessage mailMessage = new MailMessage(from, to);
            //mail.ReplyToList.Add("test@test.com");
            mailMessage.ReplyToList.Add(from);
            mailMessage.Sender = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.Default;
            mailMessage.SubjectEncoding = Encoding.Default;
            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = body;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = false;
                smtpClient.Host = "smtp.dial4jobz.in";
                if (employersupport==from)
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential("resume@dial4jobz.in", "employer2014");
                }
                else
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential("vacancy@dial4jobz.in", "candidate2014");
                }
                smtpClient.Port = 25;
                smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                smtpClient.SendAsync(mailMessage, mailMessage);
        }
    //}

        

        static void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //to be implemented        
        }
    }
}