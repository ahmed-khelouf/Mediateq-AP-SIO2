using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediateq_AP_SIO2.metier
{
    /// <summary>
    /// Class signalerParution
    /// </summary>
    class SignalerParution
    {
        private int id;
        private Revue revue;
        private Parution parution;
        private string nom;
        private string prenom;
        private DateTime date;

        /// <summary>
        /// Constructeur signalerParution
        /// </summary>
        /// <param name="unId"></param>
        /// <param name="uneRevue"></param>
        /// <param name="uneParution"></param>
        /// <param name="unNom"></param>
        /// <param name="unPrenom"></param>
        /// <param name="uneDate"></param>
        public SignalerParution(int unId, Revue uneRevue, Parution uneParution, string unNom, string unPrenom, DateTime uneDate)
        {
            id = unId;
            revue = uneRevue;
            parution = uneParution;
            nom = unNom;
            prenom = unPrenom;
            date = uneDate;
        }

        /// <summary>
        /// getter et setter sur id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// getter et setter sur revue
        /// </summary>
        public Revue Revue { get => revue; set => revue = value; }

        /// <summary>
        /// getter et setter sur parution
        /// </summary>
        public Parution Parution { get => parution; set => parution = value; }

        /// <summary>
        /// getter et setter sur nom
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// prenom
        /// </summary>
        public string Prenom { get => prenom; set => prenom = value; }

        /// <summary>
        /// getter et setter sur date
        /// </summary>
        public DateTime Date { get => date; set => date = value; }
    }
}
