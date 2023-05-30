using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.modele
{
    /// <summary>
    /// Connexion à la base de données pour les revues
    /// </summary>
    class DAORevues
    {

        /// <summary>
        /// Modifier l'état d'une parution en deterioré dans la base de données
        /// </summary>
        /// <param name="revue"></param>
        /// <param name="numero"></param>
        public static void modifierParutionDeteriore(string revue , string numero)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00003' where parution.idRevue='" + revue + "' AND parution.numero= '" + numero + "'"; ;
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }



        /// <summary>
        /// Modifier l'état d'une parution en usagé dans la base de données
        /// </summary>
        /// <param name="parution"></param>
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



        /// <summary>
        /// Modifier l'état d'un exemplaire en inutilisable dans la base de données
        /// </summary>
        /// <param name="parution"></param>
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



        /// <summary>
        /// Récupération de tous les parutions dans la base de données
        /// </summary>
        /// <returns></returns>
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
                    Parution parution = new Parution(reader[0].ToString(), DateTime.Parse(reader[1].ToString()), reader[2].ToString(), new Revue(reader[3].ToString(), reader[4].ToString(), Char.Parse(reader[5].ToString()), reader[6].ToString(), DateTime.Parse(reader[7].ToString()), int.Parse(reader[8].ToString()), reader[9].ToString()), new Etat(int.Parse(reader[10].ToString()), reader[11].ToString()));
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



        /// <summary>
        /// Récupération de tous les revue dans la base de données
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Obtenir la liste de tous les titres de revues 
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Récupérer les parution d'une revue à partir de son titre
        /// </summary>
        /// <param name="pTitre"></param>
        /// <returns></returns>
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
                Parution parution = new Parution(reader[0].ToString(), DateTime.Parse(reader[1].ToString()), reader[2].ToString(), new Revue(reader[3].ToString(), reader[4].ToString(), Char.Parse(reader[5].ToString()), reader[6].ToString(), DateTime.Parse(reader[7].ToString()), int.Parse(reader[8].ToString()), reader[9].ToString()), new Etat(int.Parse(reader[10].ToString()), reader[11].ToString()));
                lesParutions.Add(parution);
            }
            DAOFactory.deconnecter();
            return lesParutions;
        }



        /// <summary>
        /// Récupèration de tous les Parutions signalé dans la bdd
        /// </summary>
        /// <returns></returns>
        public static List<SignalerParution> getAllSignalementParution()
        {
            List<SignalerParution> lesSignalementParutions = new List<SignalerParution>();
            try
            {
                string req = "SELECT signalerparution.id, signalerparution.nom, signalerparution.prenom, signalerparution.dateSignaler, revue.id, revue.titre, revue.empruntable, revue.periodicite, revue.dateFinAbonnement, revue.delai_miseadispo, revue.idDesc, parution.numero, parution.dateParution, parution.photo, etat.id, etat.libelle FROM signalerparution inner join revue on revue.id = signalerparution.idRevue inner join parution on parution.numero = signalerparution.numeroRevue inner join etat on etat.id = parution.idEtat ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Revue revue = new Revue(reader[4].ToString(), reader[5].ToString(), char.Parse(reader[6].ToString()), reader[7].ToString(), DateTime.Parse(reader[8].ToString()), int.Parse(reader[9].ToString()), reader[10].ToString());
                    Parution parution = new Parution(reader[11].ToString(), DateTime.Parse(reader[12].ToString()), reader[13].ToString(), revue, new Etat(int.Parse(reader[14].ToString()), reader[15].ToString()));
                    SignalerParution signalerParution = new SignalerParution(int.Parse(reader[0].ToString()), revue, parution, reader[1].ToString(), reader[2].ToString(), DateTime.Parse(reader[3].ToString()));

                    lesSignalementParutions.Add(signalerParution);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesSignalementParutions;
        }



        /// <summary>
        /// Récupérer les parutions signaler à partir de id de Revue
        /// </summary>
        /// <param name="sIdRevue"></param>
        /// <returns></returns>
        public static List<SignalerParution> getSignalerParutionByIdDoc(Revue sIdRevue)
        {
            List<SignalerParution> lesSignalementParutions = new List<SignalerParution>();
            try
            {
                string req = "SELECT signalerparution.id, signalerparution.nom, signalerparution.prenom, signalerparution.dateSignaler, revue.id, revue.titre, revue.empruntable, revue.periodicite, revue.dateFinAbonnement, revue.delai_miseadispo, revue.idDesc, parution.numero, parution.dateParution, parution.photo, etat.id, etat.libelle FROM signalerparution inner join revue on revue.id = signalerparution.idRevue inner join parution on parution.numero = signalerparution.numeroRevue inner join etat on etat.id = parution.idEtat  where signalerparution.idRevue = " + sIdRevue.Id ;
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Revue revue = new Revue(reader[4].ToString(), reader[5].ToString(), char.Parse(reader[6].ToString()), reader[7].ToString(), DateTime.Parse(reader[8].ToString()), int.Parse(reader[9].ToString()), reader[10].ToString());
                    Parution parution = new Parution(reader[11].ToString(), DateTime.Parse(reader[12].ToString()), reader[13].ToString(),  revue, new Etat(int.Parse(reader[14].ToString()), reader[15].ToString()));
                    SignalerParution signalerParution = new SignalerParution(int.Parse(reader[0].ToString()), revue, parution, reader[1].ToString(), reader[2].ToString(), DateTime.Parse(reader[3].ToString()));

                    lesSignalementParutions.Add(signalerParution);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesSignalementParutions;
        }



        /// <summary>
        /// AJOUT dun signalement a la bdd
        /// </summary>
        /// <param name="idRevue"></param>
        /// <param name="numeroRevue"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="signaler"></param>
        public static void ajouterSignalement(string idRevue , string numeroRevue , string nom , string prenom , DateTime signaler)
        {
            try
            {


                //Recupération de la date 
                DateTime date = DateTime.Now.Date;

                string query = "INSERT INTO signalerParution (  idRevue , numeroRevue , nom , prenom , dateSignaler)" + "VALUES('" + idRevue + "' ,'" + numeroRevue + "' ,  '" + nom + "' , '" + prenom + "' , '" + signaler.ToString("yyyy-MM-dd") + "' )";
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
