using System.ComponentModel.DataAnnotations;

namespace Berber.ViewModels
{


    public class GirisYapViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı gereklidir.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Parola gereklidir.")]
        [DataType(DataType.Password)]
        public string Parola { get; set; }

        public bool BeniHatirla { get; set; }
    }
}
