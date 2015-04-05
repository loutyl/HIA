using System.Linq;
using HIA_client_lourd.Class;
using NUnit.Framework;
using databaseHIA;
namespace HIA_client_lourd.Asserts
{
    [TestFixture]
    public class PatientTest
    {
        private Patient _patientTest;

        [SetUp]
        public void Init()
        {
            System.Collections.Generic.List<string> listInfoPatient = new System.Collections.Generic.List<string> { "1", "MAALEM", "Thomas", "21", "2", "150", "Fenêtre", "5668d878", "1" };

            _patientTest = new Patient(listInfoPatient);

        }

        [Test]
        public void TestPatientInialisation()
        {
            Assert.IsInstanceOf<Patient>(_patientTest);
            Assert.IsNotNull(_patientTest);
        }

        [Test]
        public void TestPatientValues()
        {
            Assert.AreEqual(_patientTest.IdPatient, "1");
            Assert.AreEqual(_patientTest.NomPatient, "MAALEM");
            Assert.AreEqual(_patientTest.PrenomPatient, "Thomas");
            Assert.AreEqual(_patientTest.AgePatient, "21");
            Assert.AreEqual(_patientTest.EtagePatient, "2");
            Assert.AreEqual(_patientTest.ChambrePatient, "150");
            Assert.AreEqual(_patientTest.LitPatient, "Fenêtre");
            Assert.AreEqual(_patientTest.NumVisitePatient, "5668d878");
            Assert.AreEqual(_patientTest.StatusVisite, 1);

        }

        [Test]
        public void TestMethodGetDemandeDeVisite()
        {
            var ldb = new LightClientDatabaseObject(System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
            const string debutVisite = "10:00:00";
            const string finVisite = "12:00:00";
            const int idVisiteur = 1;
            const string guid = "F123456";
            const int statusDemande = 3;

            ldb.SendDemandeDeVisite(System.TimeSpan.Parse(debutVisite),
                System.TimeSpan.Parse(finVisite),
                1,
                idVisiteur,
                guid,
                statusDemande);

            var listDemande = _patientTest.GetDemandeDeVisite();
            Assert.IsInstanceOf<System.Collections.Generic.List<DemandeDeVisite>>(listDemande);
            Assert.IsTrue(listDemande.Count == 1);
            Assert.IsTrue(listDemande[0].VisiteurOrigine.IdVisiteur == idVisiteur.ToString());
            Assert.IsTrue(listDemande[0].HeureDVisite == debutVisite);
            Assert.IsTrue(listDemande[0].HeureFVisite == finVisite);
        }
    }
}
