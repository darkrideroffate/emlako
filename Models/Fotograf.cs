using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Fotograf
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public string File { get; set; }

        public virtual Ev Ev { get; set; }
    }
}
