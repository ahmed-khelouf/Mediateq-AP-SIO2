using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
     class SignalerExemplaire
    {
        
        private string codeD;
        private Exemplaire unExemplaire;
        private string nom;
        private string prenom;
        private DateTime date;
        

        public SignalerExemplaire(  string codeD, Exemplaire unExemplaire, string nom, string prenom)
        {
            
            this.codeD = codeD;
            this.unExemplaire = unExemplaire;
            this.nom = nom;
            this.prenom = prenom;
        }

        
        
        public string CodeD { get => codeD; set => codeD = value; }

        public Exemplaire Exemplaire { get => unExemplaire; set => unExemplaire = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}
