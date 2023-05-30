using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class descripteur
    /// </summary>
    class Descripteur
    {
        private string id;
        private string libelle;

        /// <summary>
        /// Constructeur descripteur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Descripteur(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }


        /// <summary>
        /// getter et setter de id 
        /// </summary>
        public string Id { get => id; set => id = value; }

        /// <summary>
        /// getter et setter de libelle
        /// </summary>
        public string Libelle { get => libelle; set => libelle = value; }
    }





}
