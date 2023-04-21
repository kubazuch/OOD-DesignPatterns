using System;
using System.Collections.Generic;
using System.Linq;
using BTM;
using BTM.Collections;

namespace ConsoleProject
{
    internal class Program
    {

        static void Main(string[] args)
        {
            DataManager manager = new DataManager(DataRepresentation.BASE);
            manager.PrintData();
        }
    }
}
