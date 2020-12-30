using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Ozellik
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public string OzellikTipi { get; set; }

        public virtual Ev Ev { get; set; }
    }
}
