using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class Parution
    {
        private string numero;
        private DateTime dateParution;
        private string photo;
        private Revue uneRevue;
        private Etat unEtat;

        public Parution(string numero, DateTime dateParution, string photo, Revue uneRevue, Etat unEtat)
        {
            this.numero = numero;
            this.dateParution = dateParution;
            this.photo = photo;
            this.uneRevue = uneRevue;
            this.unEtat = unEtat;
           
        }

        public string Numero { get => numero; set => numero = value; }
        public DateTime DateParution { get => dateParution; set => dateParution = value; }
        public string Photo { get => photo; set => photo = value; }
        public Revue Revue { get => uneRevue; set => uneRevue = value; }
        public Etat Etat { get => unEtat; set => unEtat = value; }
    }
}
