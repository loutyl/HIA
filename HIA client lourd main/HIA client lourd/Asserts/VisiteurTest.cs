using HIA_client_lourd.Class;
using NUnit.Framework;

namespace HIA_client_lourd.Asserts
{   
    [TestFixture]
    public class VisiteurTest
    {
        private Visiteur _visiteurTest1;
        private Visiteur _visiteurTest2;

        [SetUp]
        public void Init()
        {
            _visiteurTest1 = new Visiteur("1", "nomVisiteur1","prenomVisiteur1", "emailVisiteur1", "F123456");
            _visiteurTest2 = new Visiteur("2", "nomVisiteur2","prenomVisiteur2", "emailVisiteur2", "01 02 03 04 05", "F654321");
        }

        [Test]
        public void TestVisiteurInitialisation()
        {
            Assert.IsInstanceOf<Visiteur>(_visiteurTest1);
            Assert.IsNotNull(_visiteurTest1);

            Assert.IsInstanceOf<Visiteur>(_visiteurTest2);
            Assert.IsNotNull(_visiteurTest2);
        }

        [Test]
        public void TestVisiteursValues()
        {
            Assert.AreEqual(_visiteurTest1.IdVisiteur, "1");
            Assert.AreEqual(_visiteurTest1.NomVisiteur, "nomVisiteur1");
            Assert.AreEqual(_visiteurTest1.PrenVisiteur, "prenomVisiteur1");
            Assert.IsNull(_visiteurTest1.TelVisiteur);
            Assert.AreEqual(_visiteurTest1.EmailVisiteur, "emailVisiteur1");
            Assert.AreEqual(_visiteurTest1.NumVisiteVisiteur, "F123456");

            Assert.AreEqual(_visiteurTest2.IdVisiteur, "2");
            Assert.AreEqual(_visiteurTest2.NomVisiteur, "nomVisiteur2");
            Assert.AreEqual(_visiteurTest2.PrenVisiteur, "prenomVisiteur2");
            Assert.AreEqual(_visiteurTest2.EmailVisiteur, "emailVisiteur2");
            Assert.AreEqual(_visiteurTest2.TelVisiteur, "01 02 03 04 05");
            Assert.AreEqual(_visiteurTest2.NumVisiteVisiteur, "F654321");
        }

    }
}
