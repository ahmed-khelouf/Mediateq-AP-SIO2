using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class parution
    /// </summary>
    class Parution
    {
        private string numero;
        private DateTime dateParution;
        private string photo;
        private Revue uneRevue;
        private Etat unEtat;


        /// <summary>
        /// Constructeur parution
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateParution"></param>
        /// <param name="photo"></param>
        /// <param name="uneRevue"></param>
        /// <param name="unEtat"></param>
        public Parution(string numero, DateTime dateParution, string photo, Revue uneRevue, Etat unEtat)
        {
            this.numero = numero;
            this.dateParution = dateParution;
            this.photo = photo;
            this.uneRevue = uneRevue;
            this.unEtat = unEtat;
           
        }

        /// <summary>
        /// getter et setter sur numero
        /// </summary>
        public string Numero { get => numero; set => numero = value; }

        /// <summary>
        /// getter et setter sur dateParution
        /// </summary>
        public DateTime DateParution { get => dateParution; set => dateParution = value; }

        /// <summary>
        /// getter et setter sur photo
        /// </summary>
        public string Photo { get => photo; set => photo = value; }

        /// <summary>
        /// getter et setter sur revue
        /// </summary>
        public Revue Revue { get => uneRevue; set => uneRevue = value; }

        /// <summary>
        /// getter et setter sur etat
        /// </summary>
        public Etat Etat { get => unEtat; set => unEtat = value; }
    }
}
