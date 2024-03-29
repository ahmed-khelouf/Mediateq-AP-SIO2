﻿using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.modele
{
    /// <summary>
    /// Connexion à la base de données pour les utilisiateurs
    /// </summary>
    class DAOUtilisateur
    {

        /// <summary>
        /// Ajout d'un utilisateur dans la bdd
        /// </summary>
        /// <param name="unUserName"></param>
        /// <param name="unNom"></param>
        /// <param name="unPrenom"></param>
        /// <param name="unPassword"></param>
        public static void ajouterUtilisateur(string unUserName , string unNom , string unPrenom , string unPassword)
        {
            try
            {
                string query = "INSERT INTO utilisateur ( userName, nom, prenom, password) VALUES ('" + unUserName + "', '" + unNom + "', '" + unPrenom + "', '" + unPassword + "')";

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
        /// Récupération de tous les utilisateurs dans la base de données
        /// </summary>
        /// <returns></returns>
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
                    Utilisateur utilisateur = new Utilisateur(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString() , reader[5].ToString());
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



        /// <summary>
        /// recupere un utilisateur de la bdd
        /// </summary>
        /// <param name="unUserName"></param>
        /// <returns></returns>
        public static Utilisateur recupereUtilisateur(string unUserName)
        {
            try 
            {
                Utilisateur utilisateur = null;
                string req = " SELECT * FROM utilisateur where userName = '" + unUserName +"'";

                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    utilisateur = new Utilisateur(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString() , reader[5].ToString());
                }
                DAOFactory.deconnecter();
                return utilisateur;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
