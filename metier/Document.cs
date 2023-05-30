using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class document
    /// </summary>
    class Document
    {
        private string idDoc;
        private string titre;
        private string image;
        private Categorie laCategorie;
        private List<Descripteur> lesDescripteurs;

        /// <summary>
        /// Constructeur document
        /// </summary>
        /// <param name="unId"></param>
        /// <param name="unTitre"></param>
        /// <param name="uneImage"></param>
        /// <param name="uneCategorie"></param>
        public Document(string unId, string unTitre, string uneImage, Categorie uneCategorie)
        {
            idDoc = unId;
            titre = unTitre;
            image = uneImage;
            laCategorie = uneCategorie;
        }

        /// <summary>
        /// getter et setter sur idDoc
        /// </summary>
        public string IdDoc { get => idDoc; set => idDoc = value; }

        /// <summary>
        /// getter et setter sur titre
        /// </summary>
        public string Titre { get => titre; set => titre = value; }

        /// <summary>
        /// getter et setter sur image
        /// </summary>
        public string Image { get => image; set => image = value; }

        /// <summary>
        /// getter et setter sur categorie
        /// </summary>
        internal Categorie LaCategorie { get => laCategorie; set => laCategorie = value; }

        /// <summary>
        /// getter et setter sur la list LesDescripteurs
        /// </summary>
        internal List<Descripteur> LesDescripteurs { get => lesDescripteurs; set => lesDescripteurs = value; }
    }


}
