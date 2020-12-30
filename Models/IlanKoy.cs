using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class IlanKoy
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public int OfisId { get; set; }

        public virtual Ev Ev { get; set; }
        public virtual Ofis Ofis { get; set; }
    }
}
