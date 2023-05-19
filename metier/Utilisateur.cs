﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    public class Utilisateur
    {
        private int id;
        private string userName;
        private string nom;
        private string prenom;
        private string password;
        private string role;


        public Utilisateur(int unId, string unUserName, string unNom, string unPrenom, string unPassword , string unRole)
        {
            id = unId;
            userName = unUserName;
            nom = unNom;
            prenom = unPrenom;
            password = unPassword;
            role = unRole;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }

        public string Role { get => role; set => role = value; }

    }
}
