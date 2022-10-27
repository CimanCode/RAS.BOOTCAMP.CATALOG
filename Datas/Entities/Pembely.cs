using System;
using System.Collections.Generic;

namespace RAS.BOOTCAMP.TEMPLATE.Datas.Entities
{
    public partial class Pembely
    {
        public int IdPembeli { get; set; }
        public int IdUser { get; set; }
        public string? Nama { get; set; }
        public string? NoHandPhone { get; set; }
        public string? Alamat { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
