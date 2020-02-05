using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posrepository
{
    class Program
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Logger.Info(" Info Hello world");
           
             


                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Goodbye cruel world");
            }
        }
    }
}
