using System;

namespace Zadanie2
{
    public interface IDevice
    {
        enum State {on, off};

        void PowerOn(); // uruchamia urządzenie, zmienia stan na `on`
        void PowerOff(); // wyłącza urządzenie, zmienia stan na `off
        State GetState(); // zwraca aktualny stan urządzenia

        int Counter {get;}  // zwraca liczbę charakteryzującą eksploatację urządzenia,
                            // np. liczbę uruchomień, liczbę wydrukow, liczbę skanów, ...
    }

    public abstract class BaseDevice : IDevice
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        public void PowerOff()
        {
            state = IDevice.State.off;
            Console.WriteLine("... Device is off !");
        }

        public void PowerOn()
        {
            state = IDevice.State.on;
            Console.WriteLine("Device is on ...");  
        }

        public int Counter { get; private set; } = 0;
    }

    public interface IPrinter : IDevice
    {
        /// <summary>
        /// Dokument jest drukowany, jeśli urządzenie włączone. W przeciwnym przypadku nic się nie wykonuje
        /// </summary>
        /// <param name="document">obiekt typu IDocument, różny od `null`</param>
        void Print(in IDocument document);
    }

    public interface IScanner : IDevice
    {
        // dokument jest skanowany, jeśli urządzenie włączone
        // w przeciwnym przypadku nic się dzieje
        void Scan(out IDocument document, IDocument.FormatType formatType);
    }

    public interface ICopier : IDevice, IPrinter, IScanner
    {
        int PrintCounter { get; }

        int ScanCounter { get; }

        void ScanAndPrint();
    }

    public interface IFax : IDevice
    {
        void Send(in IDocument document);
    }

    public class Copier : BaseDevice, ICopier
    {
        public int PrintCounter { get; private set; } = 0;

        public int ScanCounter { get; private set; } = 0;

        public new int Counter { get; private set; } = 0;

        public void Print(in IDocument document)
        {
            if (GetState() == IDevice.State.off)
            {
                return;
            }

            ++PrintCounter;

            Console.WriteLine(
                String.Format(
                    "{0} Print: {1}",
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                    document.GetFileName()
                )
            );
        }

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

        public void ScanAndPrint()
        {
            IDocument document;

            Scan(out document, IDocument.FormatType.JPG);
            Print(document);
        }

        public void PowerOn()
        {
            if (GetState() == IDevice.State.off)
            {
                ++Counter;
            }

            base.PowerOn();
        }
    }
}
