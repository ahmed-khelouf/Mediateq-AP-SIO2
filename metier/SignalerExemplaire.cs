using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class signalerExemplaire
    /// </summary>
     class SignalerExemplaire
    {
        private int id;
        private Document document;
        private Exemplaire exemplaire;
        private string nom;
        private string prenom;
        private DateTime date;
        
        /// <summary>
        /// Constructeur signalerExemplaire
        /// </summary>
        /// <param name="unId"></param>
        /// <param name="unDocument"></param>
        /// <param name="unExemplaire"></param>
        /// <param name="unNom"></param>
        /// <param name="unPrenom"></param>
        /// <param name="uneDate"></param>
        public SignalerExemplaire( int unId , Document unDocument, Exemplaire unExemplaire, string unNom, string unPrenom , DateTime uneDate)
        {
            id = unId;
            document = unDocument;
            exemplaire = unExemplaire;
            nom = unNom;
            prenom = unPrenom;
            date = uneDate;
        }

        /// <summary>
        /// getter et setter sur id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// getter et setter sur document
        /// </summary>
        public Document Document { get => document; set => document = value; }

        /// <summary>
        /// getter et setter sur exemplaire
        /// </summary>
        public Exemplaire Exemplaire { get => exemplaire; set => exemplaire = value; }

        /// <summary>
        /// getter et setter sur nom
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// prenom
        /// </summary>
        public string Prenom { get => prenom; set => prenom = value; }

        /// <summary>
        /// getter et setter sur date
        /// </summary>
        public DateTime Date { get => date; set => date = value; }
    }
}
