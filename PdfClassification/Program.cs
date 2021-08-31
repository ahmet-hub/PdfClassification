using BitMiracle.Docotic.Pdf;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace PdfClassification
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = ConfigurationBuilder();

            var files = Directory.GetFiles(options.SourcePath, "*.pdf");

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files.GetValue(i).ToString();

                Classification(filePath, options.DestinationPath, options.LogicalOperation, options.TransferType, options.Keywords);
            }

        }
        static bool IsExist(string filePath, LogicalOperation logicalOperation, List<string> keywords)
        {
            bool result = false;

            PdfDocument pdf = new PdfDocument(filePath);

            for (int i = 0; i < pdf.Pages.Count; i++)
            {
                string pageText = pdf.Pages[i].GetText();

                foreach (var item in keywords)
                {
                    int index = pageText.IndexOf(item, 0, StringComparison.OrdinalIgnoreCase);

                    if (logicalOperation == LogicalOperation.Or) //  veya ||
                    {
                        if (index != -1) result = true;
                    }

                    if (logicalOperation == LogicalOperation.And) // ve && 
                    {
                        if (index == -1) result = false;
                    }
                }

            }
            pdf.Dispose();
            return result;
        }
        static void Classification(string filePath, string destinationPath, LogicalOperation logicalOperation, TransferType transferType, List<string> keywords)
        {

            var fileName = Path.GetFileName(filePath);
            if (IsExist(filePath, logicalOperation, keywords))
                FileTransfer(fileName, $"{destinationPath}{fileName}", transferType);
        }

        static void FileTransfer(string sourceFile, string destinationFile, TransferType transferType)
        {
            if (transferType == TransferType.Move)
                File.Move(sourceFile, destinationFile);
            if (transferType == TransferType.Copy)
                File.Copy(sourceFile, destinationFile);

        }
        private static Options ConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder()
                         .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var options = config.Get<Options>();
            return options;
        }
    }
}
