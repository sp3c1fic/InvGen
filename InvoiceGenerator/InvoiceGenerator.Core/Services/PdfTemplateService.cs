using QuestPDF.Fluent;
using QuestPDF.Helpers;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using InvoiceGenerator.InvoiceGenerator.Core.Contracts;

public class PdfTemplateService : IPdfTemplateService
{
    public byte[] GenerateInvoicePdf(Invoice invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Helvetica").FontColor(Colors.Black));

                // Stark header with dramatic contrast
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        // Company section - left aligned, bold hierarchy
                        row.RelativeItem().Column(companyCol =>
                        {
                            companyCol.Item().Text(invoice.CompanyName)
                                .FontSize(28)
                                .Bold()
                                .FontColor(Colors.Black);

                            companyCol.Item().PaddingTop(12).Column(detailsCol =>
                            {
                                detailsCol.Item().Text(invoice.CompanyAddress)
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken1)
                                    .LineHeight(1.4f);

                                detailsCol.Item().PaddingTop(8).Row(contactRow =>
                                {
                                    contactRow.AutoItem().Text(invoice.CompanyPhone)
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Darken1);

                                    contactRow.AutoItem().PaddingLeft(20).Text(invoice.CompanyEmail)
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Darken1);
                                });

                                if (!string.IsNullOrWhiteSpace(invoice.TaxId))
                                {
                                    detailsCol.Item().PaddingTop(4).Text($"Tax ID: {invoice.TaxId}")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Medium);
                                }
                            });
                        });

                        //// Logo and Invoice title - right aligned
                        //row.ConstantItem(200).Column(titleCol =>
                        //{
                        //    // Logo space
                        //    if (!string.IsNullOrWhiteSpace(invoice.LogoPath) && File.Exists(invoice.LogoPath))
                        //    {
                        //        titleCol.Item().Height(50).Width(140).AlignRight()
                        //            .Image(invoice.LogoPath)
                        //            .FitArea();
                        //    }

                        //    titleCol.Item().PaddingTop(15).AlignRight().Text("INVOICE")
                        //        .FontSize(42)
                        //        .Bold()
                        //        .FontColor(Colors.Black);
                        //});
                    });

                    // Separator line - sharp and minimal
                    col.Item().PaddingTop(25).Height(2).Background(Colors.Black);
                });

                page.Content().PaddingVertical(30).Column(col =>
                {
                    // Invoice metadata and client info - clean grid
                    col.Item().Row(row =>
                    {
                        // Invoice details - left column
                        row.RelativeItem().PaddingRight(40).Column(metaCol =>
                        {
                            metaCol.Item().Text("INVOICE DETAILS")
                                .FontSize(11)
                                .Bold()
                                .FontColor(Colors.Black);

                            metaCol.Item().PaddingTop(15).Column(detailsCol =>
                            {
                                detailsCol.Item().Row(detailRow =>
                                {
                                    detailRow.ConstantItem(80).Text("Number")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken2);
                                    detailRow.RelativeItem().Text(invoice.InvoiceNumber)
                                        .FontSize(9)
                                        .Bold()
                                        .FontColor(Colors.Black);
                                });

                                detailsCol.Item().PaddingTop(8).Row(detailRow =>
                                {
                                    detailRow.ConstantItem(80).Text("Issue Date")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken2);
                                    detailRow.RelativeItem().Text(invoice.IssueDate.ToString("MMMM dd, yyyy"))
                                        .FontSize(9)
                                        .FontColor(Colors.Black);
                                });

                                detailsCol.Item().PaddingTop(8).Row(detailRow =>
                                {
                                    detailRow.ConstantItem(80).Text("Due Date")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken2);
                                    detailRow.RelativeItem().Text(invoice.DueDate.ToString("MMMM dd, yyyy"))
                                        .FontSize(9)
                                        .Bold()
                                        .FontColor(Colors.Black);
                                });
                            });
                        });

                        // Client info - right column
                        row.RelativeItem().Column(clientCol =>
                        {
                            clientCol.Item().Text("BILL TO")
                                .FontSize(11)
                                .Bold()
                                .FontColor(Colors.Black);

                            clientCol.Item().PaddingTop(15).Column(clientDetailsCol =>
                            {
                                clientDetailsCol.Item().Text(invoice.ClientName)
                                    .FontSize(12)
                                    .Bold()
                                    .FontColor(Colors.Black);

                                clientDetailsCol.Item().PaddingTop(8).Text(invoice.ClientAddress)
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken1)
                                    .LineHeight(1.4f);

                                clientDetailsCol.Item().PaddingTop(8).Text(invoice.ClientPhone)
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken1);

                                clientDetailsCol.Item().PaddingTop(4).Text(invoice.ClientEmail)
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken1);
                            });
                        });
                    });

                    // Items table - brutally clean design
                    col.Item().PaddingTop(40).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(90);
                        });

                        // Sharp, minimal table header
                        table.Header(header =>
                        {
                            header.Cell()
                                .BorderBottom(1)
                                .BorderColor(Colors.Black)
                                .PaddingBottom(12)
                                .Text("DESCRIPTION")
                                .FontSize(10)
                                .Bold()
                                .FontColor(Colors.Black);

                            header.Cell()
                                .BorderBottom(1)
                                .BorderColor(Colors.Black)
                                .PaddingBottom(12)
                                .AlignCenter()
                                .Text("QTY")
                                .FontSize(10)
                                .Bold()
                                .FontColor(Colors.Black);

                            header.Cell()
                                .BorderBottom(1)
                                .BorderColor(Colors.Black)
                                .PaddingBottom(12)
                                .AlignRight()
                                .Text("UNIT PRICE")
                                .FontSize(10)
                                .Bold()
                                .FontColor(Colors.Black);

                            header.Cell()
                                .BorderBottom(1)
                                .BorderColor(Colors.Black)
                                .PaddingBottom(12)
                                .AlignRight()
                                .Text("TOTAL")
                                .FontSize(10)
                                .Bold()
                                .FontColor(Colors.Black);
                        });

                        // Clean data rows with subtle separators
                        foreach (var item in invoice.InvoiceItems)
                        {
                            table.Cell()
                                .PaddingVertical(12)
                                .BorderBottom(0.5f)
                                .BorderColor(Colors.Grey.Lighten1)
                                .Text(item.Description)
                                .FontSize(10)
                                .FontColor(Colors.Black);

                            table.Cell()
                                .PaddingVertical(12)
                                .BorderBottom(0.5f)
                                .BorderColor(Colors.Grey.Lighten1)
                                .AlignCenter()
                                .Text(item.Quantity.ToString())
                                .FontSize(10)
                                .FontColor(Colors.Black);

                            table.Cell()
                                .PaddingVertical(12)
                                .BorderBottom(0.5f)
                                .BorderColor(Colors.Grey.Lighten1)
                                .AlignRight()
                                .Text($"{item.Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);

                            table.Cell()
                                .PaddingVertical(12)
                                .BorderBottom(0.5f)
                                .BorderColor(Colors.Grey.Lighten1)
                                .AlignRight()
                                .Text($"{(item.Price * item.Quantity).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))}")
                                .FontSize(10)
                                .Bold()
                                .FontColor(Colors.Black);
                        }

                        // Calculation section - clean and prominent
                        var subtotal = invoice.InvoiceItems.Sum(x => x.Price * x.Quantity);
                        var taxAmount = invoice.Total - subtotal;

                        // Spacer
                        table.Cell().ColumnSpan(4).PaddingVertical(15);

                        // Subtotal
                        table.Cell().ColumnSpan(3)
                            .AlignRight()
                            .Text("SUBTOTAL")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                        table.Cell()
                            .AlignRight()
                            .Text($"{subtotal.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))}")
                            .FontSize(12)
                            .FontColor(Colors.Black);

                        // Tax (if applicable)
                        if (taxAmount > 0)
                        {
                            table.Cell().ColumnSpan(3)
                                .AlignRight()
                                .PaddingTop(8)
                                .Text("TAX")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);

                            table.Cell()
                                .AlignRight()
                                .PaddingTop(8)
                                .Text($"{taxAmount.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))}")
                                .FontSize(12)
                                .FontColor(Colors.Black);
                        }

                        // Total - dramatic emphasis
                        table.Cell().ColumnSpan(3)
                            .AlignRight()
                            .PaddingTop(15)
                            .BorderTop(2)
                            .BorderColor(Colors.Black)
                            .PaddingVertical(15)
                            .Text("TOTAL DUE")
                            .FontSize(12)
                            .Bold()
                            .FontColor(Colors.Black);

                        table.Cell()
                            .AlignRight()
                            .PaddingTop(15)
                            .BorderTop(2)
                            .BorderColor(Colors.Black)
                            .PaddingVertical(15)
                            .Text($"{invoice.Total.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))}")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Black);
                    });

                    // Payment terms - sophisticated footer section
                    col.Item().PaddingTop(40).Column(termsCol =>
                    {
                        termsCol.Item().Text("PAYMENT TERMS")
                            .FontSize(10)
                            .Bold()
                            .FontColor(Colors.Black);

                        termsCol.Item().PaddingTop(10).Text("Payment is due within thirty (30) days of invoice date. Late payments are subject to 1.5% monthly service charge. All disputes must be reported within 10 days of invoice date.")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1)
                            .LineHeight(1.5f);
                    });
                });

                // Minimal, elegant footer
                page.Footer().Column(footerCol =>
                {
                    footerCol.Item().Height(1).Background(Colors.Grey.Lighten1);

                    footerCol.Item().PaddingTop(15).Row(footerRow =>
                    {
                        footerRow.RelativeItem().Text("Thank you for your business.")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Medium);

                        footerRow.AutoItem().Text("Page 1 of 1")
                            .FontSize(8)
                            .FontColor(Colors.Grey.Lighten2);
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}