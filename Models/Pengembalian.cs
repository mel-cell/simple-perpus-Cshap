using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("pengembalian")]
public class Pengembalian
{
    [Column("pengembalian_id")]
    public string PengembalianId { get; set; } = string.Empty;

    [Column("peminjaman_id")]
    public string PeminjamanId { get; set; } = string.Empty;

    [Column("tanggal_pengembalian")]
    public DateTime TanggalPengembalian { get; set; }

    [Column("kondisi_buku")]
    public string KondisiBuku { get; set; } = string.Empty;

    [Column("denda")]
    public decimal Denda { get; set; }
}
