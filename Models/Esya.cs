using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Esya
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public string EsyaTipi { get; set; }

        public virtual Ev Ev { get; set; }
    }
}
