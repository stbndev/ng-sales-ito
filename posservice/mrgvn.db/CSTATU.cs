namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CSTATUS")]
    public partial class CSTATU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSTATU()
        {
            LOSTITEMS = new HashSet<LOSTITEM>();
            PRODUCTENTRIES = new HashSet<PRODUCTENTRy>();
            PRODUCTS = new HashSet<PRODUCT>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(280)]
        public string name { get; set; }

        [Required]
        [StringLength(280)]
        public string maker { get; set; }

        public long create_date { get; set; }

        public long modification_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOSTITEM> LOSTITEMS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTENTRy> PRODUCTENTRIES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCT> PRODUCTS { get; set; }
    }
}
