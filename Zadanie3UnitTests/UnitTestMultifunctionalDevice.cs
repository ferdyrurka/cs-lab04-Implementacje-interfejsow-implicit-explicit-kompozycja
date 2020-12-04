using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zadanie3;
using System;

namespace Zadanie3UnitTests
{
    [TestClass]
    public class UnitTestMultifunctionalDevice
    {
        [TestMethod]
        public void MultifunctionalDevice_GetState_StateOff()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOff();

            Assert.AreEqual(IDevice.State.off, multifunctionalDevice.GetState()); 
        }

        [TestMethod]
        public void MultifunctionalDevice_GetState_StateOn()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            Assert.AreEqual(IDevice.State.on, multifunctionalDevice.GetState());
        }

        [TestMethod]
        public void MultifunctionalDevice_Print_DeviceOn()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using( var consoleOutput = new ConsoleRedirectionToStringWriter() )
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multifunctionalDevice.Print(in doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);   
        }

        [TestMethod]
        public void MultifunctionalDevice_Print_DeviceOff()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multifunctionalDevice.Print(in doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultifunctionalDevice_Scan_DeviceOff()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multifunctionalDevice.Scan(out doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultifunctionalDevice_Scan_DeviceOn()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multifunctionalDevice.Scan(out doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultifunctionalDevice_Scan_FormatTypeDocument()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multifunctionalDevice.Scan(out doc1, formatType: IDocument.FormatType.JPG);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".jpg"));

                multifunctionalDevice.Scan(out doc1, formatType: IDocument.FormatType.TXT);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".txt"));

                multifunctionalDevice.Scan(out doc1, formatType: IDocument.FormatType.PDF);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultifunctionalDevice_Send_DeviceOn()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument docFax = new PDFDocument("faaax.pdf");

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                multifunctionalDevice.Send(in docFax);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Send fax"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains("faaax.pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultifunctionalDevice_Send_DeviceOff()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOff();

            IDocument docFax = new PDFDocument("faaax.pdf");

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                multifunctionalDevice.Send(in docFax);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Send fax"));
                Assert.IsFalse(consoleOutput.GetOutput().Contains("faaax.pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultifunctionalDevice_PrintCounter()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument docFax1 = new PDFDocument("faaax.pdf");
            IDocument docFax2 = new PDFDocument("faaax.pdf");
            IDocument doc1 = new PDFDocument("aaa.pdf");
            multifunctionalDevice.Print(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            multifunctionalDevice.Print(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            multifunctionalDevice.Send(in docFax1);
            multifunctionalDevice.Send(in docFax2);

            Assert.AreEqual(3, multifunctionalDevice.PrintCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_ScanCounter()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument doc1;
            multifunctionalDevice.Scan(out doc1);
            IDocument doc2;
            multifunctionalDevice.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            IDocument docFax1 = new PDFDocument("faaax.pdf");
            IDocument docFax2 = new PDFDocument("faaax.pdf");

            multifunctionalDevice.Send(in docFax1);
            multifunctionalDevice.Send(in docFax2);

            Assert.AreEqual(2, multifunctionalDevice.ScanCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_SendCounter()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument doc1;
            multifunctionalDevice.Scan(out doc1);
            IDocument doc2;
            multifunctionalDevice.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            IDocument docFax1 = new PDFDocument("faaax.pdf");
            IDocument docFax2 = new PDFDocument("faaax.pdf");

            multifunctionalDevice.Send(in docFax1);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Send(in docFax2);
            multifunctionalDevice.PowerOn();

            multifunctionalDevice.Send(in docFax2);

            Assert.AreEqual(2, multifunctionalDevice.SendCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_PowerOnCounter()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();
            multifunctionalDevice.PowerOn();
            multifunctionalDevice.PowerOn();

            IDocument doc1;
            multifunctionalDevice.Scan(out doc1);
            IDocument doc2;
            multifunctionalDevice.Scan(out doc2);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.PowerOff();
            multifunctionalDevice.PowerOff();
            multifunctionalDevice.PowerOn();

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            IDocument docFax1 = new PDFDocument("faaax.pdf");
            IDocument docFax2 = new PDFDocument("faaax.pdf");

            multifunctionalDevice.Send(in docFax1);
            multifunctionalDevice.Send(in docFax2);

            Assert.AreEqual(3, multifunctionalDevice.Counter);
        }

        [TestMethod]
        public void MultifunctionalDevice_ScannerDisabled()
        {
            Scanner scanner = new Scanner();
            scanner.PowerOff();
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), scanner);
            multifunctionalDevice.PowerOn();

            IDocument doc1;
            multifunctionalDevice.Scan(out doc1);
            IDocument doc2;
            multifunctionalDevice.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            // 4 skany, gdy urządzenie włączone
            Assert.AreEqual(2, multifunctionalDevice.ScanCounter);
            Assert.AreEqual(2, scanner.ScanCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_PrintDisabled()
        {
            Printer printer = new Printer();
            printer.PowerOff();
            var multifunctionalDevice = new MultifunctionalDevice(printer, new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multifunctionalDevice.Print(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            multifunctionalDevice.Print(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            // 5 wydruków, gdy urządzenie włączone
            Assert.AreEqual(3, multifunctionalDevice.PrintCounter);
            Assert.AreEqual(3, printer.PrintCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_SendDisabled()
        {
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multifunctionalDevice.Send(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            multifunctionalDevice.Send(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Send(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Send(in doc3);
            multifunctionalDevice.PowerOn();

            // 5 wydruków, gdy urządzenie włączone
            Assert.AreEqual(3, multifunctionalDevice.SendCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_ScannerEnabled()
        {
            Scanner scanner = new Scanner();
            scanner.PowerOn();
            var multifunctionalDevice = new MultifunctionalDevice(new Printer(), scanner);
            multifunctionalDevice.PowerOn();

            IDocument doc1;
            multifunctionalDevice.Scan(out doc1);
            IDocument doc2;
            multifunctionalDevice.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            // 4 skany, gdy urządzenie włączone
            Assert.AreEqual(2, multifunctionalDevice.ScanCounter);
            Assert.AreEqual(2, scanner.ScanCounter);
        }

        [TestMethod]
        public void MultifunctionalDevice_PrintEnabled()
        {
            Printer printer = new Printer();
            printer.PowerOn();
            var multifunctionalDevice = new MultifunctionalDevice(printer, new Scanner());
            multifunctionalDevice.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multifunctionalDevice.Print(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            multifunctionalDevice.Print(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            multifunctionalDevice.Print(in doc3);

            multifunctionalDevice.PowerOff();
            multifunctionalDevice.Print(in doc3);
            multifunctionalDevice.Scan(out doc1);
            multifunctionalDevice.PowerOn();

            // 5 wydruków, gdy urządzenie włączone
            Assert.AreEqual(3, multifunctionalDevice.PrintCounter);
            Assert.AreEqual(3, printer.PrintCounter);
        }
    }
}
