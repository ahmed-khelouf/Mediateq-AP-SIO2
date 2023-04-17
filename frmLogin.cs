using Mediateq_AP_SIO2.metier;
using Mediateq_AP_SIO2.modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Récupérer l'utilisateur avec le nom d'utilisateur donné
            Utilisateur utilisateur = new Utilisateur("", textBoxUserName.Text, "", "", textBoxPassword.Text);
            DAOUtilisateur.recupereUtilisateur(utilisateur);

           

            // Comparer les hachages de mot de passe
            if (textBoxPassword.Text == utilisateur.Password)
            {
                // Si les hachages sont identiques, connecter l'utilisateur et afficher le formulaire principal
                new FrmMediateq().Show();
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




        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxUserName.Text = "";
            textBoxPassword.Text = "";
            textBoxUserName.Focus();
        }

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

        private void labelCreateAccount_Click(object sender, EventArgs e)
        {
            new frmRegister().Show();
            this.Hide();
        }
    }
}
