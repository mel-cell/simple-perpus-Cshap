using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("buku")]
public class Buku
{
    [Column("buku_id")]
    public string BukuId { get; set; } = string.Empty;

    [Column("buku_judul")]
    public string BukuJudul { get; set; } = string.Empty;

    [Column("buku_penulis_id")]
    public string BukuPenulis { get; set; } = string.Empty;

    [Column("buku_penerbit_id")]
    public string BukuPenerbit { get; set; } = string.Empty;

    [Column("buku_tahun")]
    public string BukuTahun { get; set; } = string.Empty;

    [Column("buku_kategori_id")]
    public string BukuKategori { get; set; } = string.Empty;

    [Column("buku_rak_id")]
    public string BukuRak { get; set; } = string.Empty;

    [Column("buku_stok")]
    public int BukuStok { get; set; }
}
