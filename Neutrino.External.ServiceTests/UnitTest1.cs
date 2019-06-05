using System;
using System.Collections.Generic;
using AutoMapper;
using Espresso.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
            Assert.AreEqual(startDate, new DateTime(2018, 9, 23));
            Assert.AreEqual(endDate, new DateTime(2018, 10, 22));
        }


        struct MyStruct
        {
            public string Name;
            public int Id;
        }

        [TestMethod]
        public void IntersectTwoList()
        {
            //Arrange


            var lst1 = new List<MyStruct>
            {
                new MyStruct { Name = "A", Id = 10 }
                ,new MyStruct { Name = "B", Id = 11 }
                , new MyStruct { Name = "C", Id = 12 }
             };


            var lst2 = new List<MyStruct>{new MyStruct { Name = "A", Id = 10 }
            , new MyStruct { Name = "D", Id = 11 }
            , new MyStruct { Name = "B", Id = 11 } };

            var intersect = lst1.Intersect(lst2).ToList();
            var recordA = intersect.FirstOrDefault(x => x.Id == 10);
            recordA.Name = "Reza";
        }

    }
}
