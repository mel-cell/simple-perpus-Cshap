using System;

namespace pr.Models;

public class Buku
{
    public string BukuId { get; set; } = string.Empty;
    public string BukuJudul { get; set; } = string.Empty;
    public string BukuPenulis { get; set; } = string.Empty;
    public string BukuPenerbit { get; set; } = string.Empty;
    public string BukuTahun { get; set; } = string.Empty;
    public string BukuKategori { get; set; } = string.Empty;
    public string BukuRak { get; set; } = string.Empty;
    public int BukuStok { get; set; }
}
