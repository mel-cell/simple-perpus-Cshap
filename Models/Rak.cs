using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("rak")]
public class Rak
{
    [Column("rak_id")]
    public string RakId { get; set; } = string.Empty;

    [Column("rak_nama")]
    public string RakNama { get; set; } = string.Empty;

    [Column("rak_lokasi")]
    public string RakLokasi { get; set; } = string.Empty;
}
