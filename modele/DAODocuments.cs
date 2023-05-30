using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Connexion à la base de données pour les documents
    /// </summary>
    class DAODocuments
    {

        /// <summary>
        /// Récupération de tous les exemplaires dans la base de données
        /// </summary>
        /// <returns></returns>
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
                     Exemplaire exemplaire = new Exemplaire(new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString() , new Categorie(reader[3].ToString(), reader[4].ToString())) ,reader[5].ToString(), DateTime.Parse(reader[6].ToString()), reader[7].ToString(),  new Etat(int.Parse(reader[8].ToString()) , reader[9].ToString()) );
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

        /// <summary>
        /// Modifier l'état d'un exemplaire en deterioré dans la base de données
        /// </summary>
        /// <param name="unDoc"></param>
        /// <param name="unNumero"></param>
        public static void modifierExemplaireDeteriore(string unDoc , string unNumero)
        {
            try
            {
         
                string query = "UPDATE exemplaire set exemplaire.idEtat='00003' where exemplaire.idDoc='" + unDoc + "'AND exemplaire.numero= '" + unNumero + "'";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// Modifier l'état d'un exemplaire en usagé dans la base de données
        /// </summary>
        /// <param name="exemplaire"></param>
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



        /// <summary>
        /// Modifier l'état d'un exemplaire en inutilisable dans la base de données
        /// </summary>
        /// <param name="exemplaire"></param>
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



        /// <summary>
        /// Ajout d'un DVD dans la base de données
        /// </summary>
        /// <param name="dvd"></param>
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



        /// <summary>
        /// Récupération de tous les documents dans la base de données
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Récupération de tous les dvd dans la base de données
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Récupération de tous les categories dans la base de données
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Récupération de tous les descripteurs dans la base de données
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Récupération de tous les livres dans la base de données
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// Récupère tous les descripteurs associés aux livres dans la liste donnée et les associe aux livres correspondants
        /// </summary>
        /// <param name="lesLivres"></param>
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



        /// <summary>
        /// Obtenir la catégorie d'un livre dans la base de données
        /// </summary>
        /// <param name="pLivre"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Récupérer les exemplaires d'un document à partir de son titre
        /// </summary>
        /// <param name="dTitre"></param>
        /// <returns></returns>
        public static List<Exemplaire> getDocumentByTitre(Document dTitre)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select document.id , document.titre , document.image , categorie.id , categorie.libelle , exemplaire.numero ,exemplaire.dateAchat, exemplaire.idRayon , Etat.id , Etat.libelle from document inner join exemplaire on exemplaire.idDoc = document.id inner join categorie on categorie.id = document.idCategorie inner join etat on etat.id = exemplaire.idEtat  where document.id  = " + dTitre.IdDoc;
            
            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Exemplaire exemplaire = new Exemplaire(new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), new Categorie(reader[3].ToString(), reader[4].ToString())), reader[5].ToString(), DateTime.Parse(reader[6].ToString()), reader[7].ToString(), new Etat(int.Parse(reader[8].ToString()), reader[9].ToString()));
                lesExemplaires.Add(exemplaire);
            }
            DAOFactory.deconnecter();
            return lesExemplaires;
        }



        /// <summary>
        /// Récupèration de tous les documents signalé dans la bdd
        /// </summary>
        /// <returns></returns>
        public static List<SignalerExemplaire> getAllSignalementExemplaire()
        {
            List<SignalerExemplaire> lesSignalementExemplaires = new List<SignalerExemplaire>();
            try
            {
                string req = "SELECT signalerExemplaire.id,  signalerexemplaire.nom, signalerexemplaire.prenom, signalerexemplaire.dateSignaler, document.id, document.titre, document.image, categorie.id, categorie.libelle, exemplaire.numero, exemplaire.dateAchat, exemplaire.idRayon, etat.id, etat.libelle FROM categorie INNER JOIN document ON document.idCategorie = categorie.id INNER JOIN signalerexemplaire ON signalerexemplaire.idDoc = document.id INNER JOIN exemplaire ON exemplaire.numero = signalerexemplaire.numeroExemplaire INNER JOIN etat ON etat.id = exemplaire.idEtat GROUP BY signalerexemplaire.id ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Document document = new Document(reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), new Categorie(reader[7].ToString(), reader[8].ToString()));
                    Exemplaire exemplaire = new Exemplaire(document, reader[9].ToString(), DateTime.Parse(reader[10].ToString()), reader[11].ToString(), new Etat(int.Parse(reader[12].ToString()), reader[13].ToString()));
                    SignalerExemplaire signalerExemplaire = new SignalerExemplaire(int.Parse(reader[0].ToString()), document, exemplaire, reader[1].ToString(), reader[2].ToString(), DateTime.Parse(reader[3].ToString()));

                    lesSignalementExemplaires.Add(signalerExemplaire);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesSignalementExemplaires;
        }



        /// <summary>
        /// Récupérer les exemplaires signaler à partir de id du Document
        /// </summary>
        /// <param name="sIdDoc"></param>
        /// <returns></returns>
        public static List<SignalerExemplaire> getSignalerExemplairesByIdDoc(Document sIdDoc)
        {
            List<SignalerExemplaire> lesSignalementExemplaires = new List<SignalerExemplaire>();
            try
            {
                string req = "SELECT signalerExemplaire.id,  signalerexemplaire.nom, signalerexemplaire.prenom, signalerexemplaire.dateSignaler, document.id, document.titre, document.image, categorie.id, categorie.libelle, exemplaire.numero, exemplaire.dateAchat, exemplaire.idRayon, etat.id, etat.libelle FROM categorie INNER JOIN document ON document.idCategorie = categorie.id INNER JOIN signalerexemplaire ON signalerexemplaire.idDoc = document.id INNER JOIN exemplaire ON exemplaire.numero = signalerexemplaire.numeroExemplaire INNER JOIN etat ON etat.id = exemplaire.idEtat  where signalerexemplaire.idDoc  = " + sIdDoc.IdDoc ;
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    Document document = new Document(reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), new Categorie(reader[7].ToString(), reader[8].ToString()));
                    Exemplaire exemplaire = new Exemplaire(document, reader[9].ToString(), DateTime.Parse(reader[10].ToString()), reader[11].ToString(), new Etat(int.Parse(reader[12].ToString()), reader[13].ToString()));
                    SignalerExemplaire signalerExemplaire = new SignalerExemplaire(int.Parse(reader[0].ToString()), document, exemplaire, reader[1].ToString(), reader[2].ToString(), DateTime.Parse(reader[3].ToString()));

                    lesSignalementExemplaires.Add(signalerExemplaire);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesSignalementExemplaires;
        }



        /// <summary>
        ///  AJOUT dun signalement a la bdd
        /// </summary>
        /// <param name="idDoc"></param>
        /// <param name="numeroExemplaire"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="signaler"></param>
        public static void ajouterSignalement(string idDoc , string numeroExemplaire , string nom , string prenom , DateTime signaler)
        {
            try
            {


                //Recupération de la date 
                DateTime date = DateTime.Now.Date;

                string query = "INSERT INTO signalerExemplaire (  idDoc , numeroExemplaire , nom , prenom , dateSignaler)" + "VALUES('"  + idDoc + "' ,'" + numeroExemplaire + "' ,  '" + nom + "' , '" + prenom + "' , '" + signaler.ToString("yyyy-MM-dd") + "' )";
                DAOFactory.connecter();
                DAOFactory.execSQLWrite(query);
                DAOFactory.deconnecter();
            }

            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
