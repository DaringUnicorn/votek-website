using Microsoft.AspNetCore.Mvc;
using votek.Data;  // DbContext'i buradan kullanacağız
using Microsoft.AspNetCore.Identity; // Şifre hashleme için gerekli

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

        // Register sayfası için GET metodu
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

                    // Başarılı kayıt işleminden sonra yönlendirme
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Hata durumunda loglama ve kullanıcıya bilgi verme
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the user. Please try again.");
                    // Hatanın loglanması (isteğe bağlı olarak bir logging mekanizması kullanılabilir)
                    Console.WriteLine(ex.Message);
                }
            }

            // Eğer model valid değilse veya hata oluşursa, Register sayfasına geri dön
            return View(model);
        }

    }
}
