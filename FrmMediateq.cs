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
                lesTitres = DAORevues.getAllTitre();
                lesDvd = DAODocuments.getAllDvd();
                lesExemplaires = DAODocuments.getAllExemplaire();
                lesParutions = DAORevues.getAllParution();
                lesDocuments = DAODocuments.getAllDocument();
                lesRevues = DAORevues.getAllRevue();
                lesSignalerExemplaires = DAOSignalerExemplaires.getAllSignalementExemplaire();
                lesAbonnes = DAOAbonne.getAllAbonne();
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
                    dataGridViewDocument.Rows.Add(exemplaire.Document.IdDoc, exemplaire.Document.Titre, exemplaire.Numero, exemplaire.Etat.Libelle);
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
                    dataGridViewRevue.Rows.Add(parution.Revue.Id, parution.Revue.Titre, parution.Numero, parution.Etat.Libelle);
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

                // Récupérer l'exemplaire correspondant à l'index sélectionné
                Exemplaire exemplaire = lesExemplaires.ElementAt(selectedExemplaire);

                // Vérifier si l'état de l'exemplaire est déjà "usagé"
                if (exemplaire.Etat.Libelle == "usagé")
                {
                    // Afficher un message si l'exemplaire est déjà en usage
                    MessageBox.Show("Ce document est déjà en usage !");
                }
                else // Si l'exemplaire n'est pas déjà en usage
                {
                    // Modifier l'état de l'exemplaire à "usagé" dans la base de données 
                    DAODocuments.modifierExemplaireUsage(exemplaire);

                    // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                    reussi = true;
                }
            }
            else // Si la liste d'exemplaires est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner un document
                MessageBox.Show("Veuillez choisir un document !");
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : usagé effectué !");

                // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxDocuments_SelectedIndexChanged"
                comboBoxDocuments_SelectedIndexChanged(sender, e);
            }
        }


        // BOUTON POUR EXEMPLAIRE ETAT = INUTILISBALE 
        private void buttonDocumentInutilisable_Click(object sender, EventArgs e)
        {
            // Variable pour vérifier si le changement d'état a réussi
            bool reussi = false;

            // Vérifier si la liste d'exemplaires est non nulle
            if (lesExemplaires != null)
            {
                // Récupérer l'index de l'exemplaire sélectionné dans la DataGridView
                int selectedExemplaire = dataGridViewDocument.CurrentCell.RowIndex;

                // Récupérer l'exemplaire correspondant à l'index sélectionné
                Exemplaire exemplaire = lesExemplaires.ElementAt(selectedExemplaire);

                // Vérifier si l'état de l'exemplaire est déjà "inutilisable"
                if (exemplaire.Etat.Libelle == "inutilisable")
                {
                    // Afficher un message si l'exemplaire est déjà en inutilisable
                    MessageBox.Show("Ce document est déjà en inutilisable !");
                }
                else // Si l'exemplaire n'est pas déjà en inutilisable
                {
                    // Modifier l'état de l'exemplaire à "inutilisable" dans la base de données 
                    DAODocuments.modifierExemplaireInutilisable(exemplaire);

                    // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                    reussi = true;
                }
            }
            else // Si la liste d'exemplaires est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner un document
                MessageBox.Show("Veuillez choisir un document !");
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : inutilisable effectué !");

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
                // Récupérer l'index de la parution sélectionné dans la DataGridView
                int selectedParution = dataGridViewRevue.CurrentCell.RowIndex;

                // Récupérer la parution correspondant à l'index sélectionné
                Parution parution = lesParutions.ElementAt(selectedParution);

                // Vérifier si l'état de la parution est déjà "usagé"
                if (parution.Etat.Libelle == "usagé")
                {
                    // Afficher un message si la parution est déjà en usagé
                    MessageBox.Show("Cette parution est déjà en usagé !");
                }
                else // Si la parution n'est pas déjà en usagé
                {
                    // Modifier l'état de la parution à "usagé" dans la base de données 
                    DAORevues.modifierParutionUsage(parution);

                    // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                    reussi = true;
                }
            }
            else // Si la liste de parution est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner une parution
                MessageBox.Show("Veuillez choisir un document !");
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : usagé effectué !");

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
                // Récupérer l'index de la parution sélectionné dans la DataGridView
                int selectedParution = dataGridViewRevue.CurrentCell.RowIndex;

                // Récupérer la parution correspondant à l'index sélectionné
                Parution parution = lesParutions.ElementAt(selectedParution);

                // Vérifier si l'état de la parution est déjà "inutilisable"
                if (parution.Etat.Libelle == "inutilisable")
                {
                    // Afficher un message si la parution est déjà en inutilisable
                    MessageBox.Show("Cette parution est déjà en inutilisable !");
                }
                else // Si la parution n'est pas déjà en inutilisable
                {
                    // Modifier l'état de la parution à "inutilisable" dans la base de données 
                    DAORevues.modifierParutionInutilisable(parution);

                    // Mettre la variable "reussi" à "true" pour indiquer que le changement d'état a réussi
                    reussi = true;
                }
            }
            else // Si la liste de parution est nulle
            {
                // Afficher un message d'erreur pour demander à l'utilisateur de sélectionner une parution
                MessageBox.Show("Veuillez choisir un document !");
            }

            if (reussi) // Si le changement d'état a réussi
            {
                // Afficher un message de confirmation
                MessageBox.Show("Changement d'état : inutilisable effectué !");

                // Rafraîchir les données de la DataGridView en appelant la méthode "comboBoxRevues_SelectedIndexChanged"
                comboBoxRevues_SelectedIndexChanged(sender, e);
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
                            SignalerExemplaire signaler = new SignalerExemplaire(textBox13.Text, exemplaire, textBox15.Text, textBox14.Text);
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


        #region ABONNE
        // ONGLET ABONNE

        // Empeche l'utilisation du bouton 
        private void tabAbonne_Enter(object sender, EventArgs e)
        {
            buttonAjouterAbonne.Enabled = false;
            textBoxID.Visible = false;
        }

        // AJOUTER UN ABONNE DANS LA BDD
        private void buttonAjouterAbonne_Click(object sender, EventArgs e)
        {
            try
            {
                // Vérifier si tous les champs obligatoires sont remplis
                if (textBoxNom.Text != "" && textBoxNom.Text != "")
                {
                    // Activer le bouton si tous les champs obligatoires sont remplis
                    buttonAjouterAbonne.Enabled = true;
                }
                else
                {
                    // Désactiver le bouton si tous les champs obligatoires ne sont pas remplis
                    buttonAjouterAbonne.Enabled = false;
                }

                // CREATION DE l'abonne ET L'AJOUTE DANS LA COLLECTION
                DateTime dateNaissance = dateTimePickerDateNaissance.Value;

                // GÉNÉRER UN ID D'ABONNÉ UNIQUE
                string id = Guid.NewGuid().ToString();

                // SPÉCIFIER LES DATES DE DÉBUT ET DE FIN D'ABONNEMENT
                DateTime debutAbonnement = DateTime.Now;
                DateTime finAbonnement = debutAbonnement.AddDays(60);

                Abonne abo = new Abonne(id.ToString(), textBoxNom.Text, textBoxPrenom.Text, textBoxTelephone.Text, textBoxAdresse.Text, textBoxEmail.Text, dateNaissance, debutAbonnement, finAbonnement );

                // AJOUTER L'ABONNE DANS LA BDD
                DAOAbonne.ajouterAbonne(abo);

                MessageBox.Show("Abonné ajouté avec succès.");

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
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de l'abonné : " + ex.Message);
            }
        }


        //Vérification de la saisie de champs obligatoires pour activer le bouton Ajouter Abonné
        private void verifTextBox(object sender, EventArgs e)
        {
            // Vérifier si tous les champs obligatoires sont remplis
            if (textBoxNom.Text != "" && textBoxPrenom.Text != "" && textBoxTelephone.Text != "" && textBoxAdresse.Text != "" && textBoxEmail.Text != "" && dateTimePickerDateNaissance.Value.Date != DateTime.Today)
            {
                // Vérifier si le nom et le prénom ne contiennent pas de caractères spéciaux ou de chiffres
                string regex = @"^[A-Za-zÀ-ÿ\s]+$";
                Regex regexNomEtPrenom = new Regex(regex);

                // Vérifier si l'adresse email est valide
                regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                Regex regexEmail = new Regex(regex);

                // Vérifier si le numero de  telephone est valide
                regex = @"^[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}[- ]?[0-9]{2}$";
                Regex regexTelephone = new Regex(regex);

                // Vérifier si l'adresse est valide
                regex = @"^[A-Za-z0-9éà\s]+$";
                Regex regexAdresse = new Regex(regex);

                // Vérifier si la chaîne saisie correspond au modèle de format  valide
                if (regexEmail.IsMatch(textBoxEmail.Text) && regexNomEtPrenom.IsMatch(textBoxNom.Text) && regexNomEtPrenom.IsMatch(textBoxPrenom.Text) && regexTelephone.IsMatch(textBoxTelephone.Text) && regexAdresse.IsMatch(textBoxAdresse.Text))
                {
                    // Activer le bouton si tous les champs obligatoires sont remplis et qui respect le modèle de format valide
                    buttonAjouterAbonne.Enabled = true;
                }
                else
                {
                    // Désactiver si pas valide 
                    buttonAjouterAbonne.Enabled = false;
                }
            }
            else
            {
                // Désactiver le bouton si tous les champs obligatoires ne sont pas remplis
                buttonAjouterAbonne.Enabled = false;
            }
        }


        //Vérification de la saisie de champs
        private void textBoxNom_TextChanged(object sender, EventArgs e)
        {
            verifTextBox(sender, e);
        }


        //Vérification de la saisie de champs
        private void textBoxPrenom_TextChanged(object sender, EventArgs e)
        {
            verifTextBox(sender, e);
        }


        //Vérification de la saisie de champs
        private void textBoxTelephone_TextChanged(object sender, EventArgs e)
        {
            verifTextBox(sender, e);
        }


        //Vérification de la saisie de champs
        private void textBoxAdresse_TextChanged(object sender, EventArgs e)
        {
            verifTextBox(sender, e);
        }


        //Vérification de la saisie de champs
        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {
            verifTextBox(sender, e);
            string pattern = @" ^ ([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"; // Expression régulière pour valider l'adresse email
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(textBoxEmail.Text)) // Vérifier si la chaîne saisie correspond au modèle de format d'adresse email valide
            {
                // La chaîne saisie est une adresse email valide
                // Vous pouvez activer le bouton ou effectuer d'autres actions nécessaires
            }
            else
            {
                // La chaîne saisie n'est pas une adresse email valide
                // Vous pouvez désactiver le bouton ou effectuer d'autres actions nécessaires
            }
        }


        //Vérification de la saisie de champs
        private void dateTimePickerDateNaissance_ValueChanged(object sender, EventArgs e)
        {
            verifTextBox(sender, e);
        }


        // Bouton pour modifier les informations d'un abonne dans la bdd
        private void buttonModifierAbonne_Click(object sender, EventArgs e)
        {
            try
            {
                  // Création d'un nouvel objet Abonne avec les nouvelles valeurs entrées dans les zones de texte et les contrôles de date et d'heure
                  Abonne abonneModifie = new Abonne(textBoxID.Text, textBoxModifNom.Text, textBoxModifPrenom.Text, textBoxModifTelephone.Text, textBoxModifAdresse.Text, textBoxModifEmail.Text, dateTimePickerModifDateNaissance.Value, dateTimePickerDebutAbonnement.Value, dateTimePickerFinAbonnement.Value);

                  // Modification de l'abonné dans la base de données avec les nouvelles valeurs
                  DAOAbonne.modifierAbonne(abonneModifie);

                  MessageBox.Show("Abonné modifié avec succès.");

                  // Mettre à jour les données et rafraîchir le DataGridView
                  lesAbonnes = DAOAbonne.getAllAbonne();

                // Parcours les abonnées pour afficher dans le dataGridViewAbonnes
                foreach (Abonne abonne in lesAbonnes)
                  {
                      dataGridViewAbonnes.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString(), abonne.Telephone.ToString(), abonne.Adresse.ToString(), abonne.Email.ToString(), abonne.DateNaissance.ToString("yyyy-MM-dd"), abonne.DebutAbonnement.ToString("yyyy-MM-dd"), abonne.FinAbonnement.ToString("yyyy-MM-dd"), abonne.Id.ToString());
                  }
                  dataGridViewAbonnes.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la modification de l'abonné : " + ex.Message);
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
                    dataGridViewAbonnes.Rows.Add(abonne.Nom.ToString(), abonne.Prenom.ToString(), abonne.Telephone.ToString(), abonne.Adresse.ToString(), abonne.Email.ToString(), abonne.DateNaissance.ToString("yyyy-MM-dd"), abonne.DebutAbonnement.ToString("yyyy-MM-dd"), abonne.FinAbonnement.ToString("yyyy-MM-dd") , abonne.Id.ToString()); ;
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

            }
        }


        #endregion

       
    }
}
