using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Tyuiu.KAV.Sprint7.Project.V7.Lib;

namespace Tyuiu.KAV.Sprint7.Project.V7.Test
{
    [TestClass]
    public class DataServiceTest
    {
        private List<ApartmentRecord> _testData;
        private DataService _ds;

        [TestInitialize]
        public void Init()
        {
            _ds = new DataService();
            _testData = new List<ApartmentRecord>
            {
                new ApartmentRecord { EntranceNumber = 1, ApartmentNumber = 10, TotalArea = 50.0, LivingArea = 30.0, RoomsCount = 2, OwnerLastName = "Иванов", RegistrationDate = DateTime.Now, FamilyMembersCount = 3, ChildrenCount = 1, HasDebt = false, Note = "" },
                new ApartmentRecord { EntranceNumber = 1, ApartmentNumber = 11, TotalArea = 70.0, LivingArea = 45.0, RoomsCount = 3, OwnerLastName = "Петров", RegistrationDate = DateTime.Now, FamilyMembersCount = 4, ChildrenCount = 2, HasDebt = true, Note = "Долг за 2 мес." },
                new ApartmentRecord { EntranceNumber = 2, ApartmentNumber = 25, TotalArea = 30.0, LivingArea = 18.0, RoomsCount = 1, OwnerLastName = "Сидоров", RegistrationDate = DateTime.Now, FamilyMembersCount = 1, ChildrenCount = 0, HasDebt = false, Note = "" }
            };
        }

        [TestMethod]
        public void ValidGetTotalApartments()
        {
            Assert.AreEqual(3, _ds.GetTotalApartments(_testData));
        }

        [TestMethod]
        public void ValidGetSumTotalArea()
        {
            Assert.AreEqual(150.0, _ds.GetSumTotalArea(_testData));
        }

        [TestMethod]
        public void ValidGetAverageTotalArea()
        {
            Assert.AreEqual(50.0, _ds.GetAverageTotalArea(_testData));
        }

        [TestMethod]
        public void ValidGetMinAndMaxArea()
        {
            Assert.AreEqual(30.0, _ds.GetMinTotalArea(_testData));
            Assert.AreEqual(70.0, _ds.GetMaxTotalArea(_testData));
        }

        [TestMethod]
        public void ValidGetDebtorsCount()
        {
            Assert.AreEqual(1, _ds.GetDebtorsCount(_testData));
        }
    }
}
