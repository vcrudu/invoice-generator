using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

public class PdfService
{
    public byte[] GenerateInvoicePdf(Request request)
    {
        var document = new InvoiceDocument(request);
        return document.GeneratePdf();
    }
}
