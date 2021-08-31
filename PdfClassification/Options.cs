using System.Collections.Generic;

namespace PdfClassification
{
    public class Options
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public LogicalOperation LogicalOperation { get; set; }
        public TransferType TransferType { get; set; }
        public List<string> Keywords { get; set; }
    }
    public enum LogicalOperation
    {
        Or = 0,
        And = 1

    }
    public enum TransferType
    {
        Copy = 1,
        Move = 2
    }

}
