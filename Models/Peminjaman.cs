using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("peminjaman")]
public class Peminjaman
{
    [Column("peminjaman_id")]
    public string PeminjamanId { get; set; } = string.Empty;

    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [Column("buku_id")]
    public string BukuId { get; set; } = string.Empty;

    [Column("tanggal_pinjam")]
    public DateTime TanggalPinjam { get; set; }

    [Column("tanggal_kembali")]
    public DateTime TanggalKembali { get; set; }

    [Column("status")]
    public string Status { get; set; } = string.Empty;
}
