using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class Utilisateur
    {
        private string id;
        private string userName;
        private string nom;
        private string prenom;
        private string password;



        public Utilisateur(string unId, string unUserName, string unNom, string unPrenom, string unPassword)
        {
            id = unId;
            userName = unUserName;
            nom = unNom;
            prenom = unPrenom;
            password = unPassword;
           
        }

        public string Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }

    }
}
