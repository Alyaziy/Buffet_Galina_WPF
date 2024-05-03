using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffet_Galina_WPF.DTO
{
    public partial class OrderDTO
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<DishDTO> DishDTOs { get; set; } = new();
    }
}
