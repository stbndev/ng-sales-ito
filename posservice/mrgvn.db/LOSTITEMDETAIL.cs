namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOSTITEMDETAILS")]
    public partial class LOSTITEMDETAIL
    {
        public int id { get; set; }

        public int idlostitems { get; set; }

        public decimal unitary_cost { get; set; }

        public int idproducts { get; set; }

        public int quantity { get; set; }

        public virtual LOSTITEM LOSTITEM { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }
    }
}
