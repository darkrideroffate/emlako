using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class User
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string KisiTuru { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
