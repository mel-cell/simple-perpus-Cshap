using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("siswa")]
public class Siswa
{
    [Column("siswa_id")]
    public string SiswaId { get; set; } = string.Empty;

    [Column("siswa_nama")]
    public string SiswaNama { get; set; } = string.Empty;

    [Column("siswa_kelas")]
    public string SiswaKelas { get; set; } = string.Empty;

    [Column("siswa_alamat")]
    public string SiswaAlamat { get; set; } = string.Empty;

    [Column("siswa_email")]
    public string SiswaEmail { get; set; } = string.Empty;

    [Column("siswa_notelp")]
    public string SiswaNotelp { get; set; } = string.Empty;
}
