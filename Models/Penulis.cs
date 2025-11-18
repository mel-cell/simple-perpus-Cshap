using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("penulis")]
public class Penulis
{
    [Column("penulis_id")]
    public string PenulisId { get; set; } = string.Empty;

    [Column("penulis_nama")]
    public string PenulisNama { get; set; } = string.Empty;

    [Column("penulis_alamat")]
    public string PenulisAlamat { get; set; } = string.Empty;

    [Column("penulis_email")]
    public string PenulisEmail { get; set; } = string.Empty;

    [Column("penulis_notelp")]
    public string PenulisNotelp { get; set; } = string.Empty;
}
