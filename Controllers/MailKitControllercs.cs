using Aspose.Pdf;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using Traffic_Violation.Models;

namespace Traffic_Violation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailKitControllercs : ControllerBase
    {
        private readonly ModelContext _context;

        public MailKitControllercs(ModelContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public IActionResult SendlInvoice(Models.ViolationJoin violation)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.Subject = "Invoice";

            var bodyBuilder = new BodyBuilder();

            string path = "files/invoice.pdf";

            Aspose.Pdf.Document document = new Aspose.Pdf.Document();
            Page page = document.Pages.Add();
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n\n\n           ID: {violation.violation.Violationid}     Type: {violation.violationType.Name}    Fine: {violation.violationType.Fine} jod\n"));
            document.Save("files/Invoice.pdf");

            //using (FileStream fs = System.IO.File.Create(path))
            //{
            //    AddText(fs, $"\n\n\nID: {violation.violation.Violationid}     Type: {violation.violationType.Name}    Fine: {violation.violationType.Fine} jod\n");
            //}

            byte[] myFile = System.IO.File.ReadAllBytes(path);

            
            email.Body = bodyBuilder.Attachments.Add("invoice.pdf", myFile);

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("norma.rowe86@ethereal.email", "wA47b16CqryrwSMvAe");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction("ViolationManagement", "UserDashboard");



        }

        [HttpPost]
        public IActionResult SendEmail(string name, string userEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.Subject = subject;
            string body = $"A question from {name}. \n\n{message}";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = body };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("norma.rowe86@ethereal.email", "wA47b16CqryrwSMvAe");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction("Index", "Home");



        }

        public void SendLicense(ProjectLicense license)
        {
            var email = new MimeMessage();
            var user = _context.ProjectUsers.FirstOrDefault(x => x.Userid == license.Userid);
            email.From.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.Subject = "License";
            string body = $"Dear {user.Fname} {user.Lname}. \n\nYour license expires today ({license.Expdate} make sure to renew it)";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = body };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("norma.rowe86@ethereal.email", "wA47b16CqryrwSMvAe");
            smtp.Send(email);
            smtp.Disconnect(true);

        }

        [HttpPost]
        public IActionResult SendlInvoiceMultiple(List<ProjectViolation> violations, int? Userid)
        {


            var user = _context.ProjectUsers.FirstOrDefault(x => x.Userid == Userid);
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("norma.rowe86@ethereal.email"));
            email.Subject = "Invoice";

            var bodyBuilder = new BodyBuilder();

            string fileName = "invoice" +  Guid.NewGuid().ToString() + ".pdf";
            string path = "files/" + fileName;

            Aspose.Pdf.Document document = new Aspose.Pdf.Document();
            Page page = document.Pages.Add();
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Invoice"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n\n\n           User: {user.Fname} {user.Lname}"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n           Email: {user.Email}"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n           Phone: {user.Phonenumber}\n"));


            decimal fineTotal = 0;
            foreach (var item in violations)
            {
                var fullViolation = ViolationJoinReturn().FirstOrDefault(x => x.violation.Violationid == item.Violationid);
                fineTotal += _context.ProjectViolationTypes.FirstOrDefault(x => x.Violationtypeid == fullViolation.violationType.Violationtypeid).Fine;
                page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n           ID: {fullViolation.violation.Violationid}     Type: {fullViolation.violationType.Name}    Fine: {fullViolation.violationType.Fine} jod"));

            }
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n          Total Fine: {fineTotal} Jod"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"\n          Total Violations: {violations.Count()}"));

            document.Save(path);

            byte[] myFile = System.IO.File.ReadAllBytes(path);


            email.Body = bodyBuilder.Attachments.Add("Invoice.pdf", myFile);

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("norma.rowe86@ethereal.email", "wA47b16CqryrwSMvAe");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction("ViolationManagement", "UserDashboard");



        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        private IEnumerable<ViolationJoin> ViolationJoinReturn()
        {
            var violations = _context.ProjectViolations.ToList();
            var violationTypes = _context.ProjectViolationTypes.ToList();
            var violaionStates = _context.ProjectViolationStates.ToList();
            var cars = _context.ProjectCars.ToList();



            var result = from v in violations
                         join vt in violationTypes on v.Violationtypeid equals vt.Violationtypeid
                         join vs in violaionStates on v.Stateid equals vs.Stateid
                         join c in cars on v.Carid equals c.Carid
                         select new ViolationJoin { car = c, violation = v, violationType = vt, violationState = vs };

            return result;
        }
    }
}
