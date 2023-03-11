using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mediateq_AP_SIO2.metier;
using Mediateq_AP_SIO2.Exeption;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using Mediateq_AP_SIO2.modele;
using System.Security.Cryptography;

namespace Mediateq_AP_SIO2
{
    public partial class FrmMediateq : Form
    {
        #region Variables globales

        static List<Categorie> lesCategories;
        static List<Descripteur> lesDescripteurs;
        static List<Revue> lesTitres;
        static List<Livre> lesLivres;
        static List<DVD> lesDvd;
        static List<Exemplaire> lesExemplaires;
        static List<Parution> lesParutions;
        static List<Document> lesDocuments;
        static List<Revue> lesRevues;
        static List<SignalerExemplaire> lesSignalerExemplaires;
        static List<Historique> lesHistoriques; 

        #endregion


        #region Procédures évènementielles

        public FrmMediateq()
        {
            InitializeComponent();
        }

        private void FrmMediateq_Load(object sender, EventArgs e)
        {
            try
            {


                // Création de la connexion avec la base de données
                DAOFactory.creerConnection();

                // Chargement des objets en mémoire
                lesDescripteurs = DAODocuments.getAllDescripteurs();
                lesTitres = DAOPresse.getAllTitre();
                lesDvd = DAODocuments.getAllDvd();
                lesExemplaires = DAODocuments.getAllExemplaire();
                lesParutions = DAORevues.getAllParution();
                lesDocuments = DAODocuments.getAllDocument();
                lesRevues = DAORevues.getAllRevue();
                lesSignalerExemplaires = DAOSignalerExemplaires.getAllSignalementExemplaire();
                //lesHistoriques = DAODocuments.getAllHistorique();




            }
            catch (ExceptionSIO exc)
            {
                MessageBox.Show(exc.NiveauExc + " - " + exc.LibelleExc + " - " + exc.Message);
            }
            

        }

        #endregion


        #region Parutions
        //-----------------------------------------------------------
        // ONGLET "PARUTIONS"
        //------------------------------------------------------------
        private void tabParutions_Enter(object sender, EventArgs e)
        {
            cbxTitres.DataSource = lesTitres;
            cbxTitres.DisplayMember = "titre";
        }

        private void cbxTitres_SelectedIndexChanged(object sender, EventArgs e)
        {
                List<Parution> lesParutions;

                Revue titreSelectionne = (Revue)cbxTitres.SelectedItem;
                lesParutions = DAOPresse.getParutionByTitre(titreSelectionne);

                // ré-initialisation du dataGridView
                dgvParutions.Rows.Clear();

                // Parcours de la collection des titres et alimentation du datagridview
                foreach (Parution parution in lesParutions)
                {
                    dgvParutions.Rows.Add(parution.Numero, parution.DateParution, parution.Photo);
                }
            
        }
        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "TITRES"
        //------------------------------------------------------------
        private void tabTitres_Enter(object sender, EventArgs e)
        {
            cbxDomaines.DataSource = lesDescripteurs;
            cbxDomaines.DisplayMember = "libelle";
        }

        private void cbxDomaines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Objet Domaine sélectionné dans la comboBox
            Descripteur domaineSelectionne = (Descripteur)cbxDomaines.SelectedItem;

            // ré-initialisation du dataGridView
            dgvTitres.Rows.Clear();

            // Parcours de la collection des titres et alimentation du datagridview
            foreach (Revue revue in lesTitres)
            {
                if (revue.IdDescripteur==domaineSelectionne.Id)
                {
                    dgvTitres.Rows.Add(revue.Id, revue.Titre, revue.Empruntable, revue.DateFinAbonnement, revue.DelaiMiseADispo);
                }
            }
        }
        #endregion


