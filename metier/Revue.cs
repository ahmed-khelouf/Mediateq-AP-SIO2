using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class revue
    /// </summary>
    class Revue
    {
        private string id;
        private string titre;
        private char empruntable;
        private string periodicite;
        private DateTime dateFinAbonnement;
        private int delaiMiseADispo;
        private string idDescripteur;

        /// <summary>
        /// Constructeur revue
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="empruntable"></param>
        /// <param name="periodicite"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="delaiMiseADispo"></param>
        /// <param name="idDescripteur"></param>
        public Revue(string id, string titre, char empruntable, string periodicite, DateTime dateFinAbonnement, int delaiMiseADispo, string idDescripteur)
        {
            this.id = id;
            this.titre = titre;
            this.empruntable = empruntable;
            this.periodicite = periodicite;
            this.dateFinAbonnement = dateFinAbonnement;
            this.delaiMiseADispo = delaiMiseADispo;
            this.idDescripteur = idDescripteur;
        }

        /// <summary>
        /// getter et setter sur id
        /// </summary>
        public string Id { get => id; set => id = value; }

        /// <summary>
        /// getter et setter sur titre
        /// </summary>
        public string Titre { get => titre; set => titre = value; }

        /// <summary>
        /// getter et setter sur empruntable
        /// </summary>
        public char Empruntable { get => empruntable; set => empruntable = value; }

        /// <summary>
        /// getter et setter sur periodicite
        /// </summary>
        public string Periodicite { get => periodicite; set => periodicite = value; }

        /// <summary>
        /// getter et setter sur dateFinAbonnement
        /// </summary>
        public DateTime DateFinAbonnement { get => dateFinAbonnement; set => dateFinAbonnement = value; }

        /// <summary>
        /// getter et setter sur delaiMiseADispo
        /// </summary>
        public int DelaiMiseADispo { get => delaiMiseADispo; set => delaiMiseADispo = value; }

        /// <summary>
        /// getter et setter sur idDescripteur
        /// </summary>
        public string IdDescripteur { get => idDescripteur; set => idDescripteur = value; }
    }
}
