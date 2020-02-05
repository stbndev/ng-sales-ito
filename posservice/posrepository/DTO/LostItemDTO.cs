using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posrepository.DTO
{
    public class LostItemDTO
    {
        public int id { get; set; }
        public int idcstatus { get; set; }
        public long create_date { get; set; }

        public long modification_date { get; set; }
        
        public decimal total { get; set; }
        public string maker { get; set; }

        public List<LostItemDetailsDTO> Itemsdetails { get; set; } = new List<LostItemDetailsDTO>();

        
    }

    public class LostItemDetailsDTO 
    {
        public int id { get; set; }

        public decimal unitary_cost { get; set; }
        public int quantity { get; set; }

        public int idproducts { get; set; }

    }
}
