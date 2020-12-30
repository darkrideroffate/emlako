using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class EvKira
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public int EvId { get; set; }
        public int KiraciId { get; set; }
        public int KiraFiyati { get; set; }
        public string Sure { get; set; }

        public virtual Ev Ev { get; set; }
        public virtual Kiraci Kiraci { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
