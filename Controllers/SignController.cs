using Microsoft.AspNetCore.Mvc;
using votek.Data; // DbContext'i buradan kullanacağız
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace votek.Controllers
{
    public class SignController : Controller
    {
        private readonly DataContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        // Dependency Injection ile DbContext ve PasswordHasher'ı alıyoruz
        public SignController(DataContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public IActionResult Login()
        {
            return View();
        }

        // Login işlemi için POST metodu
        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanında email'e göre kullanıcıyı buluyoruz
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null)
                {
                    // Eğer kullanıcı bulunamazsa, hatayı bildiriyoruz
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }

                // Şifreyi doğruluyoruz
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    // Eğer şifre doğrulandıysa, login başarılı
                    ViewBag.Message = "Login successful!";
                    return RedirectToAction("Index", "Home"); // Başarılıysa anasayfaya yönlendiriyoruz
                }
                else
                {
                    // Şifre yanlışsa
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }

            // Model valid değilse veya giriş başarısızsa aynı sayfaya dön
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Register işlemi için POST metodu
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password cannot be null or empty.");
                    return View(model);
                }

                try
                {
                    // Şifreyi hash'liyoruz
                    var hashedPassword = _passwordHasher.HashPassword(model, model.Password);
                    model.Password = hashedPassword; // Şifreyi hashlenmiş haliyle modelde güncelliyoruz

                    // Kullanıcıyı veritabanına ekliyoruz
                    _context.Users.Add(model);
                    await _context.SaveChangesAsync();

                    // Başarılı kayıt işleminden sonra login sayfasına yönlendirme
                    return RedirectToAction("Login", "Sign");
                }
                catch (Exception ex)
                {
                    // Hata durumunda loglama ve kullanıcıya bilgi verme
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the user. Please try again.");
                    Console.WriteLine(ex.Message);
                }
            }

            // Eğer model valid değilse veya hata oluşursa, Register sayfasına geri dön
            return View(model);
        }
    }
}
