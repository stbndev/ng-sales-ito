namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCTENTRYDETAILS")]
    public partial class PRODUCTENTRYDETAIL
    {
        public int id { get; set; }

        public int idproductentries { get; set; }

        public decimal unitary_cost { get; set; }

        public int quantity { get; set; }

        public int idproducts { get; set; }

        public virtual PRODUCTENTRy PRODUCTENTRy { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }
    }
}
