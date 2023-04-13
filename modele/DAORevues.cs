using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.modele
{
    class DAORevues
    {
        //Modifier l'état d'une parution en deterioré dans la base de données
        public static void modifierParutionDeteriore(Parution parution)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00003' where parution.idRevue='" + parution.Revue.Id + "' AND parution.numero= '" + parution.Numero + "'"; ;
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        //Modifier l'état d'une parution en usagé dans la base de données
        public static void modifierParutionUsage(Parution parution)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00002'   where parution.idRevue='" + parution.Revue.Id + "' AND parution.numero= '" + parution.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        //Modifier l'état d'un exemplaire en inutilisable dans la base de données
        public static void modifierParutionInutilisable(Parution parution)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00004'   where parution.idRevue='" + parution.Revue.Id + "' AND parution.numero= '" + parution.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        //Récupération de tous les parutions dans la base de données
        public static List<Parution> getAllParution()
        {
            List<Parution> lesParutions = new List<Parution>();
            try
            {
                string req = "select parution.numero , parution.dateParution , parution.photo , revue.id , revue.titre , revue.empruntable , revue.periodicite , revue.dateFinAbonnement , revue.delai_miseadispo , revue.idDesc , etat.id , etat.libelle from revue inner join parution on revue.id=parution.idRevue inner join etat on etat.id=parution.idEtat   ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Parution parution = new Parution(int.Parse(reader[0].ToString()), DateTime.Parse(reader[1].ToString()), reader[2].ToString(), new Revue(reader[3].ToString(), reader[4].ToString(), Char.Parse(reader[5].ToString()), reader[6].ToString(), DateTime.Parse(reader[7].ToString()), int.Parse(reader[8].ToString()), reader[9].ToString()), new Etat(int.Parse(reader[10].ToString()), reader[11].ToString()));
                    lesParutions.Add(parution);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesParutions;
        }


        //Récupération de tous les revue dans la base de données
        public static List<Revue> getAllRevue()
        {
            List<Revue> lesRevues = new List<Revue>();
            try
            {
                string req = "Select revue.id , revue.titre , revue.empruntable , revue.periodicite , revue.dateFinABonnement , revue.delai_miseadispo , revue.idDesc from revue ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Revue revue = new Revue(reader[0].ToString(), reader[1].ToString(), char.Parse(reader[2].ToString()), reader[3].ToString(), DateTime.Parse(reader[4].ToString()), int.Parse(reader[5].ToString()), reader[6].ToString());
                    lesRevues.Add(revue);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;

            }
            return lesRevues;
        }


        //Obtenir la liste de tous les titres de revues 
        public static List<Revue> getAllTitre()
        {
            List<Revue> lesTitres = new List<Revue>();
            string req = "Select * from revue";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Revue titre = new Revue(reader[0].ToString(), reader[1].ToString(), char.Parse(reader[2].ToString()), reader[3].ToString(), DateTime.Parse(reader[5].ToString()), int.Parse(reader[4].ToString()), reader[6].ToString());
                lesTitres.Add(titre);
            }
            DAOFactory.deconnecter();

            return lesTitres;
        }


        //Récupérer les parution d'une revue à partir de son titre
        public static List<Parution> getParutionByTitre(Revue pTitre)
        {
            List<Parution> lesParutions = new List<Parution>();
            string req = "SELECT p.numero, p.dateParution, p.photo, r.id AS revue_id, r.titre AS revue_titre, r.empruntable, r.periodicite, r.dateFinAbonnement, r.delai_miseadispo, r.idDesc, e.id AS etat_id, e.libelle " +
             "FROM revue r " +
             "INNER JOIN parution p ON r.id = p.idRevue " +
             "INNER JOIN etat e ON e.id = p.idEtat " +
             "WHERE r.id = " + pTitre.Id;

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Parution parution = new Parution(int.Parse(reader[0].ToString()), DateTime.Parse(reader[1].ToString()), reader[2].ToString(), new Revue(reader[3].ToString(), reader[4].ToString(), Char.Parse(reader[5].ToString()), reader[6].ToString(), DateTime.Parse(reader[7].ToString()), int.Parse(reader[8].ToString()), reader[9].ToString()), new Etat(int.Parse(reader[10].ToString()), reader[11].ToString()));
                lesParutions.Add(parution);
            }
            DAOFactory.deconnecter();
            return lesParutions;
        }
    }
}
