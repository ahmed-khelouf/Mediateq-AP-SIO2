using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class SignalerParution
    {
        private int id;
        private Revue revue;
        private Parution parution;
        private string nom;
        private string prenom;
        private DateTime date;


        public SignalerParution(int unId, Revue uneRevue, Parution uneParution, string unNom, string unPrenom, DateTime uneDate)
        {
            id = unId;
            revue = uneRevue;
            parution = uneParution;
            nom = unNom;
            prenom = unPrenom;
            date = uneDate;
        }

        public int Id { get => id; set => id = value; }
        public Revue Revue { get => revue; set => revue = value; }
        public Parution Parution { get => parution; set => parution = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}
