using Microsoft.AspNetCore.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WebFerreteria.Models;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace WebFerreteria.Controllers
{
    public class ReportesController : Controller
    {
        private readonly FerreteriaDbContext _context;

        public ReportesController(FerreteriaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método para generar y descargar el reporte PDF
        public IActionResult GenerarReportePDF()
        {
            // Obtener productos desde la base de datos
            List<Productos> productos = _context.Productos.ToList();

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Agregar título
                var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
                var title = new Paragraph("Reporte de Productos - Ferretería La Puntilla", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph("\n")); // salto de línea

                // Crear tabla con 3 columnas
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3f, 1f, 1f });

                // Encabezados
                table.AddCell("Nombre");
                table.AddCell("Cantidad");
                table.AddCell("Precio");

                // Agregar filas
                foreach (var producto in productos)
                {
                    table.AddCell(producto.Nombre ?? "");
                    table.AddCell(producto.Cantidad?.ToString() ?? "0");
                    table.AddCell(producto.Precio?.ToString("C2") ?? "$0.00");
                }

                document.Add(table);
                document.Close();

                byte[] byteInfo = ms.ToArray();

                // Devolver archivo PDF para descargar
                return File(byteInfo, "application/pdf", "ReporteProductos.pdf");
            }
        }
    }
}
