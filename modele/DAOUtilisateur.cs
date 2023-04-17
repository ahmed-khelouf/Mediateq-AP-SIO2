using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.modele
{
     class DAOUtilisateur
    {
        public static void ajouterUtilisateur(Utilisateur utilisateur)
        {
            try
            {
                // GÉNÉRER UN ID D'ABONNÉ UNIQUE
                string id = Guid.NewGuid().ToString();

                // CRYPTER LE MOT DE PASSE AVEC SHA256
                string passwordHash = BitConverter.ToString(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(utilisateur.Password))).Replace("-", "");

                string query = "INSERT INTO utilisateur (id , userName, nom, prenom, password) VALUES ('" + id + "', '" + utilisateur.UserName.ToString() + "', '" + utilisateur.Nom.ToString() + "', '" + utilisateur.Prenom.ToString() + "', '" + passwordHash + "')";

                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        //Récupération de tous les utilisateurs dans la base de données
        public static List<Utilisateur> getAllUtilisateur()
        {
            List<Utilisateur> lesUtilisateurs = new List<Utilisateur>();
            try
            {
                string req = "Select * from utilisateur ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Utilisateur utilisateur = new Utilisateur(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    lesUtilisateurs.Add(utilisateur);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesUtilisateurs;
        }

        public static void recupereUtilisateur(Utilisateur utilisateur)
        {
            try
            {
                

                string query = " SELECT * FROM utilisateur where userName = '" + utilisateur.UserName + "' and password = '" + utilisateur.Password + "'";

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
