namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SALES")]
    public partial class SALE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SALE()
        {
            SALEDETAILS = new HashSet<SALEDETAIL>();
        }

        public int id { get; set; }

        public decimal total { get; set; }

        public int idcstatus { get; set; }

        [Required]
        [StringLength(280)]
        public string maker { get; set; }

        public long create_date { get; set; }

        public long modification_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SALEDETAIL> SALEDETAILS { get; set; }
    }
}
