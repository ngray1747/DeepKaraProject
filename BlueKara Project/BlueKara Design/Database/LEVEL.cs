//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlueKara_Design.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class LEVEL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LEVEL()
        {
            this.PLAYERs = new HashSet<PLAYER>();
        }
    
        public string LevelID { get; set; }
        public Nullable<int> MaxScore { get; set; }
        public string Name { get; set; }
        public Nullable<int> RemainScoreToUp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PLAYER> PLAYERs { get; set; }
    }
}
