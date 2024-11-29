using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int ReservationId { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [ForeignKey("ReservationId")]
    public Reservation Reservation { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
