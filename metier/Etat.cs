using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.metier
{
    class Etat
    {
        private int id;
        private string libelle;

        public Etat(int id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;

        }

        public int Id { get => id; set => id = value; }
        public string Libelle { get => libelle; set => libelle = value; }

    }
}
