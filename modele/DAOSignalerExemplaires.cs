using Mediateq_AP_SIO2.metier;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediateq_AP_SIO2.modele
{
    class DAOSignalerExemplaires
    {


        public static List<SignalerExemplaire> getAllSignalementExemplaire()
        {
            List<SignalerExemplaire> lesSignalementExemplaires = new List<SignalerExemplaire>();
            try
            {
                string req = "Select signalerExemplaire.codeD , document.id , document.titre , document.image  , categorie.id , categorie.libelle , exemplaire.numero, exemplaire.dateAchat , exemplaire.idRayon , etat.id , etat.libelle ,signalerexemplaire.nom , signalerexemplaire.prenom , signalerexemplaire.date    from categorie inner join document on document.idCategorie=categorie.id inner join signalerexemplaire on signalerexemplaire.codeD=document.id inner join exemplaire on exemplaire.idDoc=document.id inner join etat on etat.id=exemplaire.idEtat ";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);
                while (reader.Read())
                {
                    SignalerExemplaire signalerExemplaire = new SignalerExemplaire(reader[0].ToString(), new Exemplaire(new Document(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), new Categorie(reader[4].ToString(), reader[5].ToString())), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), new Etat(int.Parse(reader[9].ToString()), reader[10].ToString())), reader[11].ToString(), reader[12].ToString(), DateTime.Parse(reader[13].ToString()));
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
    }
}
