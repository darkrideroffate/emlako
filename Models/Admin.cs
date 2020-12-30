using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Admin
    {
        public int Id { get; set; }

        public virtual User IdNavigation { get; set; }
    }
}
