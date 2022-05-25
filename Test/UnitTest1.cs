using NUnit.Framework;
using ServerLib.Database.Mysql.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Share.Structure;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Test
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            uint a = 10;

            uint b = 20;

            try
            {
                Console.WriteLine(a - b);

            }
            catch (Exception ex)
            {

                Console.WriteLine("122 : " + ex);
            }
        }
    }
}