using System.ComponentModel.DataAnnotations;

namespace ECommerceSite.Models
{
    public class LogUser
    {
        [Display(Name="Email:")]
        public string LogEmail {get; set;}

        [Display(Name="Password:")]
        public string LogPassword {get; set;}
    }
}