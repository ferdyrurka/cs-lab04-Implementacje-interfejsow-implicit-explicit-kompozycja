using System;
namespace Zadanie3
{
    public class Scanner : BaseDevice, IScanner
    {
        public int ScanCounter { get; private set; } = 0;

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            if (GetState() == IDevice.State.off)
            {
                document = null;
                return;
            }

            ++ScanCounter;

            switch (formatType)
            {
                case IDocument.FormatType.JPG:
                    document = new ImageDocument(String.Format("ImageScan{0}.jpg", ScanCounter));
                    break;
                case IDocument.FormatType.PDF:
                    document = new PDFDocument(String.Format("PDFScan{0}.pdf", ScanCounter));
                    break;
                case IDocument.FormatType.TXT:
                    document = new TextDocument(String.Format("TextScan{0}.txt", ScanCounter));
                    break;
                default:
                    throw new ArgumentException("Undefined file type!");
            }

            Console.WriteLine(
                String.Format(
                    "{0} Scan: {1}",
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                    document.GetFileName()
                )
            );
        }
    }
}
