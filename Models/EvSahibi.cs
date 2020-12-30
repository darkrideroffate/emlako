using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class EvSahibi
    {
        public EvSahibi()
        {
            Ev = new HashSet<Ev>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }

        public virtual ICollection<Ev> Ev { get; set; }
    }
}
