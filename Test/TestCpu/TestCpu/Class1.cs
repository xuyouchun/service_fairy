using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace TestCpu
{
    public class Class1
    {
        public static void Main()
        {
            AppDomain.MonitoringIsEnabled = true;

            for (int k = 0; k < 100; k++)
            {
                _bytes.Add(new byte[1024]);
            }

            Thread thread = new Thread(_Process);
            thread.IsBackground = true;
            thread.Start();

            PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
             
            while (true)
            {
                long memorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
                long processMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
                long totalMemory = GC.GetTotalMemory(false);
                TimeSpan ts = AppDomain.CurrentDomain.MonitoringTotalProcessorTime;

                float cpuLoad = pc.NextValue();

                Thread.Sleep(1000);
                Console.WriteLine("{0} {1} {2} {3} {4}", memorySize, processMemorySize, totalMemory, ts, cpuLoad);
                //GC.Collect();
            }
        }

        private static readonly List<byte[]> _bytes = new List<byte[]>();

        private static void _Process()
        {
            double value = 100;
            int index = 100;
            while (true)
            {
                value += Math.Sin(value);
                _bytes.Add(new byte[index++]);
                _bytes.RemoveAt(0);
            }
        }

    }
}
