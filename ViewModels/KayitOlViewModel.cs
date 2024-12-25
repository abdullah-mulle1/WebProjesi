using System.ComponentModel.DataAnnotations;

namespace Berber.ViewModels
{
    public class KayitOlViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı gereklidir.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Eposta { get; set; }


        [Required(ErrorMessage = "Parola gereklidir.")]
        [DataType(DataType.Password)]
        public string Parola { get; set; }

        [Required(ErrorMessage = "Parola Onay gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("Parola", ErrorMessage = "Parolalar eşleşmiyor.")]
        public string ParolaOnay { get; set; }
    }
}

