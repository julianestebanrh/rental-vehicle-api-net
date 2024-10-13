using Dapper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Application.Vehicles.ReportVehicle
{
    internal sealed class ReportVehicleQueryHandler : IQueryHandler<ReportVehicleQuery, Document>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ReportVehicleQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<Document>> Handle(ReportVehicleQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();



            var builder = new StringBuilder("""
                SELECT 
                    id AS Id,
                    model AS Model,
                    vin AS Vin,
                    price_amount AS Price,
                    price_currency AS Currency
                FROM vehicles
                """);

            var search = string.Empty;
            var where = string.Empty;
            if (!string.IsNullOrEmpty(request.Model))
            {
                search = "%" + request.Model + "%";
                where = $" WHERE model LIKE @Search";
                builder.AppendLine(where);
            }

            builder.AppendLine(" ORDER BY model ");

            var sql = builder.ToString();
            var vehicles = await connection.QueryAsync<ReportVehicleResponse>(sql,
                new
                {
                    Search = search
                });

            var report = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4.Landscape());
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("List of high-end vehicles")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

                    page.Content()
                        .Padding(25)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Model");
                                header.Cell().Element(CellStyle).Text("Vin");
                                header.Cell().Element(CellStyle).AlignRight().Text("Price");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Black);
                                }
                            });

                            foreach (var vehicle in vehicles)
                            {
                                var price = Math.Round(vehicle.Price, 2);

                                table.Cell().Element(CellStyleBody).Text(vehicle.Model);
                                table.Cell().Element(CellStyleBody).Text(vehicle.Vin);
                                table.Cell().Element(CellStyleBody).AlignRight().Text($"${price} {vehicle.Currency}");

                                static IContainer CellStyleBody(IContainer container)
                                {
                                    return container
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .PaddingVertical(5);
                                }
                            }


                        });

                });
            });

            return report;
        }
    }
}
