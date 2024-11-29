using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Table
{
    [Key]
    public int TableId { get; set; }

    public int? RestaurantId { get; set; }

    [Required]
    public int Capacity { get; set; }

    public Restaurant Restaurant { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}
