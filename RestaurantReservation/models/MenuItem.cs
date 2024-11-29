using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MenuItem
{
    [Key]
    public int ItemId { get; set; }

    [Required]
    public int RestaurantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
