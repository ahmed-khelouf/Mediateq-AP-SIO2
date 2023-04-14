using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class Abonne 
    {
        private string id;
        private string nom;
        private string prenom;
        private string telephone;
        private string adresse;
        private string email;
        private DateTime dateNaissance;
        private DateTime debutAbonnement;
        private DateTime finAbonnement;


        public Abonne(string id ,string unNom, string unPrenom, string unTelephone, string uneAdresse, string unEmail, DateTime uneDateNaissance, DateTime unDebutAbonnement , DateTime uneFinAbonnement )
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

        public string Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public string Adresse { get => adresse; set => adresse = value; }
        public string Email { get => email; set => email = value; }
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }
        public DateTime DebutAbonnement { get => debutAbonnement; set => debutAbonnement = value; }
        public DateTime FinAbonnement { get => finAbonnement; set => finAbonnement = value; }

    }
}
