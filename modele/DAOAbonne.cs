using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediateq_AP_SIO2.modele
{
    class DAOAbonne
    {

        //Récupération de tous les abonnes dans la base de données
        public static List<Abonne> getAllAbonne()
        {
            List<Abonne> lesAbonnes = new List<Abonne>();
            try
            {
                string req = "Select * from abonne ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Abonne abonne = new Abonne(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString() , reader[4].ToString(), reader[5].ToString(), DateTime.Parse(reader[6].ToString()), DateTime.Parse(reader[7].ToString()), DateTime.Parse(reader[8].ToString()));
                    lesAbonnes.Add(abonne);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesAbonnes;
        }


        // AJout d'un nouveau abonne dans a bdd
        public static void ajouterAbonne(string unNom , string unPrenom , string unTele , string uneAdresse , string unMail , DateTime uneDateNaiss ,  DateTime uneFinAbonnement)
        {
            try
            {
  
                // SPÉCIFIER LES DATES DE DÉBUT ET DE FIN D'ABONNEMENT

                DateTime finAbonnement = uneFinAbonnement.AddDays(60);
                DateTime unDebutAbonnement = DateTime.Now;
                string date1 = DateTime.Now.ToString();

                string query = "INSERT INTO abonne ( nom, prenom, telephone, adresse, email, dateNaissance, dateDebutAbonnement, dateFinAbonnement) VALUES ('" + unNom + "', '" + unPrenom + "', '" + unTele + "', '" + uneAdresse + "', '" + unMail + "', '" + uneDateNaiss.ToString("yyyy-MM-dd") + "', '" + unDebutAbonnement.ToString("yyyy-MM-dd") + "', '" + finAbonnement.ToString("yyyy-MM-dd") + "')";

                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        // Modification des informations d'un abonnes
        public static void modifierAbonne(Abonne abonne)
        {
            try
            {
                string query = "UPDATE abonne SET  nom= '" + abonne.Nom.ToString() + "', prenom='" + abonne.Prenom.ToString() + "', telephone='" + abonne.Telephone.ToString() + "', adresse='" + abonne.Adresse.ToString() + "', email='" + abonne.Email.ToString() + "', dateNaissance='" + abonne.DateNaissance.ToString("yyyy-MM-dd") + "', dateDebutAbonnement='" + abonne.DebutAbonnement.ToString("yyyy-MM-dd") + "', dateFinAbonnement='" + abonne.FinAbonnement.ToString("yyyy-MM-dd") + "' WHERE id='" + abonne.Id.ToString() + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
                MessageBox.Show("Abonné modifié avec succès.");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        // Récupération d'un abonné avec son id 
        public static Abonne getRecupAbonneById(int abonneId)
        {
            try
            {
                string req = "SELECT * FROM abonne WHERE id = '" + abonneId + "'";
                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                if (reader.Read())
                {
                    Abonne abonne = new Abonne(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), DateTime.Parse(reader[6].ToString()), DateTime.Parse(reader[7].ToString()), DateTime.Parse(reader[8].ToString()));
                    DAOFactory.deconnecter();
                    return abonne;
                }
                else
                {
                    DAOFactory.deconnecter();
                    return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        // Supprimer un abonne 
        public static void supprimerAbonne(Abonne abonne)
        {
            try
            {
                string query = "DELETE from abonne  WHERE id='" + abonne.Id.ToString() + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
