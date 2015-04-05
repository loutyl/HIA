using HIA_client_lourd.Class;
using NUnit.Framework;

namespace HIA_client_lourd.Asserts
{
    [TestFixture]
    public class BonDeVisiteTest
    {

        private BonDeVisite _bonDeVisiteTest;

        [SetUp]
        public void Init()
        {
           _bonDeVisiteTest = new BonDeVisite("05/04/2015", "10:00:00", "12:00:00", "F123456"); 
        }

        [Test]
        public void TestBonDeVisiteInitialisation()
        {
            Assert.IsInstanceOf<BonDeVisite>(_bonDeVisiteTest);
            Assert.IsNotNull(_bonDeVisiteTest);
        }

        [Test]
        public void TestBonDeVisiteValues()
        {
            Assert.AreEqual(_bonDeVisiteTest.DateBonVisite, "05/04/2015");
            Assert.AreEqual(_bonDeVisiteTest.HeureBonDVisite, "10:00:00");
            Assert.AreEqual(_bonDeVisiteTest.HeureBonFVisite, "12:00:00");
            Assert.AreEqual(_bonDeVisiteTest.NumBonVisite, "F123456");
        }
    }
}
