using System;

namespace Zadanie3
{
    public class MultifunctionalDevice : BaseDevice, IMultifunctionalDevice
    {
        public int PrintCounter { get; private set; } = 0;

        public int ScanCounter { get; private set; } = 0;

        public int SendCounter { get; private set; } = 0;

        private Printer Printer;

        private Scanner Scanner;

        public MultifunctionalDevice(Printer printer, Scanner scanner)
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


        public void Send(in IDocument document)
        {
            if (GetState() == IDevice.State.off)
            {
                return;
            }

            ++SendCounter;

            Console.WriteLine(
                String.Format(
                     "{0} Send fax: {1}",
                     DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                     document.GetFileName()
                )
            );
        }
    }
}