        #region Livres
        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        private void tabLivres_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire
            lesCategories = DAODocuments.getAllCategories();
            lesDescripteurs = DAODocuments.getAllDescripteurs();
            lesLivres = DAODocuments.getAllLivres();
            //DAODocuments.setDescripteurs(lesLivres);
        }
   
        private void btnRechercher_Click(object sender, EventArgs e)
        {
            // On réinitialise les labels
            lblNumero.Text = "";
            lblTitre.Text = "";
            lblAuteur.Text = "";
            lblCollection.Text = "";
            lblISBN.Text = "";
            lblImage.Text = "";
            lblCategorie.Text = "";

            // On recherche le livre correspondant au numéro de document saisi.
            // S'il n'existe pas: on affiche un popup message d'erreur
            bool trouve = false;
            foreach (Livre livre in lesLivres)
            {
                if (livre.IdDoc==txbNumDoc.Text)
                {
                    lblNumero.Text = livre.IdDoc;
                    lblTitre.Text = livre.Titre;
                    lblAuteur.Text = livre.Auteur;
                    lblCollection.Text = livre.LaCollection;
                    lblISBN.Text = livre.ISBN1;
                    lblImage.Text = livre.Image;
                    lblCategorie.Text = livre.LaCategorie.Libelle;
                    trouve = true;
                }
            }
            if (!trouve)
                MessageBox.Show("Document non trouvé dans les livres");
        }

        private void txbTitre_TextChanged(object sender, EventArgs e)
        {
            dgvLivres.Rows.Clear();

            // On parcourt tous les livres. Si le titre matche avec la saisie, on l'affiche dans le datagrid.
            foreach (Livre livre in lesLivres)
            {
                // on passe le champ de saisie et le titre en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = txbTitre.Text.ToLower();
                string titreMinuscules;
                titreMinuscules = livre.Titre.ToLower();

                //on teste si le titre du livre contient ce qui a été saisi
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dgvLivres.Rows.Add(livre.IdDoc,livre.Titre,livre.Auteur,livre.ISBN1,livre.LaCollection);
                }
            }
        }
        #endregion


        #region DVD
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        // AFFICHAGE DATAGRIDVIEW DVD
        private void tabDVD_Enter(object sender, EventArgs e)
        {
            // PARCOUR DE COLLECTION ET AJOUTE DES DVD DANS LE DATAGRIDVIEW
            dataGridView1.Rows.Clear();
            foreach (DVD dvd in lesDvd)
            {
                dataGridView1.Rows.Add(dvd.Synopsis, dvd.Realisteur, dvd.Duree);
            }
        }

        #endregion


        #region ajouter DVD
        //ONGLET AJOUTER DVD

        // AJOUTER UN DVD DANS LA BDD
        private void button1_Click(object sender, EventArgs e)
        {
            // CREATION DE DVD ET L'AJOUTE DANS LA COLLECTION
            DVD dvd = new DVD(textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), textBox4.Text, textBox5.Text, textBox6.Text ,new Categorie (textBox7.Text , textBox8.Text) );
            lesDvd.Add(dvd);
            DAODocuments.ajouterDvd(dvd);
        }
        #endregion


        #region ETAT
        //-----------------------------------------------------------
        // ONGLET "CHANGER ETAT D'UN DOCUMENT OU REVUE"
        //-----------------------------------------------------------

        // AFFICHAGE DATAGRIDVIEW DES EXEMPLAIRES ET PARUTIONS
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            // PARCOUR DE COLLECTION ET AJOUTE LES EXEMPLAIRES DANS LE DATAGRIDVIEW
            dataGridView2.Rows.Clear();
            lesExemplaires = DAODocuments.getAllExemplaire();
            foreach (Exemplaire exemplaire in lesExemplaires)
            {            
                    dataGridView2.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Document.Titre ,   exemplaire.Numero, exemplaire.Etat.Libelle);
                    // AJOUTE ID ET LE NUMERO D'UN DOCUMENT A LA COMBOBOX
                    comboBoxDocuments.Items.Add("ID : " + exemplaire.Document.IdDoc + "    numero : " + exemplaire.Numero);
            }
            dataGridView2.Refresh();
            // PARCOUR DE COLLECTION ET AJOUTE LES PARUTIONS DANS LE DATAGRIDVIEW
            dataGridView3.Rows.Clear();
            lesParutions = DAORevues.getAllParution();
            foreach (Parution parution in lesParutions)
            {
                dataGridView3.Rows.Add(parution.Revue.Id, parution.Revue.Titre , parution.Numero, parution.Etat.Libelle);
                // AJOUTE ID ET LE NUMERO D'UNE PARUTION A LA COMBOBOX
                comboBoxRevues.Items.Add("ID : " + parution.Revue.Id + "    numero : " + parution.Numero);
            }
            
            dataGridView3.Refresh();
        }

        // BOUTON POUR REFRESH LE DATAGRIDVIEW
        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            lesExemplaires = DAODocuments.getAllExemplaire();
            foreach (Exemplaire exemplaire in lesExemplaires)
            {
                dataGridView2.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Document.Titre, exemplaire.Numero, exemplaire.Etat.Libelle);
            }
            dataGridView2.Refresh();

            dataGridView3.Rows.Clear();
            lesParutions = DAORevues.getAllParution();
            foreach (Parution parution in lesParutions)
            {
                dataGridView3.Rows.Add(parution.Revue.Id, parution.Revue.Titre, parution.Numero, parution.Etat.Libelle);
            }

            dataGridView3.Refresh();
        }

        // BOUTON POUR EXEMPLAIRE ETAT = USAGE
        private void button2_Click(object sender, EventArgs e)
        {
            bool reussi = false;
            //PARCOUR LA COLLECTION EXEMPLAIRE POUR SELECTIONNER UNE REVUE ET MODIFIER L'ETAT EN USAGE GRACE A LA METHODE modifierExemplaireUsage
            foreach (Exemplaire exemplaire in lesExemplaires)
            {
                if ("ID : " + exemplaire.Document.IdDoc + "    numero : " + exemplaire.Numero == comboBoxDocuments.Text && exemplaire.Etat.Libelle != "usagé")
                {
                        //Historique historique = new Historique(exemplaire, exemplaire.Etat);
                        //lesHistoriques.Add(historique);
                        //DAODocuments.ajouterUnHistorique(historique);
                        DAODocuments.modifierExemplaireUsage(exemplaire);
                        reussi = true;
                }
            }

            //AFFICHE UN MESSAGE SI LE CHANGEMENT EST REUSSI
            if (reussi)
            {
                MessageBox.Show("changement d'etat: Usagé effectué");
            }

            // AFFICHE UN MESSAGE SI LE DOCUMENT EST DEJA EN USAGE
            if (!reussi && comboBoxDocuments.Text != "")
            {
                MessageBox.Show("Le document est déja en usagé !");
            }

            // AFFICHE UN MESSAGE SI AUCUN DOCUMENT EST CHOISI
            if (!reussi && comboBoxDocuments.Text=="")
            {
                MessageBox.Show("Veuillez choisir un document !");
            }

            
        }

        // BOUTON POUR EXEMPLAIRE ETAT = INUTILISBALE 
        private void button3_Click(object sender, EventArgs e)
        {
            bool reussi = false;
            //PARCOUR LA COLLECTION EXEMPLAIRE POUR SELECTIONNER UN EXEMPLAIRE ET MODIFIER L'ETAT EN INNUTILISABLE GRACE A LA METHODE modifierExemplaireInnutilisable
            foreach (Exemplaire exemplaire in lesExemplaires)
            {
                if ("ID : " + exemplaire.Document.IdDoc + "    numero : " + exemplaire.Numero == comboBoxDocuments.Text && exemplaire.Etat.Libelle != "inutilisable")
                {
                        DAODocuments.modifierExemplaireInutilisable(exemplaire);
                        reussi = true;
                }
            }

            //AFFICHE UN MESSAGE SI LE CHANGEMENT EST REUSSI
            if (reussi)
            {
                MessageBox.Show("changement d'etat: Inutilisable effectué");
            }

            // AFFICHE UN MESSAGE SI LE DOCUMENT EST DEJA EN USAGE
            if (!reussi && comboBoxDocuments.Text != "")
            {
                MessageBox.Show("Le document est déja en inutilisable !");
            }

            // AFFICHE UN MESSAGE SI AUCUN DOCUMENT EST CHOISI
            if (!reussi && comboBoxDocuments.Text == "")
            {
                MessageBox.Show("Veuillez choisir un document !");
            }
        }

        // BOUTON POURE PARUTION ETAT = USAGE
        private void button4_Click_1(object sender, EventArgs e)
        {
            bool reussi = false;
            //PARCOUR LA COLLECTION PARUTION POUR SELECTIONNER UNE PARUTION ET MODIFIER L'ETAT EN USAGE GRACE A LA METHODE modifierParutionUsage
            foreach (Parution parution in lesParutions)
            {
                if("ID : " + parution.Revue.Id + "    numero : " + parution.Numero == comboBoxRevues.Text && parution.Etat.Libelle!="usagé")
                {
                      DAORevues.modifierParutionUsage(parution);
                      reussi = true;
                }
            }

            //AFFICHE UN MESSAGE SI LE CHANGEMENT EST REUSSI
            if (reussi)
            {
                MessageBox.Show("changement d'etat: Usagé effectué");
            }

            // AFFICHE UN MESSAGE SI LA REVUE EST DEJA EN USAGE
            if (!reussi && comboBoxRevues.Text != "")
            {
                MessageBox.Show("La Revue est déja en usagé !");
            }

            // AFFICHE UN MESSAGE SI AUCUNE REVUE EST CHOISI
            if (!reussi && comboBoxRevues.Text == "")
            {
                MessageBox.Show("Veuillez choisir une revue !");
            }
        }

        // BOUTON POUR UNE PARUTION ETAT = INUTILISBALE 
        private void button5_Click_1(object sender, EventArgs e)
        {
            bool reussi = false;
            //PARCOUR LA COLLECTION PARUTION POUR SELECTIONNER UNE PARUTION ET MODIFIER L'ETAT EN INUTILISABLE GRACE A LA METHODE modifierParutionInnutilisable
                foreach (Parution parution in lesParutions)
                {
                    if ("ID : " + parution.Revue.Id + "    numero : " + parution.Numero == comboBoxRevues.Text && parution.Etat.Libelle!="inutilisable")
                    {
                          DAORevues.modifierParutionInutilisable(parution);
                          reussi = true;
                    }
                }

            //AFFICHE UN MESSAGE SI LE CHANGEMENT EST REUSSI
            if (reussi)
            {
                MessageBox.Show("changement d'etat: Inutilisable effectué");
            }

            // AFFICHE UN MESSAGE SI LA REVUE EST DEJA EN USAGE
            if (!reussi && comboBoxRevues.Text != "")
            {
                MessageBox.Show("La Revue est déja en inutilisable !");
            }

            // AFFICHE UN MESSAGE SI AUCUNE REVUE EST CHOISI
            if (!reussi && comboBoxRevues.Text == "")
            {
                MessageBox.Show("Veuillez choisir une revue !");
            }
        }


        #endregion


        #region Signaler
        // BOUTON POUR CHANGER L'ETAT EN DETERIORE POUR LES EXEMPLAIRES
        private void button6_Click_1(object sender, EventArgs e)
        {
            
            bool reussi = false;
            foreach (Exemplaire exemplaire in lesExemplaires)
            {
                //SI ID EST EGALE A ID SAISIE DANS LE TEXTE BOX ON CONTINUE 
                if (exemplaire.Document.IdDoc == textBox13.Text)
                {
                    // SI EXEMPLAIRE EST DEJA EN DETERIORER ALORS IL CONTINUE PAS 
                    if (exemplaire.Etat.Libelle != "détérioré")
                    {
                        // SI LE NUMERO EST EGALE AU NUMERO SAISIE DANS LE TEXTE BOX
                        if (exemplaire.Numero == textBox16.Text)
                        {
                            // CHANGE L'ETAT en DETERIORE ET AJOUTE DANS LA TABLE SIGNALER LA PERSONNE A L'ORIGNE DE CETTE ETAT
                            DAODocuments.modifierExemplaireDeteriore(exemplaire);
                            // CREATION D'UN OBJET SIGNALER 
                            SignalerExemplaire signaler = new SignalerExemplaire( textBox13.Text, exemplaire, textBox15.Text, textBox14.Text);
                            DAOSignalerExemplaires.ajouterSignalement(signaler);
                            reussi = true;
                        }
                    }
                }
            }
            
            // AFFICHE UN MESSAGE 
            if (reussi)
            {
                MessageBox.Show("changement d'etat: déterioré effectué");
            }
            else
            {
                MessageBox.Show("erreur code numero ou numero document invalide ");
            }
        }


        // INTERDIRE LA SAISIE DE CARACTERE SUR LES 4 TEXTES BOX
        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
                e.Handled = true;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = false;
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
                e.Handled = true;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = false;
        }

        #endregion


        #region INUTILISABLE
        //-----------------------------------------------------------
        // ONGLET "DOCUMENT OU REVUE INUTILISABLE"
        //-----------------------------------------------------------
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            dataGridViewDocumentsInutilisable.Rows.Clear();
            lesExemplaires = DAODocuments.getAllExemplaire();
            foreach (Exemplaire exemplaire in lesExemplaires)
            {
                if (exemplaire.Etat.Libelle == "inutilisable")
                {
                    dataGridViewDocumentsInutilisable.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Numero, exemplaire.Document.Titre, exemplaire.Etat.Libelle);
                }
            }

            dataGridViewRevuesInutilisable.Rows.Clear();
            lesParutions = DAORevues.getAllParution();
            foreach (Parution parution in lesParutions)
            {
                if (parution.Etat.Libelle == "inutilisable")
                {
                    dataGridViewRevuesInutilisable.Rows.Add(parution.Revue.Id, parution.Numero, parution.Revue.Titre, parution.Etat.Libelle);
                }
            }
        }




        #endregion


        #region DETERIORES
        //-----------------------------------------------------------
        // ONGLET "DOCUMENT OU REVUE DETERIORE !n"
        //-----------------------------------------------------------

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            dataGridView6.Rows.Clear();
            lesSignalerExemplaires = DAOSignalerExemplaires.getAllSignalementExemplaire();
            foreach (SignalerExemplaire signalerExemplaire in lesSignalerExemplaires)
            {
                if (signalerExemplaire.Exemplaire.Etat.Libelle == "détérioré")
                {
                    dataGridView6.Rows.Add(signalerExemplaire.Exemplaire.Document.IdDoc, signalerExemplaire.Exemplaire.Document.Titre, signalerExemplaire.Exemplaire.Numero, signalerExemplaire.Nom, signalerExemplaire.Prenom, signalerExemplaire.Date.ToString());
                }
            }
        }



        #endregion

   
    }
}
