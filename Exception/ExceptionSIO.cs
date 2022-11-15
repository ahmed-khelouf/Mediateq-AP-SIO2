using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.Exeption
{
    class ExceptionSIO : Exception
    {
        private int niveauExc;
        private string libelleExc;

        public ExceptionSIO(int pNiveau, string pLibelle, string pMessage) : base(pMessage)
        {
            NiveauExc = pNiveau;
            LibelleExc = pLibelle;
        }

        public int NiveauExc { get => niveauExc; set => niveauExc = value; }
        public string LibelleExc { get => libelleExc; set => libelleExc = value; }
    }
}
