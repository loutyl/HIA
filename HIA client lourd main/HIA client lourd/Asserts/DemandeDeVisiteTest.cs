using HIA_client_lourd.Class;
using NUnit.Framework;

namespace HIA_client_lourd.Asserts
{   
    [TestFixture]
    public class DemandeDeVisiteTest
    {

        private Visiteur _visiteurOrigineTest;
        private DemandeDeVisite _demandeDeVisiteTest;

        [SetUp]
        public void Init()
        {
            _visiteurOrigineTest = new Visiteur("1", "nomVisiteur1", "prenomVisiteur1", "emailVisiteur1", "F123456");
            _demandeDeVisiteTest = new DemandeDeVisite(_visiteurOrigineTest,"05/04/2015","10:00:00", "12:00:00");
        }

        [Test]
        public void TestInitialisatonDemandeDeVisite()
        {
            Assert.IsInstanceOf<Visiteur>(_visiteurOrigineTest);
            Assert.IsNotNull(_visiteurOrigineTest);

            Assert.IsInstanceOf<DemandeDeVisite>(_demandeDeVisiteTest);
            Assert.IsNotNull(_demandeDeVisiteTest);
        }

        [Test]
        public void TestDemandeDeVisiteValues()
        {
            Assert.AreEqual(_demandeDeVisiteTest.VisiteurOrigine, _visiteurOrigineTest);
            Assert.AreEqual(_demandeDeVisiteTest.VisiteurOrigine.IdVisiteur, _visiteurOrigineTest.IdVisiteur);
            Assert.AreEqual(_demandeDeVisiteTest.VisiteurOrigine.NomVisiteur, _visiteurOrigineTest.NomVisiteur);
            Assert.AreEqual(_demandeDeVisiteTest.VisiteurOrigine.PrenVisiteur, _visiteurOrigineTest.PrenVisiteur);
            Assert.AreEqual(_demandeDeVisiteTest.VisiteurOrigine.NumVisiteVisiteur, _visiteurOrigineTest.NumVisiteVisiteur);
            
            Assert.AreEqual(_demandeDeVisiteTest.DateVisite, "05/04/2015");
            Assert.AreEqual(_demandeDeVisiteTest.HeureDVisite, "10:00:00");
            Assert.AreEqual(_demandeDeVisiteTest.HeureFVisite, "12:00:00");
        }
    }
}
