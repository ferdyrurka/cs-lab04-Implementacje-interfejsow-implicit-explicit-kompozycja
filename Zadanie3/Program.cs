using System;

namespace Zadanie3
{
    class Program
    {
        static void Main(string[] args)
        {
            var copier = new Copier(new Printer(), new Scanner());
            copier.PowerOn();
            IDocument doc1 = new PDFDocument("aaa_copier.pdf");
            copier.Print(in doc1);

            IDocument doc2;
            copier.Scan(out doc2);

            copier.ScanAndPrint();
            System.Console.WriteLine(copier.Counter);
            System.Console.WriteLine(copier.PrintCounter);
            System.Console.WriteLine(copier.ScanCounter);

            var multifunctional = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctional.PowerOn();
            IDocument doc3 = new PDFDocument("aaa_multi.pdf");
            multifunctional.Print(in doc3);

            IDocument doc4;
            multifunctional.Scan(out doc4);

            multifunctional.Send(doc1);
            System.Console.WriteLine(multifunctional.Counter);
            System.Console.WriteLine(multifunctional.PrintCounter);
            System.Console.WriteLine(multifunctional.ScanCounter);
            System.Console.WriteLine(multifunctional.SendCounter);
        }
    }
}
