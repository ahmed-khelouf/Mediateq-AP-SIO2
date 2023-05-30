using System;
using MySql.Data.MySqlClient;
using Mediateq_AP_SIO2.divers;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Class DAOFactory
    /// </summary>
    class DAOFactory
    {
        private static MySqlConnection connexion;

        /// <summary>
        /// Crée une connexion à la base de données
        /// </summary>
        /// <exception cref="ExceptionSio"></exception>
        public static void creerConnection()
        {
            string serverIp = "127.0.0.1";
            string username = "root";
            string password = "";
            string databaseName = "mediateq-c";
             

            string dbConnectionString = string.Format("server={0};uid={1};pwd={2};database={3}; ", serverIp, username, password, databaseName);

            try
            {
                connexion = new MySqlConnection(dbConnectionString);
            }
            catch (Exception e)
            {
                throw new ExceptionSio(1, "problème création connexion BDD", e.Message);
            }

        }

        /// <summary>
        /// Ouverture connexion à la base de données
        /// </summary>
        /// <exception cref="ExceptionSio"></exception>
        public static void connecter()
        {
            try
            {
                connexion.Open();
            }
            catch (Exception e)
            {
                throw new ExceptionSio(1, "problème ouverture connexion BDD", e.Message);
            }
        }

        /// <summary>
        /// Déconnexion à la base de données
        /// </summary>
        public static void deconnecter()
        {
            connexion.Close();
        }


        /// <summary>
        /// Lecture à la base de données
        /// </summary>
        /// <param name="requete"></param>
        /// <returns></returns>
        /// <exception cref="ExceptionSio"></exception>
        // Exécution d'une requête de lecture ; retourne un Datareader
        public static MySqlDataReader execSQLRead(string requete)
        {
            MySqlCommand command;
            MySqlDataAdapter adapter;
            command = new MySqlCommand();
            command.CommandText = requete;
            command.Connection = connexion;

            adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;

            MySqlDataReader dataReader;

            try
            {
                dataReader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new ExceptionSio(1, "Erreur lecture BDD", e.Message);
            }


            return dataReader;
        }

        /// <summary>
        /// Ecriture à la base de données
        /// </summary>
        /// <param name="requete"></param>
        /// <exception cref="ExceptionSio"></exception>
        // Exécution d'une requete d'écriture (Insert ou Update) ; ne retourne rien
        public static void execSQLWrite(string requete)
        {

            MySqlCommand command;
            command = new MySqlCommand();
            command.CommandText = requete;
            command.Connection = connexion;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new ExceptionSio(1, "Erreur écriture BDD", e.Message);
            }
            
        }
    }
}
