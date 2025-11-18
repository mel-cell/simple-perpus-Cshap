using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pr.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [Column("user_name")]
    public string UserName { get; set; } = string.Empty;

    [Column("user_alamat")]
    public string UserAlamat { get; set; } = string.Empty;

    [Required]
    [Column("user_username")]
    public string UserUsername { get; set; } = string.Empty;

    [Required]
    [Column("user_email")]
    public string UserEmail { get; set; } = string.Empty;

    [Column("user_notelp")]
    public string UserNotelp { get; set; } = string.Empty;

    [Column("user_password")]
    public string UserPassword { get; set; } = string.Empty;

    [Column("user_level")]
    public UserRole UserLevel { get; set; } = UserRole.anggota;
}
