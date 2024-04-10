using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffet_Galina_WPF.DTO
{
    public partial class CategoryDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
