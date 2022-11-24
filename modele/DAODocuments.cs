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
        public static List<Exemplaire> getAllExemplaire()
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
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
            return lesExemplaires;
        }

        public static void ajouterSignalement(SignalerExemplaire signaler)
        {
            try
            {
                string query = "INSERT INTO signalerexemplaire (  codeD , numeroD , nom , prenom , date)" + "VALUES('" + signaler.Exemplaire.Document.IdDoc.ToString() + "' ,'" + signaler.Exemplaire.Numero.ToString() + "' ,  '" + signaler.Nom.ToString() + "' , '" + signaler.Prenom.ToString() + "' , Current_Date  )";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public static void modifierParutionDeteriore(Parution parution)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00003' where parution.idRevue='" + parution.Revue.Id + "' AND parution.numero= '" + parution.Numero + "'";;
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
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

        public static void modifierExemplaireInnutilisable(Exemplaire exemplaire)
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

        public static void modifierParutionUsage(Parution parution)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00002'   where parution.idRevue='" + parution.Revue.Id + "' AND parution.numero= '" + parution.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public static void modifierParutionInnutilisable(Parution parution)
        {
            try
            {
                string query = "UPDATE parution set parution.idEtat='00004'   where parution.idRevue='" + parution.Revue.Id + "' AND parution.numero= '" + parution.Numero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static List<Parution> getAllParution()
        {
            List<Parution> lesParutions = new List<Parution>();
            string req = "select parution.numero , parution.dateParution , parution.photo , revue.id , revue.titre , revue.empruntable , revue.periodicite , revue.dateFinAbonnement , revue.delai_miseadispo , revue.idDesc , etat.id , etat.libelle from revue inner join parution on revue.id=parution.idRevue inner join etat on etat.id=parution.idEtat   ";
            DAOFactory.connecter();
            MySqlDataReader reader = DAOFactory.execSQLRead(req);
            while (reader.Read())
            {
                Parution parution = new Parution(int.Parse(reader[0].ToString()), DateTime.Parse(reader[1].ToString()), reader[2].ToString(), new Revue(reader[3].ToString() , reader[4].ToString() , Char.Parse(reader[5].ToString()), reader[6].ToString() , DateTime.Parse(reader[7].ToString()) , int.Parse(reader[8].ToString()), reader[9].ToString()) , new Etat(int.Parse(reader[10].ToString()), reader[11].ToString()));
                lesParutions.Add(parution);
            }
            DAOFactory.deconnecter();
            return lesParutions;
        }

        public static List<SignalerExemplaire> getAllSignalementExemplaire()
        {
            List<SignalerExemplaire> lesSignalementExemplaires = new List<SignalerExemplaire>();
            string req = "Select signalerExemplaire.codeD , document.id , document.titre , document.image  , categorie.id , categorie.libelle , exemplaire.numero, exemplaire.dateAchat , exemplaire.idRayon , etat.id , etat.libelle ,signalerexemplaire.nom , signalerexemplaire.prenom , signalerexemplaire.date    from categorie inner join document on document.idCategorie=categorie.id inner join signalerexemplaire on signalerexemplaire.codeD=document.id inner join exemplaire on exemplaire.idDoc=document.id inner join etat on etat.id=exemplaire.idEtat ";
            DAOFactory.connecter();
            MySqlDataReader reader = DAOFactory.execSQLRead(req);
            while (reader.Read())
            {
                SignalerExemplaire signalerExemplaire = new SignalerExemplaire( reader[0].ToString(),  new Exemplaire(new Document(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), new Categorie(reader[4].ToString(), reader[5].ToString())) , reader[6].ToString(), reader[7].ToString() , reader[8].ToString() , new Etat(int.Parse(reader[9].ToString()) , reader[10].ToString()))  , reader[11].ToString(), reader[12].ToString() ,DateTime.Parse(reader[13].ToString()));
                lesSignalementExemplaires.Add(signalerExemplaire);
            }
            DAOFactory.deconnecter();
            return lesSignalementExemplaires;
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
            string req = "Select * from document ";
            DAOFactory.connecter();
            MySqlDataReader reader = DAOFactory.execSQLRead(req);
            while (reader.Read())
            {
                Document document = new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),  new Categorie(reader[3].ToString(), reader[4].ToString()));
                lesDocuments.Add(document);
            }
            DAOFactory.deconnecter();
            return lesDocuments;
        }

        public static List<Revue> getAllRevue()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select revue.id , revue.titre , revue.empruntable , revue.periodicite , revue.dateFinABonnement , revue.delai_miseadispo , revue.idDesc from revue ";
            DAOFactory.connecter();
            MySqlDataReader reader = DAOFactory.execSQLRead(req);
            while (reader.Read())
            {
                Revue revue = new Revue(reader[0].ToString(), reader[1].ToString(), char.Parse(reader[2].ToString()), reader[3].ToString(), DateTime.Parse(reader[4].ToString()) , int.Parse(reader[5].ToString()) , reader[6].ToString());
                lesRevues.Add(revue);
            }
            DAOFactory.deconnecter();
            return lesRevues;
        }


        public static List<DVD> getAllDvd()
        {
            List<DVD> lesDvd = new List<DVD>();
            string req = "Select dvd.id, dvd.synopsis, dvd.duree , dvd.réalisateur, d.titre, d.image ,d.idCategorie, c.libelle from dvd inner join document d on dvd.id=d.id inner join categorie c on d.idCategorie = c.id ";
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
            return categorie;
        }

    }

}
