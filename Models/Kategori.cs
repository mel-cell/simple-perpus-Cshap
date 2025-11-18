using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("kategori")]
public class Kategori
{
    [Column("kategori_id")]
    public string KategoriId { get; set; } = string.Empty;

    [Column("kategori_nama")]
    public string KategoriNama { get; set; } = string.Empty;

    [Column("kategori_deskripsi")]
    public string KategoriDeskripsi { get; set; } = string.Empty;
}
