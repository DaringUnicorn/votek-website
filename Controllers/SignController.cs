using Microsoft.AspNetCore.Mvc;
using votek.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using votek.Models; // ViewModel'leri buradan kullanacağız
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace votek.Controllers
{
    public class SignController : Controller
    {
        private readonly DataContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;

        // Dependency Injection ile DbContext ve PasswordHasher'ı alıyoruz
        public SignController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login işlemi için POST metodu
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı email ve şifre ile sorguluyoruz
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if (isUser != null)
                {
                    var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()),
                new Claim(ClaimTypes.Name, isUser.Name ?? ""),
                new Claim(ClaimTypes.Email, isUser.Email ?? "")
            };

                    // Kullanıcı giriş bilgilerini ve özelliklerini tanımlıyoruz
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    // Önce mevcut oturumu kapatıyoruz ve ardından yeni oturum açıyoruz
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    // Giriş başarılı, yönlendirme yapıyoruz (index sayfasına yönlendirme yapabilirsiniz)
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            // Model geçerli değilse veya giriş başarısızsa aynı sayfaya geri dönüyoruz
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
            else
            {
                // ModelState geçerli değilse hataları kullanıcıya bildiririz
                foreach (var modelStateError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelStateError.ErrorMessage); // Hataları console'da yazdır
                }
            }

            // Eğer model valid değilse veya hata oluşursa, Register sayfasına geri dön
            return View(model);
        }
    }
}
