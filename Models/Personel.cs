using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Personel
    {
        public Personel()
        {
            EvKira = new HashSet<EvKira>();
        }

        public int Id { get; set; }
        public int OfisId { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual Ofis Ofis { get; set; }
        public virtual ICollection<EvKira> EvKira { get; set; }
    }
}
