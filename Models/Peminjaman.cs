using System;

namespace pr.Models;

public class Peminjaman
{
    public string PeminjamanId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string BukuId { get; set; } = string.Empty;
    public DateTime TanggalPinjam { get; set; }
    public DateTime TanggalKembali { get; set; }
    public string Status { get; set; } = string.Empty;
}
