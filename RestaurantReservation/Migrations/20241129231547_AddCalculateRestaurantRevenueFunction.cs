using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculateRestaurantRevenueFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE FUNCTION CalculateRestaurantRevenue(@restaurantId INT)
        RETURNS DECIMAL(18,2)
        AS
        BEGIN
            RETURN (
                SELECT SUM(o.TotalAmount)
                FROM Orders o
                JOIN Reservations r ON o.ReservationId = r.ReservationId
                WHERE r.RestaurantId = @restaurantId
            );
        END;
    ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS CalculateRestaurantRevenue;");

        }
    }
}
