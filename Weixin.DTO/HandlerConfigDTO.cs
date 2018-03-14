using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class HandlerConfigDTO
    {
        public long Id { get; set; }

        [Required]
        public string AppId { get; set; }

        public string ClassName { get; set; }
        [Required]
        public string HandlerName { get; set; }
    }
}
