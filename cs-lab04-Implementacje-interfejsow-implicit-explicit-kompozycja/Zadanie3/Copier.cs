using System;
using System.Collections.Generic;
using System.Text;

namespace Zadanie3
{
    public class Copier : BaseDevice, ICopier
    {
        public int PrintCounter { get; private set; } = 0;

        public int ScanCounter { get; private set; } = 0;

        private Printer Printer;

        private Scanner Scanner;

        public Copier(Printer printer, Scanner scanner)
        {
            this.Printer = printer;
            this.Scanner = scanner;
        }

        public void Print(in IDocument document)
        {
            if (this.GetState() == IDevice.State.off)
            {
                return;
            }

            bool toDisabled = false;
            if (this.Printer.GetState() == IDevice.State.off)
            {
                toDisabled = true;
                this.Printer.PowerOn();
            }

            ++this.PrintCounter;
            this.Printer.Print(in document);

            if (toDisabled)
            {
                this.Printer.PowerOff();
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            if (this.GetState() == IDevice.State.off)
            {
                document = null;
                return;
            }

            bool toDisabled = false;
            if (this.Scanner.GetState() == IDevice.State.off)
            {
                toDisabled = true;
                this.Scanner.PowerOn();
            }

            ++this.ScanCounter;
            this.Scanner.Scan(out document, formatType);

            if (toDisabled)
            {
                this.Scanner.PowerOff();
            }
        }

        public void ScanAndPrint()
        {
            IDocument document;

            Scan(out document, IDocument.FormatType.JPG);
            Print(document);
        }
    }
}
