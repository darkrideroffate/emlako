using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Kiraci
    {
        public Kiraci()
        {
            EvKira = new HashSet<EvKira>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public virtual ICollection<EvKira> EvKira { get; set; }
    }
}
