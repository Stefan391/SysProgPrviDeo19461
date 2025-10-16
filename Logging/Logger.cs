﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgPrviDeo19461.Logging
{
    public static class Logger
    {
        static object consoleLock = new object();
        public static void Log(string msg)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(msg);
            }
        }

        public static void Error(string msg)
        {
            lock (consoleLock)
            {
                ConsoleColor org = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"ERR: {msg}");

                Console.ForegroundColor = org;
            }
        }
    }
}
