using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emlakkko
{
    public partial class Fotograf
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public string File { get; set; }

        [Required]
        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile tempPath { get; set; }

        public virtual Ev Ev { get; set; }
    }
}
