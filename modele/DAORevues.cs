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
    }
}
