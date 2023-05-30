using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class livre qui hérite de la class document
    /// </summary>
    class Livre : Document
    {
        private string ISBN;
        private string auteur;
        private string laCollection;

        /// <summary>
        /// Constructeur livre
        /// </summary>
        /// <param name="unId"></param>
        /// <param name="unTitre"></param>
        /// <param name="unISBN"></param>
        /// <param name="unAuteur"></param>
        /// <param name="uneCollection"></param>
        /// <param name="uneImage"></param>
        /// <param name="uneCategorie"></param>
        public Livre(string unId, string unTitre, string unISBN, string unAuteur, string uneCollection,string uneImage, Categorie uneCategorie) : base(unId, unTitre, uneImage, uneCategorie)
        {
            ISBN1 = unISBN;
            Auteur = unAuteur;
            LaCollection = uneCollection;
        }

        /// <summary>
        /// getter et setter sur ISBN
        /// </summary>
        public string ISBN1 { get => ISBN; set => ISBN = value; }

        /// <summary>
        /// getter et setter sur auteur
        /// </summary>
        public string Auteur { get => auteur; set => auteur = value; }

        /// <summary>
        /// getter et setter sur laCollection
        /// </summary>
        public string LaCollection { get => laCollection; set => laCollection = value; }
    }
}
