using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SMB.eServe.DocPulse.Models;
using System.Xml.Linq;
using SMB.eServe.DocPulse.Services; 
using System.IO;

namespace SMB.eServe.DocPulse.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public Action<string>? OnLogMessage;
        private ConversionSettings _settings;
        private FileSystemWatcher _watcher;
        private readonly Dictionary<string, int> _retryTracker = new();

        public void ApplySettings(ConversionSettings settings)
        {
            _settings = settings;
            StartWatching();
        }

        public void StartWatching()
        {
            _watcher?.Dispose();

            // ✅ Check if SourceFolder is null or empty
            if (string.IsNullOrWhiteSpace(_settings.SourceFolder) || !Directory.Exists(_settings.SourceFolder))
            {
                OnLogMessage?.Invoke($"❌ Cannot start watching: invalid SourceFolder path '{_settings.SourceFolder}'");
                return;
            }

            _watcher = new FileSystemWatcher(_settings.SourceFolder, "*.xml")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                EnableRaisingEvents = true
            };

            _watcher.Created += async (s, e) =>
            {
                await Task.Delay(1000);
                ConvertXmlToPdf(e.FullPath);
            };
        }


        public void ConvertXmlToPdf(string xmlFilePath)
        {
            try
            {
                var doc = XDocument.Load(xmlFilePath);
                string poNumber = doc.Descendants("orderNumber").FirstOrDefault()?.Value ?? "Unknown";

                var items = doc.Descendants("POrderDetail").Select(x => new
                {
                    SKU = x.Element("purchaserItemCode")?.Value,
                    Description = x.Element("supplierDescription")?.Value,
                    UOM = x.Element("supplierUOM")?.Value,
                    Qty = x.Element("quantityOrdered")?.Value,
                    UnitPrice = x.Element("unitPrice")?.Value,
                    Total = x.Element("extendedPriceGross")?.Value
                }).ToList();

                string pdfPath = Path.Combine(_settings.DestinationFolder, $"{poNumber}.pdf");

                PdfDocument pdf = new PdfDocument();
                var page = pdf.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 10);
                var headerFont = new XFont("Arial", 14, XFontStyle.Bold);

                double y = 40;
                gfx.DrawString($"Purchase Order No: {poNumber}", headerFont, XBrushes.Black, new XRect(40, y, page.Width, 20));
                y += 30;

                foreach (var item in items)
                {
                    string line = $"{item.SKU} | {item.Description} | {item.UOM} | Qty: {item.Qty} | Price: {item.UnitPrice} | Total: {item.Total}";
                    gfx.DrawString(line, font, XBrushes.Black, new XRect(40, y, page.Width, 20));
                    y += 20;
                    if (y > page.Height - 40)
                    {
                        page = pdf.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 40;
                    }
                }

                pdf.Save(pdfPath);
                pdf.Close();

                // Optional: Upload via SFTP
                if (_settings.UseSftp)
                {
                    var sftp = new SftpService(_settings.SftpHost, _settings.SftpUsername, _settings.SftpPassword, _settings.SftpPort);
                    sftp.UploadFile(pdfPath, Path.Combine(_settings.SftpRemotePath, Path.GetFileName(pdfPath)));
                }

                _retryTracker.Remove(xmlFilePath);
                LogConverted(xmlFilePath, pdfPath);
            }
            catch (Exception ex)
            {
                if (!_retryTracker.ContainsKey(xmlFilePath))
                    _retryTracker[xmlFilePath] = 1;
                else
                    _retryTracker[xmlFilePath]++;

                if (_retryTracker[xmlFilePath] >= _settings.MaxFailureAttempts)
                {
                    string failedPath = Path.Combine(_settings.FailedFolder, Path.GetFileName(xmlFilePath));
                    File.Move(xmlFilePath, failedPath, true);
                    LogFailed(xmlFilePath, $"Max retries reached. Moved to: {failedPath}");
                }
                else
                {
                    LogFailed(xmlFilePath, $"Attempt {_retryTracker[xmlFilePath]}: {ex.Message}");
                }
            }
        }

        private void LogConverted(string xmlPath, string pdfPath)
        {
            Directory.CreateDirectory("logs");
            string entry = $"{DateTime.Now} | SUCCESS | {Path.GetFileName(xmlPath)} -> {Path.GetFileName(pdfPath)}";
            File.AppendAllText("logs/converted.txt", entry + Environment.NewLine);
            OnLogMessage?.Invoke(entry);
        }

        private void LogFailed(string xmlPath, string message)
        {
            Directory.CreateDirectory("logs");
            string entry = $"{DateTime.Now} | FAILED | {Path.GetFileName(xmlPath)} | {message}";
            File.AppendAllText("logs/failed.txt", entry + Environment.NewLine);
            OnLogMessage?.Invoke(entry);
        }

    }
}