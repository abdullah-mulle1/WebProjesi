using Microsoft.AspNetCore.Identity;

namespace Berber.Models
{
    public class Kullanici : IdentityUser
    {
        // خصائص إضافية يمكن إضافتها للمستخدم
        public string Ad { get; set; }


    }
}
