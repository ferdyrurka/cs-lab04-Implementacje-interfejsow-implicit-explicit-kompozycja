using System;

namespace Zadanie3
{
    public class Printer : BaseDevice, IPrinter
    {
        public int PrintCounter { get; private set; } = 0;

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
    }
}
