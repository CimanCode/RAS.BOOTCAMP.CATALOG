using System;
using System.Collections.Generic;

namespace RAS.BOOTCAMP.TEMPLATE.Datas.Entities
{
    public partial class User
    {
        public User()
        {
            Keranjangs = new HashSet<Keranjang>();
            Pembelies = new HashSet<Pembely>();
            Penjuals = new HashSet<Penjual>();
            Transaksis = new HashSet<Transaksi>();
        }

        public int IdUser { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int NoHandPhone { get; set; }
        public string? Tipe { get; set; }

        public virtual ICollection<Keranjang> Keranjangs { get; set; }
        public virtual ICollection<Pembely> Pembelies { get; set; }
        public virtual ICollection<Penjual> Penjuals { get; set; }
        public virtual ICollection<Transaksi> Transaksis { get; set; }
    }
}
