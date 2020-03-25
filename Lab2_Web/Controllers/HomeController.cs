using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab2_Web.Models;
using Lab2_Web.Interfaces;
using MimeKit;

namespace Lab2_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailClient _emailClient;

        public HomeController(ILogger<HomeController> logger, IEmailClient emailClient)
        {
            _logger = logger;
            _emailClient = emailClient;
        }

        public IActionResult Index()
        {
            if(_emailClient.Username != null && _emailClient.Password != null)
            {
                return RedirectToAction("Inbox");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Login(LoginViewModel model)
        {
            _emailClient.Initialize(model.Email, model.Password);
            return RedirectToAction("Inbox");
        }
        public IActionResult Inbox()
        {
            if(_emailClient.Username == null && _emailClient.Password == null)
            {
                return RedirectToAction("Index");
            }
            var messages = _emailClient.GetInbox();
            return View(messages);
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            if (_emailClient.Username == null && _emailClient.Password == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SendEmail(SendEmailViewModel model)
        {
            _emailClient.SendEmail(model.To, model.Subject, model.Body);
            return View();
        }
        [HttpGet("Home/Message/{id}")]
        public IActionResult Message(int id)
        {
            var msg = _emailClient.GetEmail(id);
            return View(msg);
        }
        public IActionResult Logout()
        {
            _emailClient.Close();
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
