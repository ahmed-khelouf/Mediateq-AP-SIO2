﻿using Mediateq_AP_SIO2.metier;
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
        public static List<Exemplaire> getAllExemplaire()
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            try
            {
                string req = "Select document.id , document.titre , document.image , categorie.id , categorie.libelle , exemplaire.numero ,exemplaire.dateAchat, exemplaire.idRayon , Etat.id , Etat.libelle from categorie ";
                req += " join document on categorie.id=document.idCategorie";
                req += " join exemplaire  on document.id=exemplaire.idDoc";
                req += " join etat  on etat.id = exemplaire.idEtat";

                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                while (reader.Read())
                {
                     Exemplaire exemplaire = new Exemplaire(new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString() , new Categorie(reader[3].ToString(), reader[4].ToString())) ,reader[5].ToString(), reader[6].ToString(), reader[7].ToString(),  new Etat(int.Parse(reader[8].ToString()) , reader[9].ToString()) );
                     lesExemplaires.Add(exemplaire);
                }
                 DAOFactory.deconnecter();
            }

            catch (Exception exc)
            {
                throw exc;
            }
            return lesExemplaires;
        }

        //public static void ajouterUnHistorique(Historique historique)
        //{
            //try
            //{
                //string query = "INSERT INTO historique (  idDoc , etat , date , numeroExemplaire)" + "VALUES('"+ historique.IdExemplaire.Document.IdDoc.ToString() + "' ,'" + historique.IdEtat.Libelle.ToString() + "' , '" + "', Current_Date  " + "' , '" + historique.IdExemplaire.Numero.ToString() +" )";
                //DAOFactory.connecter();
                //DAOFactory.execSQLWrite(query);
                //DAOFactory.deconnecter();
            //}
           // catch (Exception exc)
           //{
              //  throw exc;
           // }

       // }

        //public static List<Historique> getAllHistorique()
        //{
           // List<Historique> lesHistorique = new List<Historique>();
           // try
            //{
                //string req = "select historique.id , historique.idExemplaire , historique.etat , historique.date , exemplaire.idDoc , exemplaire.numero , exemplaire.dateAchat , exemplaire.dateAchat , exemplaire.idRayon , exemplaire.idEtat , etat.id , etat.libelle from historique inner join exemplaire on historique.idExemplaire=exemplaire.idDoc inner join etat on etat.id=exemplaire.idEtat ";
                //DAOFactory.connecter();
                //MySqlDataReader reader = DAOFactory.execSQLRead(req);
               // while (reader.Read())
               // {
                  //  Historique historique = new Historique(int.Parse(reader[0].ToString()), new Exemplaire(new Document(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), new Categorie(reader[4].ToString(), reader[5].ToString())), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), new Etat(int.Parse(reader[9].ToString()), reader[10].ToString())), reader[11].ToString(), DateTime.Parse(reader[12].ToString()));
                    //lesHistorique.Add(historique);
               // }
               // DAOFactory.deconnecter();
            //}
            //catch (Exception exc)
            //{
              //  throw exc;

            //}
            //return lesHistorique;
        //}

        public static void modifierExemplaireDeteriore(Exemplaire exemplaire)
        {
            try
            {
         
                string query = "UPDATE exemplaire set exemplaire.idEtat='00003' where exemplaire.idDoc='" + exemplaire.Document.IdDoc + "'AND exemplaire.numero= '" + exemplaire.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public static void modifierExemplaireUsage(Exemplaire exemplaire)
        {
            try
            {
                string query = "UPDATE exemplaire set exemplaire.idEtat='00002'   where exemplaire.idDoc='" + exemplaire.Document.IdDoc + "' AND exemplaire.numero= '" + exemplaire.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc) 
            {
                throw exc;
            }
        }

        public static void modifierExemplaireInutilisable(Exemplaire exemplaire)
        {
            try
            {
                string query = "UPDATE exemplaire set exemplaire.idEtat='00004'   where exemplaire.idDoc='" + exemplaire.Document.IdDoc + "' AND exemplaire.numero= '" + exemplaire.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static void ajouterDvd(DVD dvd)
        {
            try
            {
                string query = "INSERT INTO document ( id , titre , image , commandeEnCours , idCategorie)" + "VALUES('" + dvd.IdDoc.ToString() + "' ,'" + dvd.Titre.ToString() + "' ,'" + dvd.Image.ToString() + "' , null , null )";
                string queryss = "INSERT INTO categorie (id , libelle )" + " VALUES('" + dvd.IdDoc.ToString() + "', " + dvd.LaCategorie.Libelle + ")";
                string querys = "INSERT INTO dvd (id , synopsis , réalisateur , duree)" + "VALUES('" + dvd.IdDoc.ToString() + "','" + dvd.Synopsis.ToString() + "', '" + dvd.Realisteur.ToString() + "' ," + int.Parse(dvd.Duree.ToString()) + " )";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.execSQLWrite(querys);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }
       
        public static List<Document> getAllDocument()
        {
            List<Document> lesDocuments = new List<Document>();
            try
            {
                string req = "Select * from document ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                     Document document = new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),  new Categorie(reader[3].ToString(), reader[4].ToString()));
                     lesDocuments.Add(document);
                }
                 DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;

            }
            return lesDocuments;
        }

        public static List<DVD> getAllDvd()
        {
            List<DVD> lesDvd = new List<DVD>();
            try
            {
                string req = "Select dvd.id, dvd.synopsis, dvd.duree , dvd.réalisateur, d.titre, d.image ,d.idCategorie, c.libelle from dvd inner join document d on dvd.id=d.id inner join categorie c on d.idCategorie = c.id ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                     DVD dvd = new DVD(reader[0].ToString(), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString(), reader[4].ToString(), reader[5].ToString() , new Categorie(reader[6].ToString() , reader[7].ToString()));
                     lesDvd.Add(dvd);
                }
                 DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;

            }
            return lesDvd;
        }

        public static List<Categorie> getAllCategories()
        {
            List<Categorie> lesCategories = new List<Categorie>();
            try
            {
                string req = "Select * from categorie";

                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                while (reader.Read())
                {
                    Categorie categorie = new Categorie(reader[0].ToString(), reader[1].ToString());
                    lesCategories.Add(categorie);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;

            }
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
            try
            {
                string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, d.idCategorie, c.libelle from livre l ";
                req += " join document d on l.id=d.id";
                req += " join categorie c on d.idCategorie = c.id";

                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                while (reader.Read())
                {
                    Livre livre = new Livre(reader[0].ToString(), reader[3].ToString(), reader[1].ToString(),
                    reader[2].ToString(), reader[5].ToString(), reader[4].ToString(),new Categorie(reader[6].ToString(),reader[7].ToString()));
                    lesLivres.Add(livre);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesLivres;
        }

        public static void setDescripteurs(List<Livre> lesLivres)
        {
            try
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
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static Categorie getCategorieByLivre(Livre pLivre)
        {
            Categorie categorie;
            try
            {
                string req = "Select c.id,c.libelle from categorie c,document d where c.id = d.idCtagorie and d.id='";
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
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return categorie;
        }

    }

}