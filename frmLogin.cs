using Mediateq_AP_SIO2.metier;
using Mediateq_AP_SIO2.modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediateq_AP_SIO2
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            
        }

        #region Variables globales

        static List<Utilisateur> lesUtilisateurs;


        #endregion


        #region Login


        //Initialisation de la connexion à la base de données et chargement des utilisateurs
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                // Création de la connexion avec la base de données
                DAOFactory.creerConnection();

                // Chargement des objets en mémoire
                lesUtilisateurs = DAOUtilisateur.getAllUtilisateur();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


        //Vérification des informations d'identification de l'utilisateur et ouverture de la fenêtre principale en cas de réussite
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            
            // Récupérer l'utilisateur avec le nom d'utilisateur donné
            Utilisateur user = DAOUtilisateur.recupereUtilisateur(textBoxUserName.Text);


            // CRYPTER LE MOT DE PASSE AVEC SHA256
            string passwordHash = BitConverter.ToString(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(textBoxPassword.Text))).Replace("-", "");

            // Comparer les hachages de mot de passe
            
            if (user.Password == passwordHash)
            {
                // Si les hachages sont identiques, connecter l'utilisateur et afficher le formulaire principal
                new FrmMediateq(user).Show(); //  crée une nouvelle instance de la classe FrmMediateq en passant l'objet user en tant que paramètre
                this.Hide();
            }
            else
            {
                // Sinon, afficher un message d'erreur
                MessageBox.Show("Nom d'utilisateur ou mot de passe invalide", "Connexion échouée", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxUserName.Text = "";
                textBoxPassword.Text = "";
                textBoxUserName.Focus();
            }
        }



        //Button Clear des champs de saisie
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxUserName.Text = "";
            textBoxPassword.Text = "";
            textBoxUserName.Focus();
        }


        // Affichage / masquage du caractère de mot de passe dans le champ de saisie de mot de passe en fonction de la case à cocher
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPassword.Checked)
            {
                textBoxPassword.PasswordChar = '\0';
             
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
            }
        }


        //Ouverture de la fenêtre d'inscription à partir de l'étiquette de création de compte
        private void labelCreateAccount_Click(object sender, EventArgs e)
        {
            new frmRegister().Show();
            this.Hide();
        }



        #endregion


        #region Exit
        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        #endregion
    }

}
