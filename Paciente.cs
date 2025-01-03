using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Models;

public partial class Paciente
{
    [Key]
    [Column("pacienteID")]
    public int PacienteId { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    [StringLength(50)]
    public string Apellido { get; set; } = null!;

    [Column("fechaNacimiento")]
    public DateOnly? FechaNacimiento { get; set; }

    [Column("direccion")]
    [StringLength(100)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [StringLength(15)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("especialidad")]
    [StringLength(20)]
    public string? Especialidad { get; set; }

    [InverseProperty("Paciente")]
    public virtual ICollection<Examen> Examen { get; set; } = new List<Examen>();

    [InverseProperty("Paciente")]
    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
