using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;

namespace InvoiceGenerator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly PdfService _pdfService;

    // The directory to save and retrieve uploaded files
    private const string UploadFolder = "uploads";

    [BindProperty]
    public IFormFile JsonFile { get; set; }

    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    public List<string> UploadedFiles { get; set; } = new List<string>();

    public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env, PdfService pdfService)
    {
        _logger = logger;
        _env = env;
        _pdfService = pdfService;
    }

    public void OnGet()
    {
        // Retrieve the list of files from the upload directory
        var uploadPath = Path.Combine(_env.WebRootPath, UploadFolder);

        if (Directory.Exists(uploadPath))
        {
            UploadedFiles = Directory.GetFiles(uploadPath)
                                     .Select(Path.GetFileName)
                                     .ToList();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (JsonFile == null || JsonFile.Length == 0)
        {
            ErrorMessage = "Please upload a valid JSON file.";
            OnGet();
            return Page();
        }

        try
        {
            // Ensure the upload directory exists
            var uploadPath = Path.Combine(_env.WebRootPath, UploadFolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save the uploaded file to the upload directory
            var filePath = Path.Combine(uploadPath, JsonFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await JsonFile.CopyToAsync(stream);
            }

            // Validate the JSON
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var root = await JsonSerializer.DeserializeAsync<Root>(stream);
                if (root == null || root.Requests == null || !root.Requests.Any())
                {
                    ErrorMessage = "The uploaded JSON is invalid or does not contain any requests.";
                    return Page();
                }
            }

            SuccessMessage = "JSON file uploaded and validated successfully.";
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing the JSON file.");
            ErrorMessage = "There was an error processing the JSON file.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            ErrorMessage = "An unexpected error occurred while processing the file.";
        }

        // Refresh the list of files after upload
        OnGet();

        return Page();
    }

    public IActionResult OnPostDownload(string fileName)
    {
        // Retrieve the file from the upload directory
        var uploadPath = Path.Combine(_env.WebRootPath, UploadFolder);
        var filePath = Path.Combine(uploadPath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            ErrorMessage = "File not found.";
            return Page();
        }

        // Deserialize the JSON content into a Request object
        Root document;
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            document = JsonSerializer.Deserialize<Root>(stream);
        }

        // Generate the PDF using PdfService
        var pdfBytes = _pdfService.GenerateInvoicePdf(document.Requests.First());

        // Set the PDF file name based on the original JSON file name
        var pdfFileName = Path.ChangeExtension(fileName, ".pdf");

        // Return the generated PDF as a file download
        return File(pdfBytes, "application/pdf", pdfFileName);
    }
}
