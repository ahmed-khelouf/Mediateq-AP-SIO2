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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

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
        static List<Abonne> lesAbonnes;
        static List<SignalerParution> lesSignalerParutions;


        //la gestion de l'utilisateur connecté en fournissant un moyen pratique d'accéder à ses informations et de les utiliser dans diverses fonctionnalités de l'application.
        public Utilisateur user { get; set; }

        #endregion


        #region Procédures évènementielles

        public FrmMediateq(Utilisateur utilisateur)
        {
            InitializeComponent();
            user = utilisateur;
        }

        private void FrmMediateq_Load(object sender, EventArgs e)
        {
            try
            {
                // Création de la connexion avec la base de données
                DAOFactory.creerConnection();

                // Chargement des objets en mémoire
                lesDescripteurs = DAODocuments.getAllDescripteurs();
                lesTitres = DAORevues.getAllTitre();
                lesDvd = DAODocuments.getAllDvd();
                lesExemplaires = DAODocuments.getAllExemplaire();
                lesParutions = DAORevues.getAllParution();
                lesDocuments = DAODocuments.getAllDocument();
                lesRevues = DAORevues.getAllRevue();
                lesSignalerExemplaires = DAODocuments.getAllSignalementExemplaire();
                lesAbonnes = DAOAbonne.getAllAbonne();
                lesSignalerParutions = DAORevues.getAllSignalementParution();
            }
            catch (ExceptionSIO exc)
            {
                MessageBox.Show(exc.NiveauExc + " - " + exc.LibelleExc + " - " + exc.Message);
            }

            //Gestion des onglets en fonction du rôle de l'utilisateur

             if (user.Role == "administratif")
            {
                // cache les pages suivantes
                tabOngletsAppli.TabPages.Remove(tabParutions); 
                tabOngletsAppli.TabPages.Remove(tabTitres);
                tabOngletsAppli.TabPages.Remove(tabLivres);
                tabOngletsAppli.TabPages.Remove(tabDVD);
                tabOngletsAppli.TabPages.Remove(tabAbonne);
            }

             if(user.Role == "Prêts")
            {
                // cache les pages suivantes
                tabOngletsAppli.TabPages.Remove(tabPageChangerEtat);
                tabOngletsAppli.TabPages.Remove(tabSignaler);
                tabOngletsAppli.TabPages.Remove(tabPageInutilisable);
                tabOngletsAppli.TabPages.Remove(tabPageDeteriore);
                tabOngletsAppli.TabPages.Remove(tabAbonne);
                tabOngletsAppli.TabPages.Remove(tabPageAjoutDVD);
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
            lesParutions = DAORevues.getParutionByTitre(titreSelectionne);

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
                if (revue.IdDescripteur == domaineSelectionne.Id)
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
                if (livre.IdDoc == txbNumDoc.Text)
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
                    dgvLivres.Rows.Add(livre.IdDoc, livre.Titre, livre.Auteur, livre.ISBN1, livre.LaCollection);
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
            DVD dvd = new DVD(textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), textBox4.Text, textBox5.Text, textBox6.Text, new Categorie(textBox7.Text, textBox8.Text));
            lesDvd.Add(dvd);
            DAODocuments.ajouterDvd(dvd);
        }
        #endregion


        #region ETAT
        //-----------------------------------------------------------
        // ONGLET "CHANGER ETAT D'UN DOCUMENT OU REVUE"
        //-----------------------------------------------------------

        // ALLIMENTATION COMBOBOX DES DOCUMENTS ET REVUES
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            // attribue la source de données de la combobox 
            comboBoxDocuments.DataSource = lesDocuments;

            //définition du champ qui doit être affiché dans la combobox
            comboBoxDocuments.DisplayMember = "titre";

            comboBoxRevues.DataSource = lesRevues;

            comboBoxRevues.DisplayMember = "titre";

            // Définir la propriété ReadOnly au datagridview
            dataGridViewDocument.ReadOnly = true;

            dataGridViewRevue.ReadOnly = true;

        }


        //  appelée lorsqu'un document est sélectionné dans la combobox pour afficher les exemplaires correspondants dans le dataGridView
        private void comboBoxDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération du document sélectionné dans la combobox
            Document titreSelectionne = (Document)comboBoxDocuments.SelectedItem;

            // Vérification qu'un document a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des exemplaires pour le document sélectionné à partir de la base de données
                lesExemplaires = DAODocuments.getDocumentByTitre(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewDocument.Rows.Clear();

                // Parcours de la collection des exemplaires
                foreach (Exemplaire exemplaire in lesExemplaires)
                {
                    if(exemplaire.Etat.Libelle == "neuf" || exemplaire.Etat.Libelle == "usagé")
                    {
                        dataGridViewDocument.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Document.Titre, exemplaire.Numero, exemplaire.Etat.Libelle);
                    }
                }
            }
        }


        //  appelée lorsqu'un document est sélectionné dans la combobox pour afficher les parutions correspondants dans le dataGridView
        private void comboBoxRevues_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération de la revue sélectionné dans la combobox
            Revue titreSelectionne = (Revue)comboBoxRevues.SelectedItem;

            // Vérification qu'une revue a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des parutions pour le document sélectionné à partir de la base de données
                lesParutions = DAORevues.getParutionByTitre(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewRevue.Rows.Clear();

                // Parcours de la collection des parutions
                foreach (Parution parution in lesParutions)
                {
                    if(parution.Etat.Libelle == "neuf" || parution.Etat.Libelle == "usagé")
                    {
                        dataGridViewRevue.Rows.Add(parution.Revue.Id, parution.Revue.Titre, parution.Numero, parution.Etat.Libelle);
                    }
                }
            }
        }


        // BOUTON POUR EXEMPLAIRE ETAT = USAGE
        private void buttonDocumentUsage_Click(object sender, EventArgs e)
        {
            // Variable pour vérifier si le changement d'état a réussi
            bool reussi = false;

            // Vérifier si la liste d'exemplaires est non nulle
            if (lesExemplaires != null)
            {
                // Récupérer l'index de l'exemplaire sélectionné dans la DataGridView
                int selectedExemplaire = dataGridViewDocument.CurrentCell.RowIndex;

                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewDocument.SelectedRows[0];
                if (row != null && row.Cells["id"].Value != null)
                {
                    // Récupérer l'exemplaire correspondant à l'index sélectionné
                    Exemplaire exemplaire = lesExemplaires.ElementAt(selectedExemplaire);

                    // Vérifier si l'état de l'exemplaire est déjà "usagé"
                    if (exemplaire.Etat.Libelle == "usagé")
                    {
                        // Afficher un message si l'exemplaire est déjà en usage
                        MessageBox.Show("Ce document est déjà en usage !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Si l'exemplaire n'est pas déjà en usage
                    {
                        // Modifier l'état de l'exemplaire à "usagé" dans la base de données 
                        DAODocuments.modifierExemplaireUsage(exemplaire);

                        // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                        reussi = true;
                    }
                }
            }
            else // Si la liste d'exemplaires est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner un document
                MessageBox.Show("Veuillez choisir un document !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : usagé effectué !", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxDocuments_SelectedIndexChanged"
                comboBoxDocuments_SelectedIndexChanged(sender, e);
            }
        }


        // BOUTON POUR EXEMPLAIRE ETAT = INUTILISBALE 
        private void buttonDocumentInutilisable_Click(object sender, EventArgs e)
        {
            // Variable pour vérifier si le changement d'état a réussi
            bool reussi = false;

            // Vérifier si la liste d'exemplaires est non null
            if (lesExemplaires != null)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewDocument.SelectedRows[0];
                if (row != null && row.Cells["id"].Value != null)
                {
                    // Récupérer l'index de l'exemplaire sélectionné dans la DataGridView
                    int selectedExemplaire = dataGridViewDocument.CurrentCell.RowIndex;

                    // Récupérer l'exemplaire correspondant à l'index sélectionné
                    Exemplaire exemplaire = lesExemplaires.ElementAt(selectedExemplaire);

                    // Vérifier si l'état de l'exemplaire est déjà "inutilisable"
                    if (exemplaire.Etat.Libelle == "inutilisable")
                    {
                        // Afficher un message si l'exemplaire est déjà en inutilisable
                        MessageBox.Show("Ce document est déjà en inutilisable !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Si l'exemplaire n'est pas déjà en inutilisable
                    {
                        // Modifier l'état de l'exemplaire à "inutilisable" dans la base de données 
                        DAODocuments.modifierExemplaireInutilisable(exemplaire);

                        // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                        reussi = true;
                    }
                }
            }
            else // Si la liste d'exemplaires est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner un document
                MessageBox.Show("Veuillez choisir un document !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : inutilisable effectué !", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxDocuments_SelectedIndexChanged"
                comboBoxDocuments_SelectedIndexChanged(sender, e);
            }
        }


        // BOUTON POUR PARUTION ETAT = USAGE
        private void buttonRevueUsage_Click(object sender, EventArgs e)
        {
            // Variable pour vérifier si le changement d'état a réussi
            bool reussi = false;

            // Vérifier si la liste de parutions est non nulle
            if (lesParutions != null)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewRevue.SelectedRows[0];
                if (row != null && row.Cells["ColumnIdRevue"].Value != null)
                {
                    // Récupérer l'index de la parution sélectionné dans la DataGridView
                    int selectedParution = dataGridViewRevue.CurrentCell.RowIndex;

                    // Récupérer la parution correspondant à l'index sélectionné
                    Parution parution = lesParutions.ElementAt(selectedParution);

                    // Vérifier si l'état de la parution est déjà "usagé"
                    if (parution.Etat.Libelle == "usagé")
                    {
                        // Afficher un message si la parution est déjà en usagé
                        MessageBox.Show("Cette parution est déjà en usagé !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Si la parution n'est pas déjà en usagé
                    {
                        // Modifier l'état de la parution à "usagé" dans la base de données 
                        DAORevues.modifierParutionUsage(parution);

                        // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                        reussi = true;
                    }
                }
            }
            else // Si la liste de parution est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner une parution
                MessageBox.Show("Veuillez choisir un document !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : usagé effectué !", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxRevues_SelectedIndexChanged"
                comboBoxRevues_SelectedIndexChanged(sender, e);
            }
        }


        // BOUTON POUR UNE PARUTION ETAT = INUTILISBALE 
        private void buttonRevueInutilisable_Click(object sender, EventArgs e)
        {
            // Variable pour vérifier si le changement d'état a réussi
            bool reussi = false;

            // Vérifier si la liste de parutions est non nulle
            if (lesParutions != null)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewRevue.SelectedRows[0];
                if (row != null && row.Cells["ColumnIdRevue"].Value != null)
                {
                    // Récupérer l'index de la parution sélectionné dans la DataGridView
                    int selectedParution = dataGridViewRevue.CurrentCell.RowIndex;

                    // Récupérer la parution correspondant à l'index sélectionné
                    Parution parution = lesParutions.ElementAt(selectedParution);

                    // Vérifier si l'état de la parution est déjà "inutilisable"
                    if (parution.Etat.Libelle == "inutilisable")
                    {
                        // Afficher un message si la parution est déjà en inutilisable
                        MessageBox.Show("Cette parution est déjà en inutilisable !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Si la parution n'est pas déjà en inutilisable
                    {
                        // Modifier l'état de la parution à "inutilisable" dans la base de données 
                        DAORevues.modifierParutionInutilisable(parution);

                        // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                        reussi = true;
                    }
                }
            }
            else // Si la liste de parution est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner une parution
                MessageBox.Show("Veuillez choisir un document !", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : inutilisable effectué !", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxRevues_SelectedIndexChanged"
                comboBoxRevues_SelectedIndexChanged(sender, e);
            }
        }



        #endregion


        #region Signaler

        // ALIMENTATION DE LA COMBOBOX et empecher d'ecrire dans les textBox
        private void tabSignaler_Enter(object sender, EventArgs e)
        {
            // attribue la source de données de la combobox 
            comboBoxDocPageSignaler.DataSource = lesDocuments;

            //définition du champ qui doit être affiché dans la combobox
            comboBoxDocPageSignaler.DisplayMember = "titre";

            comboBoxRevuePageSignaler.DataSource = lesRevues;

            comboBoxRevuePageSignaler.DisplayMember = "titre";

            //ReadOnly a true
            textBoxIdDoc.ReadOnly = true;
            textBoxNumExemplaire.ReadOnly = true;
            textBoxNomAbo.ReadOnly = true;
            textBoxPrenomAbo.ReadOnly = true;

            textBoxIdSignalerRevue.ReadOnly = true;
            textBoxNumeroSignalerRevue.ReadOnly = true;
            textBoxNomSignalerRevue.ReadOnly = true;
            textBoxPrenomSignalerRevue.ReadOnly = true;

            //Bloque les button
            buttonSignalerExemplaire.Enabled = false;
            buttonSignalerRevue.Enabled = false;

            // Définir la propriété ReadOnly au datagridview
            dataGridViewAbo.ReadOnly = true;

            dataGridViewDoc.ReadOnly = true;

            dataGridViewRev.ReadOnly = true;
        }


        //  appelée lorsqu'un document est sélectionné dans la combobox pour afficher les exemplaires correspondants dans le dataGridView
        private void comboBoxDocPageSignaler_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération du document sélectionné dans la combobox
            Document titreSelectionne = (Document)comboBoxDocPageSignaler.SelectedItem;

            // Vérification qu'un document a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des exemplaires pour le document sélectionné à partir de la base de données
                lesExemplaires = DAODocuments.getDocumentByTitre(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewDoc.Rows.Clear();

                // Parcours de la collection des exemplaires
                foreach (Exemplaire exemplaire in lesExemplaires)
                {
                    // si l'etat est differents de détérioré tu affiches 
                    if(exemplaire.Etat.Libelle != "détérioré" && exemplaire.Etat.Libelle != "inutilisable")
                    {
                        dataGridViewDoc.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Document.Titre, exemplaire.Numero, exemplaire.Etat.Libelle);
                    }
                }
            }
        }


        //  appelée lorsqu'une revue est sélectionné dans la combobox pour afficher les parutions correspondants dans le dataGridView
        private void comboBoxRevuePageSignaler_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération du document sélectionné dans la combobox
            Revue titreSelectionne = (Revue)comboBoxRevuePageSignaler.SelectedItem;

            // Vérification qu'un document a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des exemplaires pour le document sélectionné à partir de la base de données
                lesParutions = DAORevues.getParutionByTitre(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewRev.Rows.Clear();

                // Parcours de la collection des exemplaires
                foreach (Parution parution in lesParutions)
                {
                    // si l'etat est differents de détérioré tu affiches 
                    if (parution.Etat.Libelle != "détérioré" && parution.Etat.Libelle != "inutilisable")
                    {
                        dataGridViewRev.Rows.Add(parution.Revue.Id, parution.Revue.Titre, parution.Numero, parution.Etat.Libelle);
                    }
                }
            }
        }


        // Recherche nom d'un abonne
        private void textBoxRechercheNomAbo_TextChanged(object sender, EventArgs e)
        {
            dataGridViewAbo.Rows.Clear();

            // On parcourt tous les abonnes. Si le nom matche avec la saisie, on l'affiche dans le dataGridViewAbonnes.
            foreach (Abonne abonne in lesAbonnes)
            {
                // on passe le champ de saisie et le nom en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = textBoxRechercheNomAbo.Text.ToLower();
                string titreMinuscules;
                titreMinuscules = abonne.Nom.ToLower();


                //on teste si le nom de l'abonne contient ce qui a été saisi
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dataGridViewAbo.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString()); ;
                }
            }
        }


        // Affichage des abonnes dans le dataGridViewAbo
        private void dataGridViewAbo_SelectionChanged(object sender, EventArgs e)
        {
            // Vérification qu'une seule ligne est sélectionnée
            if (dataGridViewAbo.SelectedRows.Count == 1)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewAbo.SelectedRows[0];

                if(row != null && row.Cells["nomAbo"].Value != null)
                {
                    // Récupération des valeurs de chaque cellule de la ligne
                    string nom = row.Cells["nomAbo"].Value.ToString();
                    string prenom = row.Cells["prenomAbo"].Value.ToString();

                    
                    // Affichage des valeurs dans les TextBox correspondantes
                    textBoxNomAbo.Text = nom;
                    textBoxPrenomAbo.Text = prenom;
                    textBoxNomSignalerRevue.Text = nom;
                    textBoxPrenomSignalerRevue.Text = prenom;
                }
            }
        }


        // Affichage des documents dans le dataGridViewDoc
        private void dataGridViewDoc_SelectionChanged(object sender, EventArgs e)
        {
            // Vérification qu'une seule ligne est sélectionnée
            if (dataGridViewDoc.SelectedRows.Count == 1)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewDoc.SelectedRows[0];

                if(row != null && row.Cells["idDocument"].Value != null && row.Cells["numExemplaire"].Value != null)
                {
                    // Récupération des valeurs de chaque cellule de la ligne
                    string idDoc = row.Cells["idDocument"].Value.ToString();
                    string numero = row.Cells["numExemplaire"].Value.ToString();

                        // Affichage des valeurs dans les TextBox correspondantes
                        textBoxIdDoc.Text = idDoc;
                        textBoxNumExemplaire.Text = numero;

                    
                }
            }
        }


        // Affichage des revues dans le dataGridViewRev
        private void dataGridViewRev_SelectionChanged(object sender, EventArgs e)
        {
            // Vérification qu'une seule ligne est sélectionnée
            if (dataGridViewRev.SelectedRows.Count == 1)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewRev.SelectedRows[0];

                if (row != null && row.Cells["idRevue"].Value != null && row.Cells["numParution"].Value != null)
                {
                    // Récupération des valeurs de chaque cellule de la ligne
                    string idRevue = row.Cells["idRevue"].Value.ToString();
                    string numero = row.Cells["numParution"].Value.ToString();

                    // Affichage des valeurs dans les TextBox correspondantes
                    textBoxIdSignalerRevue.Text = idRevue;
                    textBoxNumeroSignalerRevue.Text = numero;
                }
            }
        }


        // BOUTON POUR CHANGER L'ETAT EN DETERIORE POUR LES EXEMPLAIRES
        private void buttonSignalerExemplaire_Click(object sender, EventArgs e)
        {
            try
            {
                // Si les textBox sont différents de vide
                if(textBoxIdDoc.Text!= "" && textBoxNumExemplaire.Text != "" && textBoxNomAbo.Text != "" && textBoxPrenomAbo.Text != "")
                {

                    // SPÉCIFIER La date
                    DateTime date = DateTime.Now;

                    // Modifie l'exemplaire en détériorer 
                    DAODocuments.modifierExemplaireDeteriore(textBoxIdDoc.Text , textBoxNumExemplaire.Text);
                    // AJOUTER L'ABONNE DANS LA BDD
                    DAODocuments.ajouterSignalement(textBoxIdDoc.Text, textBoxNumExemplaire.Text, textBoxNomAbo.Text, textBoxPrenomAbo.Text, date.Date);

                    //Affiche un message
                    MessageBox.Show("Signalement ajouté avec succès.", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxDocuments_SelectedIndexChanged"
                    comboBoxDocPageSignaler_SelectedIndexChanged(sender, e);

                    // Mettre à jour les données et rafraîchir le DataGridView
                    lesAbonnes = DAOAbonne.getAllAbonne();
                

                    // Parcour les abonnes pour les afficher dans le dataGridViewAbo
                    foreach (Abonne abonne in lesAbonnes)
                    {
                        dataGridViewAbo.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString());
                    }

                    // Parcour les abonnes pour les afficher dans le dataGridViewAbo
                    foreach (Exemplaire ex in lesExemplaires)
                    {
                        dataGridViewDoc.Rows.Add(ex.Document.IdDoc, ex.Document.Titre, ex.Numero, ex.Etat.Libelle);
                    }
                    dataGridViewDoc.Rows.Clear();
                    dataGridViewAbo.Rows.Clear();
                    textBoxRechercheNomAbo.Clear();

                    // VIDER LES CHAMPS DE TEXTE
                    textBoxIdDoc.Text = "";
                    textBoxPrenomAbo.Text = "";
                    textBoxNumExemplaire.Text = "";
                    textBoxNomAbo.Text = "";

                    textBoxNomSignalerRevue.Text = "";
                    textBoxPrenomSignalerRevue.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du signalement de l'abonné : " + ex.Message);
            }
        }


        // BOUTON POUR CHANGER L'ETAT EN DETERIORE POUR LES PARUTIONS
        private void buttonSignalerRevue_Click(object sender, EventArgs e)
        {
            try
            {
                // Si les textBox sont différents de vide
                if (textBoxIdSignalerRevue.Text != "" && textBoxNumeroSignalerRevue.Text != "" && textBoxNomSignalerRevue.Text != "" && textBoxPrenomSignalerRevue.Text != "")
                {
                    // GÉNÉRER UN ID UNIQUE
                    string id = Guid.NewGuid().ToString();

                    // SPÉCIFIER La date
                    DateTime date = DateTime.Now;

             

                
                    // Modifie l'exemplaire en détériorer 
                    DAORevues.modifierParutionDeteriore(textBoxIdSignalerRevue.Text, textBoxNumeroSignalerRevue.Text);
                    // AJOUTER L'ABONNE DANS LA BDD
                    DAORevues.ajouterSignalement(textBoxIdSignalerRevue.Text, textBoxNumeroSignalerRevue.Text, textBoxNomSignalerRevue.Text, textBoxPrenomSignalerRevue.Text, date.Date);

                    //Affiche un message
                    MessageBox.Show("Signalement ajouté avec succès.", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mettre à jour les données et rafraîchir le DataGridView
                    lesAbonnes = DAOAbonne.getAllAbonne();
                    lesParutions = DAORevues.getAllParution();

                    // Parcour les abonnes pour les afficher dans le dataGridViewAbo
                    foreach (Abonne abonne in lesAbonnes)
                    {
                        dataGridViewAbo.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString());
                    }

                    // Parcour les abonnes pour les afficher dans le dataGridViewAbo
                    foreach (Parution paru in lesParutions)
                    {
                        dataGridViewRev.Rows.Add(paru.Revue.Id, paru.Revue.Titre, paru.Numero, paru.Etat.Libelle);
                    }
                    dataGridViewRev.Rows.Clear();
                    dataGridViewAbo.Rows.Clear();
                    textBoxRechercheNomAbo.Clear();

                    // VIDER LES CHAMPS DE TEXTE
                    textBoxIdSignalerRevue.Text = "";
                    textBoxPrenomSignalerRevue.Text = "";
                    textBoxNumeroSignalerRevue.Text = "";
                    textBoxNomSignalerRevue.Text = "";

                    textBoxNomAbo.Text = "";
                    textBoxPrenomAbo.Text = "";


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du signalement de l'abonné : " + ex.Message);
            }
        }


        //Vérification de la saisie de champs  pour activer le bouton signaler exemplaire 
        private void verifTextBoxSignalerExemplaire(object sender, EventArgs e)
        {
            // Vérifier si tous les champs obligatoires sont remplis
            if (textBoxIdDoc.Text != "" && textBoxPrenomAbo.Text != "" && textBoxNumExemplaire.Text != "" && textBoxNomAbo.Text != "")
            {
              // Active le bouton
              buttonSignalerExemplaire.Enabled = true;

            }
            else
            {
                // Désactiver le bouton si tous les champs obligatoires ne sont pas remplis
                buttonSignalerExemplaire.Enabled = false;
            }
        }


        //Vérification de la saisie de champs  pour activer le bouton revue parution 
        private void verifTextBoxSignalerRevue(object sender, EventArgs e)
        {
            // Vérifier si tous les champs obligatoires sont remplis
            if (textBoxIdSignalerRevue.Text != "" && textBoxPrenomSignalerRevue.Text != "" && textBoxNumeroSignalerRevue.Text != "" && textBoxNomSignalerRevue.Text != "")
            {
                // Active le bouton
                buttonSignalerRevue.Enabled = true;

            }
            else
            {
                // Désactiver le bouton si tous les champs obligatoires ne sont pas remplis
                buttonSignalerRevue.Enabled = false;
            }
        }


        //Verification textBox Revue
        private void textBoxIdSignalerRevue_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerRevue(sender, e);
        }


        //Verification textBoxRevue
        private void textBoxNumeroSignalerRevue_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerRevue(sender, e);
        }


        //Verification textBox Revue
        private void textBoxNomSignalerRevue_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerRevue(sender, e);
        }


        //Verification textBox Revue
        private void textBoxPrenomSignalerRevue_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerRevue(sender, e);
        }


        //Verification textBox Exemplaire
        private void textBoxIdDoc_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerExemplaire(sender, e);
        }


        //Verification textBox Exemplaire
        private void textBoxNumExemplaire_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerExemplaire(sender, e);
        }


        //Verification textBox Exemplaire
        private void textBoxNomAbo_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerExemplaire(sender, e);
        }


        //Verification textBox Exemplaire
        private void textBoxPrenomAbo_TextChanged(object sender, EventArgs e)
        {
            verifTextBoxSignalerExemplaire(sender, e);
        }
        #endregion


        #region INUTILISABLE
        //-----------------------------------------------------------
        // ONGLET "DOCUMENT OU REVUE INUTILISABLE"
        //-----------------------------------------------------------
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            // attribue la source de données de la combobox 
            comboBoxDoc.DataSource = lesDocuments;

            //définition du champ qui doit être affiché dans la combobox
            comboBoxDoc.DisplayMember = "titre";

            comboBoxRev.DataSource = lesRevues;

            comboBoxRev.DisplayMember = "titre";

            // Rafraîchir les données de la DataGridView en appelant la méthode "comboBox_document_SelectedIndexChanged"
            comboBox_document_SelectedIndexChanged(sender, e);

            // Rafraîchir les données de la DataGridView en appelant la méthode "comboBox_Rev_SelectedIndexChanged"
            comboBox_Rev_SelectedIndexChanged(sender, e);

            // Définir la propriété ReadOnly au datagridview
            dataGridViewDocumentsInutilisable.ReadOnly = true;

            dataGridViewRevuesInutilisable.ReadOnly = true;
        }


        // appelée lorsqu'un document est sélectionné dans la combobox pour afficher les exemplaires inutilisable correspondants dans le dataGridView
        private void comboBox_document_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération du document sélectionné dans la combobox
            Document titreSelectionne = (Document)comboBoxDoc.SelectedItem;

            // Vérification qu'un document a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des exemplaires pour le document sélectionné à partir de la base de données
                lesExemplaires = DAODocuments.getDocumentByTitre(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewDocumentsInutilisable.Rows.Clear();

                // Parcours de la collection des exemplaires
                foreach (Exemplaire exemplaire in lesExemplaires)
                {
                    // Si l'etat est égale à inutilisable
                    if (exemplaire.Etat.Libelle == "inutilisable")
                    {
                        dataGridViewDocumentsInutilisable.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Document.Titre,  exemplaire.Numero,  exemplaire.Etat.Libelle);
                    }
                }
            }
        }


        //  appelée lorsqu'un document est sélectionné dans la combobox pour afficher les parutions inutilisable correspondants dans le dataGridView
        private void comboBox_Rev_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération de la revue sélectionné dans la combobox
            Revue titreSelectionne = (Revue)comboBoxRev.SelectedItem;

            // Vérification qu'une revue a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des parutions pour le document sélectionné à partir de la base de données
                lesParutions = DAORevues.getParutionByTitre(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewRevuesInutilisable.Rows.Clear();

                // Parcours de la collection des parutions
                foreach (Parution parution in lesParutions)
                {
                    // Si l'etat est égale à inutilisable
                    if (parution.Etat.Libelle == "inutilisable")
                    {
                        dataGridViewRevuesInutilisable.Rows.Add(parution.Revue.Id, parution.Revue.Titre, parution.Numero, parution.Etat.Libelle);
                    }
                }
            }
        }


       
        #endregion


        #region DETERIORES
        //-----------------------------------------------------------
        // ONGLET "DOCUMENT OU REVUE DETERIORE !n"
        //-----------------------------------------------------------

        // ALLIMENTATION COMBOBOX DES DOCUMENTS ET REVUES
        private void tabPage4_Enter(object sender, EventArgs e)
        {
            // attribue la source de données de la combobox 
            comboBoxDocumentDeteriore.DataSource = lesDocuments;

            //définition du champ qui doit être affiché dans la combobox
            comboBoxDocumentDeteriore.DisplayMember = "titre";
          
            // Value = idDoc
            comboBoxDocumentDeteriore.ValueMember = "idDoc";

            // attribue la source de données de la combobox 
            comboBoxParutionDeteriore.DataSource = lesRevues;

            //définition du champ qui doit être affiché dans la combobox
            comboBoxParutionDeteriore.DisplayMember = "titre";

            // Value = idRevue
            comboBoxParutionDeteriore.ValueMember = "id";

            // Définir la propriété ReadOnly au datagridview
            dataGridViewDocDeteriore.ReadOnly = true;

            dataGridViewRevDeteriore.ReadOnly = true;
        }


        //  appelée lorsqu'un document est sélectionné dans la combobox pour afficher les exemplaires correspondants dans le dataGridView
        private void comboBoxDocumentDeteriore_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération du document sélectionné dans la combobox
            Document titreSelectionne = (Document)comboBoxDocumentDeteriore.SelectedItem;

            // Vérification qu'un document a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des exemplaires pour le document sélectionné à partir de la base de données
                lesSignalerExemplaires = DAODocuments.getSignalerExemplairesByIdDoc(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewDocDeteriore.Rows.Clear();


                // Parcours de la collection des SignalerExemplaire
                foreach (SignalerExemplaire signaler in lesSignalerExemplaires)
                {
                    dataGridViewDocDeteriore.Rows.Add(signaler.Document.IdDoc , signaler.Document.Titre, signaler.Exemplaire.Numero , signaler.Nom, signaler.Prenom, signaler.Date.ToString("yyyy-MM-dd"));
                }

            }
        }


        //  appelée lorsqu'unz revue est sélectionné dans la combobox pour afficher les parutions correspondants dans le dataGridView
        private void comboBoxParutionDeteriore_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération du document sélectionné dans la combobox
            Revue titreSelectionne = (Revue)comboBoxParutionDeteriore.SelectedItem;

            // Vérification qu'un document a été sélectionné
            if (titreSelectionne != null)
            {
                // Récupération des exemplaires pour le document sélectionné à partir de la base de données
                lesSignalerParutions = DAORevues.getSignalerParutionByIdDoc(titreSelectionne);

                // Effacement des lignes existantes dans le dataGridView
                dataGridViewRevDeteriore.Rows.Clear();


                // Parcours de la collection des SignalerExemplaire
                foreach (SignalerParution signaler in lesSignalerParutions)
                {
                    dataGridViewRevDeteriore.Rows.Add(signaler.Revue.Id, signaler.Revue.Titre, signaler.Parution.Numero, signaler.Nom, signaler.Prenom, signaler.Date.ToString("yyyy-MM-dd"));
                }

            }
        }
        #endregion


        #region ABONNE
        // ONGLET ABONNE

        // Empeche l'utilisation de bouton 
        private void tabAbonne_Enter(object sender, EventArgs e)
        {
 
            textBoxID.ReadOnly = true;

            // bloquer le button 
            buttonSupprimer.Enabled = false;

            // Definir la propriété ReadOnly au textBox
            textBoxModifNom.ReadOnly = true;
            textBoxModifPrenom.ReadOnly = true;
            textBoxModifTelephone.ReadOnly = true;
            textBoxModifEmail.ReadOnly = true;
            textBoxModifAdresse.ReadOnly = true;

            // Définir la propriété ReadOnly au datagridview
            dataGridViewAbonnes.ReadOnly = true;
        }


        // AJOUTER UN ABONNE DANS LA BDD
        private void buttonAjouterAbonne_Click(object sender, EventArgs e)
        {
            try
            { 
                // Vérifier si tous les champs obligatoires sont remplis
                if (textBoxNom.Text == "" || textBoxPrenom.Text == "" || textBoxTelephone.Text == "" || textBoxAdresse.Text == "" || textBoxEmail.Text == "" )
                {
                    MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si le nom contient des caractères spéciaux ou de chiffres
                string regex = @"^[A-Za-zÀ-ÿ\s]+$";
                Regex regexNom = new Regex(regex);
                if (!regexNom.IsMatch(textBoxNom.Text))
                {
                     MessageBox.Show("Le nom saisi contient des caractères non valides.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                }

                // Vérifier si le prénom contient des caractères spéciaux ou de chiffres
                Regex regexPrenom = new Regex(regex);
                if (!regexPrenom.IsMatch(textBoxPrenom.Text))
                {
                     MessageBox.Show("Le prénom saisi contient des caractères non valides.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                }

                // Vérifier si le numéro de téléphone est valide
                regex = @"^[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}$";
                Regex regexTelephone = new Regex(regex);
                if (!regexTelephone.IsMatch(textBoxTelephone.Text))
                {
                    MessageBox.Show("Le numéro de téléphone saisi n'est pas valide.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si l'adresse contient des caractères spéciaux
                regex = @"^[A-Za-z0-9éà\s]+$";
                Regex regexAdresse = new Regex(regex);
                if (!regexAdresse.IsMatch(textBoxAdresse.Text))
                {
                    MessageBox.Show("L'adresse saisie contient des caractères non valides.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si l'adresse email est valide
                regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                Regex regexEmail = new Regex(regex);
                if (!regexEmail.IsMatch(textBoxEmail.Text))
                {
                    MessageBox.Show("L'adresse email saisie n'est pas valide.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si de la date de naissance 
                if (dateTimePickerDateNaissance.Value.Date == DateTime.Today)
                         {
                    MessageBox.Show("Date naissance saisie n'est pas valide.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // CREATION DE L'ABONNE ET L'AJOUTER DANS LA COLLECTION
                DateTime dateNaissance = dateTimePickerDateNaissance.Value;

                // SPÉCIFIER LES DATES DE DÉBUT ET DE FIN D'ABONNEMENT
                DateTime finAbonnement = DateTime.Now.AddDays(60);

                // AJOUTER L'ABONNE DANS LA BDD
                DAOAbonne.ajouterAbonne(textBoxNom.Text, textBoxPrenom.Text, textBoxTelephone.Text, textBoxAdresse.Text, textBoxEmail.Text, dateNaissance, finAbonnement);

                MessageBox.Show("Abonné ajouté avec succès.", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mettre à jour les données et rafraîchir le DataGridView
                lesAbonnes = DAOAbonne.getAllAbonne();

                // Parcour les abonnes pour les afficher dans le dataGridViewAbonnes
                foreach (Abonne abonne in lesAbonnes)
                {
                    dataGridViewAbonnes.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString(), abonne.Telephone.ToString(), abonne.Adresse.ToString(), abonne.Email.ToString(), abonne.DateNaissance.ToString("yyyy-MM-dd"), abonne.DebutAbonnement.ToString("yyyy-MM-dd"), abonne.FinAbonnement.ToString("yyyy-MM-dd"), abonne.Id.ToString());
                }

                dataGridViewAbonnes.Rows.Clear();
                // VIDER LES CHAMPS DE TEXTE
                textBoxNom.Text = "";
                textBoxPrenom.Text = "";
                textBoxTelephone.Text = "";
                textBoxAdresse.Text = "";
                textBoxEmail.Text = "";
                dateTimePickerDateNaissance.Value = DateTime.Now;
            }
            catch (ExceptionSIO ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de l'abonné : " + ex.Message);
            }
        }


        // Vérification de la date saisie dans le dataPicker DateNaissance
        private void dateTimePickerDateNaissance_ValueChanged(object sender, EventArgs e)
        {
            // Vérifier si l'année sélectionnée est inférieure à 1900
            if (dateTimePickerDateNaissance.Value.Year < 1900)
            {
                // Réinitialiser la valeur à la date du jour
                dateTimePickerDateNaissance.Value = DateTime.Now;
            }

            // Vérifier si la date sélectionnée est postérieure à la date du jour
            if (dateTimePickerDateNaissance.Value.Date > DateTime.Now.Date)
            {
                // Réinitialiser la valeur à la date du jour
                dateTimePickerDateNaissance.Value = DateTime.Now;
            }
        }


        //Comparer Informations Abonne
        private bool AbonneIdentique(Abonne abonne1, Abonne abonne2)
        {
            // Comparer les propriétés de l'abonné
            return abonne1.Nom == abonne2.Nom &&
                   abonne1.Prenom == abonne2.Prenom &&
                   abonne1.Telephone == abonne2.Telephone &&
                   abonne1.Adresse == abonne2.Adresse &&
                   abonne1.Email == abonne2.Email &&
                   abonne1.DateNaissance == abonne2.DateNaissance &&
                   abonne1.DebutAbonnement == abonne2.DebutAbonnement &&
                   abonne1.FinAbonnement == abonne2.FinAbonnement;
        }

      
        // Bouton pour modifier les informations d'un abonne dans la bdd
        private void buttonModifierAbonne_Click(object sender, EventArgs e)
        {
            try
            {
                // Vérifier si tous les champs obligatoires sont remplis
                if (textBoxModifNom.Text == "" || textBoxModifPrenom.Text == "" || textBoxModifTelephone.Text == "" || textBoxModifAdresse.Text == "" || textBoxModifEmail.Text == "")
                {
                    MessageBox.Show("Veuillez sélectionner un abonné dans le dataGridView.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si le nom contient des caractères spéciaux ou de chiffres
                string regex = @"^[A-Za-zÀ-ÿ\s]+$";
                Regex regexNom = new Regex(regex);
                if (!regexNom.IsMatch(textBoxModifNom.Text))
                {
                    MessageBox.Show("Le nom saisi contient des caractères non valides.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si le prénom contient des caractères spéciaux ou de chiffres
                Regex regexPrenom = new Regex(regex);
                if (!regexPrenom.IsMatch(textBoxModifPrenom.Text))
                {
                    MessageBox.Show("Le prénom saisi contient des caractères non valides.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si le numéro de téléphone est valide
                regex = @"^[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}$";
                Regex regexTelephone = new Regex(regex);
                if (!regexTelephone.IsMatch(textBoxModifTelephone.Text))
                {
                    MessageBox.Show("Le numéro de téléphone saisi n'est pas valide.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si l'adresse contient des caractères spéciaux
                regex = @"^[A-Za-z0-9éà\s]+$";
                Regex regexAdresse = new Regex(regex);
                if (!regexAdresse.IsMatch(textBoxModifAdresse.Text))
                {
                    MessageBox.Show("L'adresse saisie contient des caractères non valides.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si l'adresse email est valide
                regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                Regex regexEmail = new Regex(regex);
                if (!regexEmail.IsMatch(textBoxModifEmail.Text))
                {
                    MessageBox.Show("L'adresse email saisie n'est pas valide.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Vérifier si de la date de naissance 
                if (dateTimePickerModifDateNaissance.Value.Date == DateTime.Today)
                {
                    MessageBox.Show("Date naissance saisie n'est pas valide.", "Enregistrement échoué", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

              

                // Vérifier si la chaîne saisie correspond au modèle de format  valide
                if (regexEmail.IsMatch(textBoxModifEmail.Text) && regexNom.IsMatch(textBoxModifNom.Text) && regexPrenom.IsMatch(textBoxModifPrenom.Text) && regexTelephone.IsMatch(textBoxModifTelephone.Text) && regexAdresse.IsMatch(textBoxModifAdresse.Text))
                {
                    // Création d'un nouvel objet Abonne avec les nouvelles valeurs entrées dans les zones de texte 
                    Abonne abonneModifie = new Abonne(int.Parse(textBoxID.Text), textBoxModifNom.Text, textBoxModifPrenom.Text, textBoxModifTelephone.Text, textBoxModifAdresse.Text, textBoxModifEmail.Text, dateTimePickerModifDateNaissance.Value, dateTimePickerDebutAbonnement.Value, dateTimePickerFinAbonnement.Value);

                    // recuperation information de l'abonné dans la bdd
                    Abonne abo =  DAOAbonne.getRecupAbonneById(int.Parse(textBoxID.Text));


                    // compraison des informations de la bdd avec les infos des textBox
                    if (!AbonneIdentique(abonneModifie, abo))
                    {

                        // Modification de l'abonné dans la base de données avec les nouvelles valeurs
                        DAOAbonne.modifierAbonne(abonneModifie);

                        MessageBox.Show("Abonné modifié avec succès.", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Mettre à jour les données et rafraîchir le DataGridView
                        lesAbonnes = DAOAbonne.getAllAbonne();

                        // Parcours les abonnées pour afficher dans le dataGridViewAbonnes
                        foreach (Abonne abonne in lesAbonnes)
                        {
                            dataGridViewAbonnes.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString(), abonne.Telephone.ToString(), abonne.Adresse.ToString(), abonne.Email.ToString(), abonne.DateNaissance.ToString("yyyy-MM-dd"), abonne.DebutAbonnement.ToString("yyyy-MM-dd"), abonne.FinAbonnement.ToString("yyyy-MM-dd"), abonne.Id.ToString());
                        }

                        // CLEAR dataGridViewAbonnes
                        dataGridViewAbonnes.Rows.Clear();

                        // VIDER LES CHAMPS DE TEXTE
                        textBoxID.Text = "";
                        textBoxRechercheNom.Text = "";
                        textBoxModifNom.Text = "";
                        textBoxModifPrenom.Text = "";
                        textBoxModifTelephone.Text = "";
                        textBoxModifAdresse.Text = "";
                        textBoxModifEmail.Text = "";
                        dateTimePickerModifDateNaissance.Value = DateTime.Now;
                        dateTimePickerDebutAbonnement.Value = DateTime.Now;
                        dateTimePickerFinAbonnement.Value = DateTime.Now;

                        // Permet de ne pas ecrire dans les textBox
                        textBoxModifNom.ReadOnly = true;
                        textBoxModifPrenom.ReadOnly = true;
                        textBoxModifTelephone.ReadOnly = true;
                        textBoxModifEmail.ReadOnly = true;
                        textBoxModifAdresse.ReadOnly = true;
                    }
                }    
            }
            catch (ExceptionSIO ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la modification de l'abonné : " + ex.Message);
            }
        }


        // Vérification de la date saisie dans le dataPicker ModifDateNaissance
        private void dateTimePickerModifDateNaissance_ValueChanged(object sender, EventArgs e)
        {
            // Vérifier si l'année sélectionnée est inférieure à 1900
            if (dateTimePickerModifDateNaissance.Value.Year < 1900)
            {
                // Réinitialiser la valeur à la date du jour
                dateTimePickerModifDateNaissance.Value = DateTime.Now;
            }

            // Vérifier si la date sélectionnée est postérieure à la date du jour
            if (dateTimePickerModifDateNaissance.Value.Date > DateTime.Now.Date)
            {
                // Réinitialiser la valeur à la date du jour
                dateTimePickerModifDateNaissance.Value = DateTime.Now;
            }
        }


        // Vérification de la date saisie dans le dataPicker Debut Abonnement
        private void dateTimePickerDebutAbonnement_ValueChanged(object sender, EventArgs e)
        {
            // Vérifier si l'année sélectionnée est inférieure à 1900
            if (dateTimePickerDebutAbonnement.Value.Year < 1900)
            {
                // Réinitialiser la valeur à la date du jour
                dateTimePickerDebutAbonnement.Value = DateTime.Now;
            }
        }


        // Vérification de la date saisie dans le dataPicker Fin Abonnement
        private void dateTimePickerFinAbonnement_ValueChanged(object sender, EventArgs e)
        {
            // Vérifier si l'année sélectionnée est inférieure à 1900
            if (dateTimePickerDebutAbonnement.Value > dateTimePickerFinAbonnement.Value)
            {
                // Réinitialiser la valeur à la date de dateTimePickerDebutAbonnement + 60 jours
                dateTimePickerFinAbonnement.Value = dateTimePickerDebutAbonnement.Value.AddDays(60);
            }
        }


        // Recherche nom d'un abonne 
        private void textBoxRechercheNom_TextChanged(object sender, EventArgs e)
        {
            dataGridViewAbonnes.Rows.Clear();

            // On parcourt tous les abonnes. Si le nom matche avec la saisie, on l'affiche dans le dataGridViewAbonnes.
            foreach (Abonne abonne in lesAbonnes)
            {
                // on passe le champ de saisie et le nom en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = textBoxRechercheNom.Text.ToLower();
                string titreMinuscules;
                titreMinuscules = abonne.Nom.ToLower();


                //on teste si le nom de l'abonne contient ce qui a été saisi
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dataGridViewAbonnes.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString(), abonne.Telephone.ToString(), abonne.Adresse.ToString(), abonne.Email.ToString(), abonne.DateNaissance.ToString("yyyy-MM-dd"), abonne.DebutAbonnement.ToString("yyyy-MM-dd"), abonne.FinAbonnement.ToString("yyyy-MM-dd"), abonne.Id.ToString()); ;
                }
            }

        }


        // Affichage des abonnes dans le dataGridViewAbonnes
        private void dataGridViewAbonnes_SelectionChanged(object sender, EventArgs e)
        {
            // Vérification qu'une seule ligne est sélectionnée
            if (dataGridViewAbonnes.SelectedRows.Count == 1)
            {
                // Récupération de la ligne sélectionnée
                DataGridViewRow row = dataGridViewAbonnes.SelectedRows[0];
                if (row.Cells != null && row.Cells["ColumnNomAbo"].Value != null && row.Cells["ColumnPrenomAbo"].Value != null && row.Cells["ColumnTelAbo"].Value != null && row.Cells["ColumnAdresseAbo"].Value != null && row.Cells["ColumnEmailAbo"].Value != null && row.Cells["ColumnNaissanceAbo"].Value != null && row.Cells["ColumnDebutAbo"].Value != null && row.Cells["ColumnFinAbo"].Value != null && row.Cells["ColumnIdAbo"].Value != null)
                {
                    // Récupération des valeurs de chaque cellule de la ligne
                    string nom = row.Cells["ColumnNomAbo"].Value.ToString();
                    string prenom = row.Cells["ColumnPrenomAbo"].Value.ToString();
                    string telephone = row.Cells["ColumnTelAbo"].Value.ToString();
                    string adresse = row.Cells["ColumnAdresseAbo"].Value.ToString();
                    string email = row.Cells["ColumnEmailAbo"].Value.ToString();
                    DateTime dateNaissance = DateTime.Parse(row.Cells["ColumnNaissanceAbo"].Value.ToString());
                    DateTime debutAbonnement = DateTime.Parse(row.Cells["ColumnDebutAbo"].Value.ToString());
                    DateTime finAbonnement = DateTime.Parse(row.Cells["ColumnFinAbo"].Value.ToString());
                    string id = row.Cells["ColumnIdAbo"].Value.ToString();

                    // Affichage des valeurs dans les TextBox correspondantes
                    textBoxModifNom.Text = nom;
                    textBoxModifPrenom.Text = prenom;
                    textBoxModifTelephone.Text = telephone;
                    textBoxModifAdresse.Text = adresse;
                    textBoxModifEmail.Text = email;
                    dateTimePickerModifDateNaissance.Value = dateNaissance;
                    dateTimePickerDebutAbonnement.Value = debutAbonnement;
                    dateTimePickerFinAbonnement.Value = finAbonnement;
                    textBoxID.Text = id;

                    // Permet d'ecire dans les textBox
                    textBoxModifNom.ReadOnly = false;
                    textBoxModifPrenom.ReadOnly = false;
                    textBoxModifTelephone.ReadOnly = false;
                    textBoxModifEmail.ReadOnly = false;
                    textBoxModifAdresse.ReadOnly = false;

                    // Débloquer le button 
                    buttonSupprimer.Enabled = true;

                    // Vérifier si la date actuelle est supérieure ou égale à la fin de l'abonnement
                    if (DateTime.Now >= finAbonnement) 
                    {
                        MessageBox.Show("L'abonnement a expiré"); 
                    }
                    else
                    {
                        // Calculer la différence entre la date actuelle et la fin de l'abonnement
                        TimeSpan difference = finAbonnement - DateTime.Now;

                        // Convertir la différence en jours
                        int differenceEnJours = (int)difference.TotalDays;

                        // Vérifier si la différence est inférieure ou égale à 30 jours
                        if (differenceEnJours <= 30) 
                        {
                            MessageBox.Show("L'abonnement va bientot expirer dans : " + differenceEnJours.ToString() + " " + "jours"); 
                        }
                    }
                }
            }
        }


        //Supprimer un abonne
        private void buttonSupprimer_Click(object sender, EventArgs e)
        {
            if(textBoxID.Text!= "")
            {
                // Création d'un nouvel objet Abonne avec les nouvelles valeurs entrées dans les zones de texte 
                Abonne abonneSup = new Abonne(int.Parse(textBoxID.Text), textBoxModifNom.Text, textBoxModifPrenom.Text, textBoxModifTelephone.Text, textBoxModifAdresse.Text, textBoxModifEmail.Text, dateTimePickerModifDateNaissance.Value, dateTimePickerDebutAbonnement.Value, dateTimePickerFinAbonnement.Value);
                DAOAbonne.supprimerAbonne(abonneSup);
                MessageBox.Show("Abonne supprimé", "Enregistrement réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mettre à jour les données et rafraîchir le DataGridView
                lesAbonnes = DAOAbonne.getAllAbonne();

                // Parcours les abonnées pour afficher dans le dataGridViewAbonnes
                foreach (Abonne abonne in lesAbonnes)
                {
                    dataGridViewAbonnes.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString(), abonne.Telephone.ToString(), abonne.Adresse.ToString(), abonne.Email.ToString(), abonne.DateNaissance.ToString("yyyy-MM-dd"), abonne.DebutAbonnement.ToString("yyyy-MM-dd"), abonne.FinAbonnement.ToString("yyyy-MM-dd"), abonne.Id.ToString());
                }

                // CLEAR dataGridViewAbonnes
                dataGridViewAbonnes.Rows.Clear();

                // VIDER LES CHAMPS DE TEXTE
                textBoxID.Text = "";
                textBoxRechercheNom.Text = "";
                textBoxModifNom.Text = "";
                textBoxModifPrenom.Text = "";
                textBoxModifTelephone.Text = "";
                textBoxModifAdresse.Text = "";
                textBoxModifEmail.Text = "";
                dateTimePickerModifDateNaissance.Value = DateTime.Now;
                dateTimePickerDebutAbonnement.Value = DateTime.Now;
                dateTimePickerFinAbonnement.Value = DateTime.Now;

                // Permet de ne pas ecrire dans les textBox
                textBoxModifNom.ReadOnly = true;
                textBoxModifPrenom.ReadOnly = true;
                textBoxModifTelephone.ReadOnly = true;
                textBoxModifEmail.ReadOnly = true;
                textBoxModifAdresse.ReadOnly = true;
            }

        }

        #endregion


        #region Exit
        //Exit application
        private void FrmMediateq_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }





        #endregion

       
    }
}
