namespace mrgvn.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCTENTRIES")]
    public partial class PRODUCTENTRy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCTENTRy()
        {
            PRODUCTENTRYDETAILS = new HashSet<PRODUCTENTRYDETAIL>();
        }

        public int id { get; set; }

        public decimal total { get; set; }

        public long create_date { get; set; }

        public int idcstatus { get; set; }

        public virtual CSTATU CSTATU { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTENTRYDETAIL> PRODUCTENTRYDETAILS { get; set; }
    }
}
