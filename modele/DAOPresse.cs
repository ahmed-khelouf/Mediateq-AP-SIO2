using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mediateq_AP_SIO2.metier;

namespace Mediateq_AP_SIO2
{
    class DAOPresse
    {
        /*public static List<Domaine> getAllDomaines()
        {
            List<Domaine> lesDomaines = new List<Domaine>();
            string req = "Select * from domaine";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Domaine domaine = new Domaine(reader[0].ToString(), reader[1].ToString());
                lesDomaines.Add(domaine);
            }
            DAOFactory.deconnecter();
            return lesDomaines;
        }

        public static Domaine getDomainesById(string pId)
        {
            Domaine domaine;
            string req = "Select * from domaine where idDomaine = " + pId;

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            if (reader.Read())
            {
                domaine = new Domaine(reader[0].ToString(), reader[1].ToString());
            }
            else
            {
                domaine =null;
            }
            DAOFactory.deconnecter();
            return domaine;
        }

        public static Domaine getDomainesByTitre(Revue pTitre)
        {
            Domaine domaine;
            string req = "Select d.idDomaine,d.libelle from domaine d,titre t where d.idDomaine = t.idDomaine and t.idTitre='" ;
            req += pTitre.IdTitre + "'";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            if (reader.Read())
            {
                domaine = new Domaine(reader[0].ToString(), reader[1].ToString());
            }
            else
            {
                domaine = null;
            }
            DAOFactory.deconnecter();
            return domaine;
        }
        */

       
      

    }
}

