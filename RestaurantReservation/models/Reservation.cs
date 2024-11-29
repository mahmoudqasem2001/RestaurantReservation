using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }

    public int? CustomerId { get; set; }

    public int? RestaurantId { get; set; }

    public int? TableId { get; set; }

    [Required]
    public DateTime ReservationDate { get; set; }

    [Required]
    public int PartySize { get; set; }

    public Customer Customer { get; set; }
    public Restaurant Restaurant { get; set; }
    public Table Table { get; set; }

    public ICollection<Order> Orders { get; set; }
}
