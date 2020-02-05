namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SALEDETAILS")]
    public partial class SALEDETAIL
    {
        public int id { get; set; }

        public int idsales { get; set; }

        public decimal unitary_cost { get; set; }

        public decimal unitary_price { get; set; }

        public int quantity { get; set; }

        public int idproducts { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }

        public virtual SALE SALE { get; set; }
    }
}
