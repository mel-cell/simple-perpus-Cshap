using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("penerbit")]
public class Penerbit
{
    [Column("penerbit_id")]
    public string PenerbitId { get; set; } = string.Empty;

    [Column("penerbit_nama")]
    public string PenerbitNama { get; set; } = string.Empty;

    [Column("penerbit_alamat")]
    public string PenerbitAlamat { get; set; } = string.Empty;

    [Column("penerbit_email")]
    public string PenerbitEmail { get; set; } = string.Empty;

    [Column("penerbit_notelp")]
    public string PenerbitNotelp { get; set; } = string.Empty;
}
