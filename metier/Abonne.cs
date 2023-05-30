using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
  /// <summary>
  /// Class abonne
  /// </summary>
     class Abonne 
    {
        private int id;
        private string nom;
        private string prenom;
        private string telephone;
        private string adresse;
        private string email;
        private DateTime dateNaissance;
        private DateTime debutAbonnement;
        private DateTime finAbonnement;

        /// <summary>
        /// Constructeur abonne
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unNom"></param>
        /// <param name="unPrenom"></param>
        /// <param name="unTelephone"></param>
        /// <param name="uneAdresse"></param>
        /// <param name="unEmail"></param>
        /// <param name="uneDateNaissance"></param>
        /// <param name="unDebutAbonnement"></param>
        /// <param name="uneFinAbonnement"></param>
        public Abonne(int id ,string unNom, string unPrenom, string unTelephone, string uneAdresse, string unEmail, DateTime uneDateNaissance, DateTime unDebutAbonnement , DateTime uneFinAbonnement )
        {
            this.id = id;
            this.nom = unNom;
            this.prenom = unPrenom;
            this.telephone = unTelephone;
            this.adresse = uneAdresse;
            this.email = unEmail;
            this.dateNaissance = uneDateNaissance;
            this.debutAbonnement = unDebutAbonnement;
            this.finAbonnement = uneFinAbonnement;
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
        /// getter et setter sur telephone
        /// </summary>
        public string Telephone { get => telephone; set => telephone = value; }

        /// <summary>
        /// getter et setter sur adresse
        /// </summary>
        public string Adresse { get => adresse; set => adresse = value; }

        /// <summary>
        /// getter et setter sur email
        /// </summary>
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// getter et setter sur dateNaissance
        /// </summary>
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }

        /// <summary>
        /// getter et setter sur debutAbonnement
        /// </summary>
        public DateTime DebutAbonnement { get => debutAbonnement; set => debutAbonnement = value; }

        /// <summary>
        /// getter et setter sur finAbonnement
        /// </summary>
        public DateTime FinAbonnement { get => finAbonnement; set => finAbonnement = value; }

    }
}
