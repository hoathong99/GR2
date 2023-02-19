using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StuManSys.Areas.Identity.Pages.Account.Data
{
    public class AccountInfo : IdentityUser
    {
        [PersonalData]
        public String UID { get; set; }
    }
}
