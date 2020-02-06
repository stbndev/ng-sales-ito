namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCTS")]
    public partial class PRODUCT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCT()
        {
            LOSTITEMDETAILS = new HashSet<LOSTITEMDETAIL>();
            PRODUCTENTRYDETAILS = new HashSet<PRODUCTENTRYDETAIL>();
            SALEDETAILS = new HashSet<SALEDETAIL>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(280)]
        public string name { get; set; }

        [Required]
        [StringLength(280)]
        public string barcode { get; set; }

        public int idcstatus { get; set; }

        public decimal unitary_price { get; set; }

        public decimal unitary_cost { get; set; }

        public int existence { get; set; }

        public virtual CSTATU CSTATU { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOSTITEMDETAIL> LOSTITEMDETAILS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTENTRYDETAIL> PRODUCTENTRYDETAILS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SALEDETAIL> SALEDETAILS { get; set; }
    }
}
