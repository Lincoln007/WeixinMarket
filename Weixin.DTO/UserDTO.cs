using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(9,ErrorMessage ="最长为9个字符")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^(13[0-9]|14[579]|15[0-3,5-9]|17[0135678]|18[0-9])\\d{8}$")]
        public string PhoneNum { get; set; }
    }
}
