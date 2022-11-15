using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2
{
    class DAODocuments
    {
        public static void ajouterDvd(DVD dvd)
        {
            
            string query = "INSERT INTO document ( id , titre , image , commandeEnCours , idPublic)" + "VALUES('" + dvd.IdDoc.ToString() + "' ,'" + dvd.Titre.ToString() + "' ,'" + dvd.Image.ToString() + "' , null , null )";
            string queryss = "INSERT INTO categorie (id , libelle )" +" VALUES('" + dvd.IdDoc.ToString() + "', " + dvd.LaCategorie.Libelle + ")";
            string querys = "INSERT INTO dvd (id , synopsis , réalisateur , duree)" + "VALUES('" + dvd.IdDoc.ToString() + "','" + dvd.Synopsis.ToString() + "', '" + dvd.Realisteur.ToString() + "' ," + int.Parse(dvd.Duree.ToString()) + " )";
            DAOFactory.connecter();
            DAOFactory.execSQLWrite(query);
           
            DAOFactory.execSQLWrite(querys);
            DAOFactory.deconnecter();


        }


        public static List<DVD> getAllDvd()
        {
            List<DVD> lesDvd = new List<DVD>();
            string req = "Select dvd.id, dvd.synopsis, dvd.duree , dvd.réalisateur, d.titre, d.image ,d.idPublic, c.libelle from dvd inner join document d on dvd.id=d.id inner join categorie c on d.idPublic = c.id ";
            DAOFactory.connecter();
            MySqlDataReader reader = DAOFactory.execSQLRead(req);
            while (reader.Read())
            {
                DVD dvd = new DVD(reader[0].ToString(), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString(), reader[4].ToString(), reader[5].ToString() , new Categorie(reader[6].ToString() , reader[7].ToString()));
                lesDvd.Add(dvd);
            }
            DAOFactory.deconnecter();
            return lesDvd;
        }
        public static List<Categorie> getAllCategories()
        {
            List<Categorie> lesCategories = new List<Categorie>();
            string req = "Select * from categorie";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Categorie categorie = new Categorie(reader[0].ToString(), reader[1].ToString());
                lesCategories.Add(categorie);
            }
            DAOFactory.deconnecter();
            return lesCategories;
        }

        public static List<Descripteur> getAllDescripteurs()
        {
            List<Descripteur> lesDescripteurs = new List<Descripteur>();

            try
            {
                string req = "Select * from descripteur";

                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                while (reader.Read())
                {
                    Descripteur genre = new Descripteur(reader[0].ToString(), reader[1].ToString());
                    lesDescripteurs.Add(genre);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
                
            }
            return lesDescripteurs;
        }
         

public static List<Livre> getAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, d.idPublic, c.libelle from livre l ";
            req += " join document d on l.id=d.id";
            req += " join categorie c on d.idPublic = c.id";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Livre livre = new Livre(reader[0].ToString(), reader[3].ToString(), reader[1].ToString(),
                reader[2].ToString(), reader[5].ToString(), reader[4].ToString(),new Categorie(reader[6].ToString(),reader[7].ToString()));
  
                lesLivres.Add(livre);
                
            }

            DAOFactory.deconnecter();

            return lesLivres;
        }

        public static void setDescripteurs(List<Livre> lesLivres)
        {
            DAOFactory.connecter();

            foreach (Livre livre in lesLivres)
            {
                List<Descripteur> lesDescripteursDuLivre = new List<Descripteur>(); ;
                string req = "Select de.id, de.libelle from descripteur de ";
                req += " join est_decrit_par e on de.id = e.idDesc";
                req += " join document do on do.id = '" + livre.IdDoc + "'";
                             
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    lesDescripteursDuLivre.Add(new Descripteur(reader[0].ToString(), reader[1].ToString()));
                }
                livre.LesDescripteurs = lesDescripteursDuLivre;
            }
            DAOFactory.deconnecter();
        }

        public static Categorie getCategorieByLivre(Livre pLivre)
        {
            Categorie categorie;
            string req = "Select c.id,c.libelle from categorie c,document d where c.id = d.idPublic and d.id='";
            req += pLivre.IdDoc + "'";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            if (reader.Read())
            {
                categorie = new Categorie(reader[0].ToString(), reader[1].ToString());
            }
            else
            {
                categorie = null;
            }
            DAOFactory.deconnecter();
            return categorie;
        }

    }

}
