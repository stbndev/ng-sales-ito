namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("INVENTORYSHRINKAGE")]
    public partial class INVENTORYSHRINKAGE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INVENTORYSHRINKAGE()
        {
            INVENTORYSHRINKAGEDETAILS = new HashSet<INVENTORYSHRINKAGEDETAIL>();
        }

        public int id { get; set; }

        public int idcstatus { get; set; }

        public decimal total { get; set; }

        public long create_date { get; set; }

        public long modification_date { get; set; }

        [StringLength(280)]
        public string maker { get; set; }

        public virtual CSTATU CSTATU { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVENTORYSHRINKAGEDETAIL> INVENTORYSHRINKAGEDETAILS { get; set; }
    }
}
