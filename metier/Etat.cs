using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class etat
    /// </summary>
    class Etat
    {
        private int id;
        private string libelle;


        /// <summary>
        /// Constructeur etat
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Etat(int id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;

        }

        /// <summary>
        /// getter et setter sur id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// getter et setter sur libelle
        /// </summary>
        public string Libelle { get => libelle; set => libelle = value; }

    }
}
