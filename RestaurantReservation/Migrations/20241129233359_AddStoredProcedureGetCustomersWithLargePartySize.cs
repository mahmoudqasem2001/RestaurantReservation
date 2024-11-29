using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedureGetCustomersWithLargePartySize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE PROCEDURE GetCustomersWithLargePartySize
        @partySize INT
        AS
        BEGIN
            SELECT DISTINCT c.CustomerId, c.FirstName, c.LastName, c.Email, c.PhoneNumber
            FROM Customers c
            JOIN Reservations r ON c.CustomerId = r.CustomerId
            WHERE r.PartySize > @partySize;
        END;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetCustomersWithLargePartySize;");
        }
    }
}
