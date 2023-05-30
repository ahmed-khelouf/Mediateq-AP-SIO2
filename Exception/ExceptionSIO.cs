using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.Exeption
{
    /// <summary>
    /// Class ExceptionSIO qui hétire de la class exception
    /// </summary>
    public class ExceptionSIO : Exception
    {
        private int niveauExc;
        private string libelleExc;

        /// <summary>
        /// Constructeur ExceptionSIO
        /// </summary>
        /// <param name="pNiveau"></param>
        /// <param name="pLibelle"></param>
        /// <param name="pMessage"></param>
        public ExceptionSIO(int pNiveau, string pLibelle, string pMessage) : base(pMessage)
        {
            NiveauExc = pNiveau;
            LibelleExc = pLibelle;
        }


        /// <summary>
        /// getter et setter de niveauExc
        /// </summary>
        public int NiveauExc { get => niveauExc; set => niveauExc = value; }

        /// <summary>
        /// getter et setter de libelleExc
        /// </summary>
        public string LibelleExc { get => libelleExc; set => libelleExc = value; }
    }
}
