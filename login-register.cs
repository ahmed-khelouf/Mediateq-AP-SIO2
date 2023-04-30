using Mediateq_AP_SIO2.metier;
using Mediateq_AP_SIO2.modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediateq_AP_SIO2
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        #region Register
        //Initialisation de la connexion à la base de données 
        private void frmRegister_Load(object sender, EventArgs e)
        {
            try
            {
                // Création de la connexion avec la base de données
                DAOFactory.creerConnection();

                // Chargement des objets en mémoire
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message );
            }
        }


        //Validation des informations d'enregistrement de l'utilisateur et ajout dans la base de données
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            //Vérification que les champs son vide ou non
            if(textBoxUserName.Text == "" && textBoxName.Text == "" && textBoxFirstName.Text == "" && textBoxPassword.Text == "" && textBoxCompPassword.Text == "")
            {
                MessageBox.Show("Les champs sont vides" , "Enregistrement échoué" , MessageBoxButtons.OK , MessageBoxIcon.Error);
            }else if (textBoxPassword.Text == textBoxCompPassword.Text)
            {


                // CRYPTER LE MOT DE PASSE AVEC SHA256
                string passwordHash = BitConverter.ToString(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(textBoxPassword.Text))).Replace("-", "");
                
                // Ajout d'un utilisateur dans la bdd
                DAOUtilisateur.ajouterUtilisateur(textBoxUserName.Text.ToString(), textBoxName.Text.ToString(), textBoxFirstName.Text.ToString(), passwordHash);

                //Clear textBox
                textBoxUserName.Text = "";
                textBoxName.Text = "";
                textBoxFirstName.Text = "";
                textBoxPassword.Text = "";
                textBoxCompPassword.Text = "";

                MessageBox.Show("Utilisateur créé", "Enregistrement reussi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("le mot de passe ne correspond pas" , "Enregistrement echoué", MessageBoxButtons.OK , MessageBoxIcon.Error);

                textBoxPassword.Text = "";
                textBoxCompPassword.Text = "";
                textBoxPassword.Focus();
            }
        }


        //Affichage ou masquage du texte  dans les champs de mot de passe
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPassword.Checked)
            {
                textBoxPassword.PasswordChar = '\0';
                textBoxCompPassword.PasswordChar = '\0';
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
                textBoxCompPassword.PasswordChar = '*';
            }
        }


        //Button Clear des champs de saisie
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxUserName.Text = "";
            textBoxName.Text = "";
            textBoxFirstName.Text = "";
            textBoxPassword.Text = "";
            textBoxCompPassword.Text = "";
            textBoxPassword.Focus();
        }


        //Retour à la page de connexion
        private void labelCliqueBackLogin_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Hide();
        }

        #endregion


    }
}
