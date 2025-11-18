using System;

namespace pr.Models;

public class Pengembalian
{
    public string PengembalianId { get; set; } = string.Empty;
    public string PeminjamanId { get; set; } = string.Empty;
    public DateTime TanggalPengembalian { get; set; }
    public string KondisiBuku { get; set; } = string.Empty;
    public decimal Denda { get; set; }
}
