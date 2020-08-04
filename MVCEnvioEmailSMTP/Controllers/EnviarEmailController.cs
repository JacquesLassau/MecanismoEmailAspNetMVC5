using System;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace MVCEnvioEmailSMTP.Controllers
{
    public class EnviarEmailController : Controller
    {
        [HttpGet]
        public ActionResult EnviarEmailUI()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarEmailAR(string txtEmail, string txtAssunto, string txtMensagem, HttpPostedFileBase flAnexo)
        {
            if((txtEmail == null) || (txtAssunto == null) || (txtMensagem == null))
                return RedirectToAction("EnviarEmailUI", "EnviarEmail");

            string rota = Server.MapPath("../Temp");

            try
            {
                using (MailMessage mailMessage = new MailMessage("emailCredenciais@email.com", "emailCopia@email.com"))
                {
                    mailMessage.Subject = txtAssunto;
                    mailMessage.CC.Add(txtEmail);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = txtMensagem;

                    if (flAnexo != null)
                    {
                        flAnexo.SaveAs(rota + "\\" + flAnexo.FileName);
                        Attachment anexo = new Attachment(rota + "\\" + flAnexo.FileName);
                        mailMessage.Attachments.Add(anexo);
                    }

                    /*
                     ** Cada serviço de e-mail contém sua configuração própria:
                     ** ------------------------------------------------------- 
                     ** smtp.gmail.com
                     ** smtp.live.com
                     ** smtp.yahoo.com
                     ** -------------------------------------------------------
                     */

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                                        
                    System.Net.NetworkCredential credencial = new System.Net.NetworkCredential("emailCredenciais@email.com", "5enh@8mai1%redenciais");
                    smtpClient.Credentials = credencial;

                    // Mecanismo que identifica as credencias com certificado SSL
                    System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                    smtpClient.Send(mailMessage);
                }                
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = ex.Message;
            }

            System.IO.File.Delete(rota + "\\" + flAnexo.FileName);
            return RedirectToAction("EnviarEmailUI", "EnviarEmail");

        }
    }
}