using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class Utilisateur
    /// </summary>
    public class Utilisateur 
    {
        private int id;
        private string userName;
        private string nom;
        private string prenom;
        private string password;
        private string role;


        /// <summary>
        /// Constructeur utilisateur
        /// </summary>
        /// <param name="unId"></param>
        /// <param name="unUserName"></param>
        /// <param name="unNom"></param>
        /// <param name="unPrenom"></param>
        /// <param name="unPassword"></param>
        /// <param name="unRole"></param>
        public Utilisateur(int unId, string unUserName, string unNom, string unPrenom, string unPassword , string unRole)
        {
            id = unId;
            userName = unUserName;
            nom = unNom;
            prenom = unPrenom;
            password = unPassword;
            role = unRole;
        }

        /// <summary>
        /// getter et setter sur id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// getter et setter sur nom
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// getter et setter sur prenom
        /// </summary>
        public string Prenom { get => prenom; set => prenom = value; }

        /// <summary>
        /// getter et setter sur userName
        /// </summary>
        public string UserName { get => userName; set => userName = value; }

        /// <summary>
        /// getter et setter sur password
        /// </summary>
        public string Password { get => password; set => password = value; }

        /// <summary>
        /// getter et setter sur role
        /// </summary>
        public string Role { get => role; set => role = value; }

    }
}
