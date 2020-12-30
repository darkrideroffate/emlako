using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Ev
    {
        public Ev()
        {
            Esya = new HashSet<Esya>();
            EvKira = new HashSet<EvKira>();
            Fotograf = new HashSet<Fotograf>();
            IlanKoy = new HashSet<IlanKoy>();
            Oda = new HashSet<Oda>();
            Ozellik = new HashSet<Ozellik>();
        }

        public int Id { get; set; }
        public int EvSahibiId { get; set; }
        public string Name { get; set; }
        public string EvTipi { get; set; }
        public int KiraFiyati { get; set; }
        public int AdresId { get; set; }

        public virtual Adres Adres { get; set; }
        public virtual EvSahibi EvSahibi { get; set; }
        public virtual ICollection<Esya> Esya { get; set; }
        public virtual ICollection<EvKira> EvKira { get; set; }
        public virtual ICollection<Fotograf> Fotograf { get; set; }
        public virtual ICollection<IlanKoy> IlanKoy { get; set; }
        public virtual ICollection<Oda> Oda { get; set; }
        public virtual ICollection<Ozellik> Ozellik { get; set; }
    }
}
