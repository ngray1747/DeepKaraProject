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
    
    public partial class VIDEOOFFLINE
    {
        public string LinkVideo { get; set; }
        public string VideoID { get; set; }
    
        public virtual CHUNG_VIDEOKARAOKE CHUNG_VIDEOKARAOKE { get; set; }
    }
}
