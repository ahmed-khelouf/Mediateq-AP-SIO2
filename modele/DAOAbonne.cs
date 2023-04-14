using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Abonne abonne = new Abonne(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString() , reader[4].ToString(), reader[5].ToString(), DateTime.Parse(reader[6].ToString()), DateTime.Parse(reader[7].ToString()), DateTime.Parse(reader[8].ToString()));
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
        public static void ajouterAbonne(Abonne abonne)
        {
            try
            {
                // GÉNÉRER UN ID D'ABONNÉ UNIQUE
                string id = Guid.NewGuid().ToString();

                // SPÉCIFIER LES DATES DE DÉBUT ET DE FIN D'ABONNEMENT
                DateTime debutAbonnement = DateTime.Now;
                DateTime finAbonnement = debutAbonnement.AddDays(60);

                string query = "INSERT INTO abonne (id , nom, prenom, telephone, adresse, email, dateNaissance, dateDebutAbonnement, dateFinAbonnement) VALUES ('" + id +  "', '" + abonne.Nom.ToString() + "', '" + abonne.Prenom.ToString() + "', '" + abonne.Telephone.ToString() + "', '" + abonne.Adresse.ToString() + "', '" + abonne.Email.ToString() + "', '" + abonne.DateNaissance.ToString("yyyy-MM-dd") + "', '" + debutAbonnement.ToString("yyyy-MM-dd") + "', '" + finAbonnement.ToString("yyyy-MM-dd") + "')";

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
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        //Récupérer les exemplaires d'un document à partir de son titre
        public static List<Abonne> getAbonneById(Abonne aId)
        {
            try
            {
                List<Abonne> lesAbonnes = new List<Abonne>();
                string req = "SELECT * FROM abonne WHERE id = '" + aId.Id +  "'";

                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                while (reader.Read())
                {
                    Abonne abonne = new Abonne(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), DateTime.Parse(reader[6].ToString()), DateTime.Parse(reader[7].ToString()), DateTime.Parse(reader[8].ToString()));
                    lesAbonnes.Add(abonne);
                }
                DAOFactory.deconnecter();
                return lesAbonnes;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

    }
}
