using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class dvd qui hérite de la class document
    /// </summary>
    class DVD : Document
    {
        private string synopsis;
        private string realisteur;
        private int duree;

        /// <summary>
        /// Constructeur dvd
        /// </summary>
        /// <param name="unSynopsis"></param>
        /// <param name="unRealisteur"></param>
        /// <param name="uneDuree"></param>
        /// <param name="unId"></param>
        /// <param name="unTitre"></param>
        /// <param name="uneImage"></param>
        /// <param name="uneCategorie"></param>
        public DVD(string unSynopsis, string unRealisteur, int uneDuree, string unId, string unTitre, string uneImage , Categorie uneCategorie) : base(unId, unTitre, uneImage , uneCategorie)
        {
            Synopsis = unSynopsis;
            Realisteur = unRealisteur;
            Duree = uneDuree;
        }

        /// <summary>
        /// getter et setter sur synopsis
        /// </summary>
        public string Synopsis { get => synopsis; set => synopsis = value; }

        /// <summary>
        /// getter et setter sur realisateur
        /// </summary>
        public string Realisteur { get => realisteur; set => realisteur = value; }

        /// <summary>
        /// getter et setter sur duree
        /// </summary>
        public int Duree { get => duree; set => duree = value; }

    }
}

