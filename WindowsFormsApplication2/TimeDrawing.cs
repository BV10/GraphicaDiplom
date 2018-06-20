using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    static class TimeDrawing
    {
        private static uint dotesPerSecond;

        public static uint DotesPerSecond
        {
            get
            {
                return dotesPerSecond;
            }
            set
            {
                dotesPerSecond = value;

                MilliSecForDote = 1000 / dotesPerSecond; // quant millisec on dote
            }
        }

        //private static TimeSpan passedTime; // pass time from beg draw
        private static Stopwatch stopWatch = new Stopwatch(); // for measure time

        private static long MilliSecForDote { get; set; }


        public static void BegDraw()
        {           
            stopWatch.Start();
        }

        public static void EndDraw()
        {
            stopWatch.Stop();
            if(stopWatch.ElapsedMilliseconds < MilliSecForDote) // fast draw -> sleep
            {
                Thread.Sleep((int)(MilliSecForDote - stopWatch.ElapsedMilliseconds));
            }

            stopWatch.Reset();
        }
    }
}
