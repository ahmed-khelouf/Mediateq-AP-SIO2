using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class exemplaire
    /// </summary>
    class Exemplaire
    {
        private Document unDocument;
        private string numero;
        private DateTime dateAchat;
        private string idRayon;
        private Etat etat;

        /// <summary>
        /// Constructeur exemplaire
        /// </summary>
        /// <param name="unDocument"></param>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="idRayon"></param>
        /// <param name="unetat"></param>
        public Exemplaire(Document unDocument, string numero, DateTime dateAchat, string idRayon, Etat unetat )
        {
            this.unDocument = unDocument;
            this.numero = numero;
            this.dateAchat = dateAchat;
            this.idRayon = idRayon;
            this.etat = unetat;
        }

        /// <summary>
        /// getter et setter sur document
        /// </summary>
        public Document Document { get => unDocument; set => unDocument = value; }

        /// <summary>
        /// getter et setter sur numero
        /// </summary>
        public string Numero { get => numero; set => numero = value; }

        /// <summary>
        /// getter et setter sur dateAchat
        /// </summary>
        public DateTime DateAchat { get => dateAchat; set => dateAchat = value; }

        /// <summary>
        /// getter et setter sur idRayon
        /// </summary>
        public string IdRayon { get => idRayon; set => idRayon = value; }

        /// <summary>
        /// getter et setter sur etat
        /// </summary>
        public Etat Etat { get => etat; set => etat = value; }


    }
}
