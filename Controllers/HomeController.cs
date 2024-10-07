using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using votek.Models;
using System.Net.Mail;

namespace votek.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Blog()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult SendEmail(ContactModel model){
        if (ModelState.IsValid)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("ortamali0031@gmail.com");
                mail.From = new MailAddress(model.Email);
                mail.Body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage:\n{model.Message}";
                mail.Subject = model.Subject;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";  // SMTP sunucusu
                smtp.Port = 587; // SSL ALINCA 465 YAPACAZ
                smtp.Credentials = new System.Net.NetworkCredential("ortamali0031@gmail.com", "ubun rjil zran swng");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                TempData["Message"] = "Mesajınız başarıyla gönderildi!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }
                  return RedirectToAction("Contact");
        }
        return View("Contact", model);
    }
    public IActionResult DetailPost()
    {
        return View();
    }

    public IActionResult Projects()
    {
        return View();
    }

    public IActionResult Styleguide()
    {
        return View();
    }

    public IActionResult Team()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
