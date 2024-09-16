using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class InvoiceDocument : IDocument
{
    public Request Model { get; }

    public InvoiceDocument(Request model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
            
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                    
                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
    
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Invoice #{Model.OrderId}").Style(titleStyle);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.OrderConfirmedDate:d}");
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.OrderConfirmedDate:d}");
                });
            });

            row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    void ComposeContent(IContainer container)
    {
         container.PaddingVertical(40).Column(column => 
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", new Address("Nothwind Traders", "1234 Main Street", "Redmond", "WA", "USA")));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("For",  new Address(Model)));
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.Items.Sum(x => x.GrossPrice);
            column.Item().AlignRight().Text($"Grand total: {totalPrice}{CurrencyCodeMapper.GetSymbol(Model.CurrencyName)}").FontSize(14);
        });
    }

 void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            // step 1
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(45);
                columns.ConstantColumn(140);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });
            
            // step 2
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Product");
                header.Cell().Element(CellStyle).Text("Item");
                header.Cell().Element(CellStyle).Text("Month");
                header.Cell().Element(CellStyle).Text("Year");
                header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });
            
            // step 3
            foreach (var item in Model.Items)
            {
                table.Cell().Element(CellStyle).Text(item.OrderItemId.ToString());
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).Text(item.ItemDescription);
                table.Cell().Element(CellStyle).Text(item.MonthName);
                table.Cell().Element(CellStyle).Text(item.Year.ToString());
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.NetPrice}{CurrencyCodeMapper.GetSymbol(Model.CurrencyName)}");    
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(2);
                }
            }
        });
    }
}