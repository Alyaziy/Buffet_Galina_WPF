using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffet_Galina_WPF.DTO
{
    public partial class AdminDTO
    {
        public int Id { get; set; }

        public string? Password { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
