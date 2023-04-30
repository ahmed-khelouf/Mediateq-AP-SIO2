using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mediateq_AP_SIO2.metier
{
     class SignalerExemplaire
    {
        private int id;
        private Document document;
        private Exemplaire exemplaire;
        private string nom;
        private string prenom;
        private DateTime date;
        

        public SignalerExemplaire( int unId , Document unDocument, Exemplaire unExemplaire, string unNom, string unPrenom , DateTime uneDate)
        {
            id = unId;
            document = unDocument;
            exemplaire = unExemplaire;
            nom = unNom;
            prenom = unPrenom;
            date = uneDate;
        }

        public int Id { get => id; set => id = value; }
        public Document Document { get => document; set => document = value; }
        public Exemplaire Exemplaire { get => exemplaire; set => exemplaire = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}
