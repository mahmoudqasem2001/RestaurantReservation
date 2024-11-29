using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required]
    public int RestaurantId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Position { get; set; }

    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; }

    public ICollection<Order> Orders { get; set; }
}
