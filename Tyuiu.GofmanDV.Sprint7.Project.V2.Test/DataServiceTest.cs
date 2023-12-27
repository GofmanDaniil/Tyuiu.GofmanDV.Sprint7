using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tyuiu.GofmanDV.Sprint7.Project.V2.Lib;

namespace Tyuiu.GofmanDV.Sprint7.Project.V2.Test
{
    [TestClass]
    public class DataServiceTest
    {
        [TestMethod]
        public void ValidCalc()
        {
            DataService ds = new DataService();

            string row = "Пятерочка";
            int column = 1;
            string res = ds.CollectTextFromFile(row, column);

            string wait = "тел.:8 (800) 555-55-05, почта: info@pyaterochka.ru, головной офис: Россия, Москва, Средняя Калитниковская ул., д. 28, стр. 4.";
            Assert.AreEqual(wait, res);
        }
    }
}
