using System;
using System.Collections.Generic;

namespace RAS.BOOTCAMP.TEMPLATE.Datas.Entities
{
    public partial class Barang
    {
        public Barang()
        {
            ItemTransaksis = new HashSet<ItemTransaksi>();
            Keranjangs = new HashSet<Keranjang>();
        }

        public int IdBarang { get; set; }
        public int Code { get; set; }
        public string? Nama { get; set; }
        public string? Description { get; set; }
        public int Harga { get; set; }
        public int Stok { get; set; }
        public int IdPenjual { get; set; }
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;

        public virtual Penjual IdPenjualNavigation { get; set; } = null!;
        public virtual ICollection<ItemTransaksi> ItemTransaksis { get; set; }
        public virtual ICollection<Keranjang> Keranjangs { get; set; }
    }
}
