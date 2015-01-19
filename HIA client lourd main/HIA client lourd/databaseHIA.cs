using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIA_client_lourd
{
    class databaseHIA
    {
        //Représente la connection avec la base de données
        private SqlConnection dbHIA;
        //Accesseur de la connection à la base de données
        public SqlConnection DbHIA
        {
            get { return dbHIA; }
            set { dbHIA = value; }
        }
        //transaction avec la base de données
        private SqlTransaction dbTransaction;
        //Accesseur
        public SqlTransaction DbTransaction
        {
            get { return dbTransaction; }
            set { dbTransaction = value; }
        }
        //Méthode d'ouverture de la connection
        public void openConnection()
        {
            //dbTransaction = dbHIA.BeginTransaction();
            DbHIA.Open();
        }
        //Méthode de fermeture de la connection
        public void closeConnection()
        {
            //dbTransaction.Commit();
            DbHIA.Close();
        }
        //Méthode appellée lors d'une erreur pendant une requête 
        public void rollbackTransaction()
        {
            //dbTransaction.Rollback();
            closeConnection();
        }
        //Méthode de connection à l'application
        public bool login(string username, string password)
        {
            bool bRet = false;
            //Création d'une requete
            string requete = @"SELECT Count(*) FROM Personnel 
                             WHERE login = @login AND password = @password";
            //Ouverture d'une connection à la base de données

            SqlCommand cmd = new SqlCommand(requete, dbHIA);

            try
                {
                    //Ouverture de la connection
                    openConnection();
                    //Passage par paramêtre des filtres WHERE à la requête
                    cmd.Parameters.AddWithValue("@login", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int result = (int)cmd.ExecuteScalar();
                    //Si le résultat est > à 0 on retourne vrai 
                    if (result > 0)
                    {
                        bRet = true;
                    }
                }
                catch (Exception ex)
                {
                    //Affichage du message d'erreur 
                    throw new ApplicationException(ex.Message);
                }
                finally
                {
                    //Destruction des objets cmd et connection
                    cmd.Dispose();
                    closeConnection();
                }
            return bRet;
            }
        //Méthode de recherche d'un patient
        public Patient recherchePatient(string recherche)
        {
            SqlCommand cmd = dbHIA.CreateCommand();
            //Essaie de recherche
            try
            {
                //Ouverture de la connection
                openConnection();
                //Création de la requête
                cmd.CommandText = "SELECT * FROM LocalisationPatient " +
                    "WHERE nom_patient LIKE @nom_patient OR prenom_patient LIKE @prenom_patient;";
                //Passage par paramêtre des filtres WHERE
                cmd.Parameters.AddWithValue("@nom_patient", "%" + recherche + "%");
                cmd.Parameters.AddWithValue("@prenom_patient", "%" + recherche + "%");
                //Création d'un reader
                SqlDataReader reader = cmd.ExecuteReader();
                //Lecture du reader
                reader.Read();
                //Récupération des données dans les variables
                string IDpatient = reader.GetInt32(0).ToString();
                string nom = reader.GetString(1);
                string prenom = reader.GetString(2);
                string age = reader.GetValue(3).ToString();
                string etage = reader.GetValue(4).ToString();
                string chambre = reader.GetValue(5).ToString();
                string lit = reader.GetString(6);
                string numVisite = reader.GetString(7);
                bool statusVisite = reader.GetBoolean(8);
                //Instanciation d'un nouveau patient
                Patient patient = new Patient(nom, prenom, age, etage, chambre, lit, IDpatient, numVisite, statusVisite);
                return patient;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                closeConnection();
            }
        }
        //Méthode d'ajout d'un visiteur à la pré-liste
        public bool ajoutPrelist(List<string> listInfoPL)
        {
            bool bRet = false;
            //Création d'une requête
            SqlCommand cmd = dbHIA.CreateCommand();

            //Essaie d'ajout à la preliste
            try
            {
                //Ouveture de la connection
                openConnection();
                //Création de la requête 
                cmd.CommandText = "INSERT INTO PreListe VALUES (@PL_nom_visiteur, @PL_prenom_visiteur, @PL_tel_visiteur, @PL_email_visiteur, @id_patient)";
                //Passage par paramêtre des filtres WHERE
                cmd.Parameters.AddWithValue("@PL_nom_visiteur",listInfoPL[0]);
                cmd.Parameters.AddWithValue("@PL_prenom_visiteur", listInfoPL[1]);
                cmd.Parameters.AddWithValue("@PL_tel_visiteur", listInfoPL[2]);
                cmd.Parameters.AddWithValue("@PL_email_visiteur", listInfoPL[3]);
                cmd.Parameters.AddWithValue("@id_patient", listInfoPL[4]);

                //Execution de la requête
                cmd.ExecuteNonQuery();

                bRet = true;

            }
            catch (Exception exe)
            {
                //rollbackTransaction();
                bRet = false;
                //Affichage du message d'erreur
                throw new ApplicationException(exe.Message);

            }
            finally
            {
                //Destruction des objets cmd et connection
                cmd.Dispose();
                closeConnection();
            }

            return bRet;
        }
        //Méthode permettant le débloquage des visite d'un patient
        public bool debloquerVisite(string nomPatient)
        {
            bool bRet = false;
            
            //Création d'une requête 
            SqlCommand cmd = dbHIA.CreateCommand();

            try
            {
                //Ouverture de la connection
                openConnection();
                //Création de la requête
                cmd.CommandText = "UPDATE Patient SET status_visite = '1' WHERE nom_patient = @nomPatient AND status_visite = '0'";
                //Ajout des filtres WHERE par paramêtre
                cmd.Parameters.AddWithValue("@nomPatient", nomPatient);
                //Exécution de la requête
                cmd.ExecuteNonQuery();
                bRet = true;
            }
            catch (Exception ex)
            {
                //rollbackTransaction();
                bRet = false;
                throw new ApplicationException(ex.Message);

            }
            finally
            {
                //Destruction des objets cmd et connection
                cmd.Dispose();
                closeConnection();
            }
            return bRet;
        }
        //Méthode d'obtention des demandes de visite d'un patient
        public List<demandeDeVisite> getDemandeDeVisite(int statusDemande, string nomPatient)
        {
            List<demandeDeVisite> listeDemande = new List<demandeDeVisite>();
            //Création d'une requête
            SqlCommand cmd = dbHIA.CreateCommand();           

            try
            {
                //Ouverture de la connection
                openConnection();
                //Création de la requête
                cmd.CommandText = "SELECT DISTINCT id_visiteur, nom_visiteur, prenom_visiteur, email_visiteur, tel_visiteur, " +
                    "num_visite, date_visite, heure_deb_visite, heure_fin_visite, num_bon_visite FROM VisitePatient " +
                    "WHERE status_demande = @status AND nom_patient = @recherche ORDER BY date_visite";
                //Ajout par paramêtre des filtres WHERE
                cmd.Parameters.AddWithValue("@status", statusDemande);
                cmd.Parameters.AddWithValue("@recherche", nomPatient);

                //Création d'un reader
                SqlDataReader reader = cmd.ExecuteReader();

                //Boucle tant que l'on peut lire dans le reader
                while (reader.Read())
                {
                    //Récupération des données du reader dans les variables 
                    string visiteurID = reader.GetValue(0).ToString();
                    string nomV = reader.GetString(1);
                    string prenomV = reader.GetString(2);
                    string emailV = reader.GetString(3);
                    string telV;
                    if (reader.IsDBNull(4))
                    {
                        telV = "";
                    }
                    else
                    {
                        telV = reader.GetString(4);

                    }
                    string numVisiteV = reader.GetString(5);
                    string dateV = reader.GetValue(6).ToString().Split(' ').First();
                    string heureD = reader.GetValue(7).ToString();
                    string heureF = reader.GetValue(8).ToString();
                    string numBonVisite = reader.GetString(9);

                    //Instanciation d'un visiteur
                    Visiteur visiteur = new Visiteur(visiteurID, nomV, prenomV, emailV, numVisiteV);
                    //Si telV n'est pas vide
                    if (!String.IsNullOrEmpty(telV))
                    {
                        //Affectation de la variable telV au téléphone du visiteur
                        visiteur.TelVisiteur = telV;
                    }

                    //Instanciation d'une demande de visite avec les variables créée précédement
                    demandeDeVisite demande = new demandeDeVisite(visiteur, dateV, heureD, heureF);
                    listeDemande.Add(demande);
                }
                return listeDemande;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                closeConnection();
            }            
        }

        //Constructeur de la base de donnée
        public databaseHIA()
        {
            dbHIA = new SqlConnection(Properties.Settings.Default.dbConnectionString);
        }
    }
}
