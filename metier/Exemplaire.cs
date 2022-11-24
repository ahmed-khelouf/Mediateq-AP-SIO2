using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class Exemplaire
    {
        private Document unDocument;
        private string numero;
        private string dateAchat;
        private string idRayon;
        private Etat etat;

        public Exemplaire(Document unDocument, string numero, string dateAchat, string idRayon, Etat unetat)
        {
            this.unDocument = unDocument;
            this.numero = numero;
            this.dateAchat = dateAchat;
            this.idRayon = idRayon;
            this.etat = unetat;
            
        }

        public Document Document { get => unDocument; set => unDocument = value; }

        public string Numero { get => numero; set => numero = value; }

        public string DateAchat { get => dateAchat; set => dateAchat = value; }

        public string IdRayon { get => idRayon; set => idRayon = value; }

        public Etat Etat { get => etat; set => etat = value; }

    }
}
