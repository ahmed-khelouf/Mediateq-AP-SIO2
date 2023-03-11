using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class Historique
    {
       
        private Exemplaire idExemplaire;
        private Etat idEtat;
        private DateTime date;
        

        public Historique( Exemplaire idExemplaire, Etat idEtat )
        {
    
            this.idExemplaire = idExemplaire;
            this.idEtat = idEtat;
            this.date = DateTime.Now;
        }

        

        public Exemplaire IdExemplaire { get => idExemplaire; set => idExemplaire = value; }

        public Etat IdEtat { get => idEtat; set => idEtat = value; }

        public DateTime Date { get => date; set => date = value; }

    }
}
