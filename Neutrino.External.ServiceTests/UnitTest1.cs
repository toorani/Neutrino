using System;
using System.Collections.Generic;
using AutoMapper;
using Espresso.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neutrino.ServiceTests
{
    [TestClass]
    public class GeneralTests
    {
        class Source
        {
            public int Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        class Destination
        {
            public int Prop1 { get; set; }
            public int Prop2 { get; set; }
        }
        [TestMethod]
        public void ArrayToListMapping()
        {
            //Arrange
            Source[] arr = new Source[]
            {
                new Source() { Prop1 = 1, Prop2 = 2 },
                new Source() { Prop1 = 2, Prop2 = 3 },
                new Source() { Prop1 = 3, Prop2 = 4 }
            };

            var config = new MapperConfiguration(cfg =>
            {
               cfg.CreateMap<Source, Destination>();
            });
            var mapper = config.CreateMapper();

            //Action
            List<Destination> lst = mapper.Map<Source[], List<Destination>>(arr);

            //Assert
            Assert.AreEqual(lst.Count, arr.Length);
        }

        [TestMethod]
        public void Utilities_ToDateTimne_YearMonth()
        {
            //Arrange
            int year = 1397;
            int month = 7;

            //Action
            var startDate = Utilities.ToDateTime(year, month);
            var endDate = Utilities.ToDateTime(year, month, ToDateTimeOptions.EndMonth);

            //Assert
            Assert.AreEqual(startDate, new DateTime(2018,9,23));
            Assert.AreEqual(endDate, new DateTime(2018, 10, 22));
        }

    }
}
