using Syncfusion.Licensing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Redaction;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Syncfusion.Drawing;

namespace UnableToAllocatePixels
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();


			// Load PDF
			var fs1 = File.OpenRead(@"../../../Data/manual.pdf");
			using PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fs1, true);
			loadedDocument.EnableMemoryOptimization = true;

			//PDF Font erstellen aus Textline zur Ersetzung
			var pdfFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

			for (int i = 0; i < loadedDocument.Pages.Count; i++)
			{
				PdfLoadedPage currentPage = loadedDocument.Pages[i] as PdfLoadedPage;

				loadedDocument.FindText("<PAGENUMBER>", i, out List<RectangleF> matchRect);
				foreach (var rect in matchRect)
				{
					PdfRedaction redaction = new PdfRedaction(rect);
					currentPage.AddRedaction(redaction);
				}
			}
			loadedDocument.Redact();

			using var fs2 = File.OpenWrite("output.pdf");
			loadedDocument.Save(fs2);

			TimeSpan ts = stopWatch.Elapsed;
			// Format and display the TimeSpan value.
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				ts.Hours, ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			stopWatch.Stop();
			Console.WriteLine("RunTime:" + elapsedTime);
		}
	}
}
