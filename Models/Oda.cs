using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Oda
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public string OdaTipi { get; set; }

        public virtual Ev Ev { get; set; }
    }
}
