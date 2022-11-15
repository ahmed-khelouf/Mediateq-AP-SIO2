using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class DVD : Document
    {
        private string synopsis;
        private string realisteur;
        private int duree;


        public DVD(string unSynopsis, string unRealisteur, int uneDuree, string unId, string unTitre, string uneImage , Categorie uneCategorie) : base(unId, unTitre, uneImage , uneCategorie)
        {
            Synopsis = unSynopsis;
            Realisteur = unRealisteur;
            Duree = uneDuree;
        }

        public string Synopsis { get => synopsis; set => synopsis = value; }
        public string Realisteur { get => realisteur; set => realisteur = value; }

        public int Duree { get => duree; set => duree = value; }

    }
}

