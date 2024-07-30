using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows.Forms;
using appAPi.Model;

namespace appAPi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            // Method intentionally left empty.
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            dgProduit.DataSource = ServGetListProduit();
        }


        /// <summary>
        /// Permet d'obtenir la liste des produits
        /// </summary>
        /// <returns></returns>
        public List<Produit> ServGetListProduit()
        {
            HttpClient client = new HttpClient();
            List<Produit> services = new List<Produit>();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["gl2021/Values/GetEmployees"]).Result;

            // Lavavel Rest Api
            //var response = client.GetAsync("api/produit").Result;
            HttpResponseMessage response = client.GetAsync("api/produit").Result;

            // Wcf Soap Api
            //var response = client.GetAsync("api/Produits/GetProduit").Result;

            if (response.IsSuccessStatusCode)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                //var responseData = response.Content.ReadAsStringAsync().Result;
                services = JsonConvert.DeserializeObject<List<Produit>>(responseData);
            }
            return services;
        }

        /// <summary>
        /// Ajoute un produit 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            Produit p = new Produit
            {
                CodeProduit = txtCode.Text,
                DesignationProduit = txtDesignation.Text,
                PrixAchat = double.Parse(txtPrixAchat.Text),
                PrixUnitaire = double.Parse(txtPrixVente.Text),
                QuantiteMinimale = int.Parse(txtQteMin.Text),
                QuantiteMaximale = int.Parse(txtQteMax.Text),
                CodeCategorie = "C001"
            };
            _ = AddProduct(p);
            Effacer();
        }

        /// <summary>
        /// Efface les champs du formulaire d'ajout
        /// </summary>
        public void Effacer()
        {
            txtIdProduit.Clear();
            txtCode.Clear();
            txtDesignation.Clear();
            txtPrixAchat.Clear();
            txtPrixVente.Clear();
            txtQteMax.Clear();
            txtQteMin.Text = string.Empty;
            dgProduit.DataSource = ServGetListProduit();
        }

        /// <summary>
        /// Permet d'envoyer le produit à ajouter au serveur
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        public bool AddProduct(Produit produit)
        {
            bool rep = false;
            string Id = produit.idProduit > 0 ? produit.idProduit.ToString() : "0";
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "idProduit", Id},
                { "CodeProduit", produit.CodeProduit},
                { "DesignationProduit", produit.DesignationProduit },
                { "PrixAchat", produit.PrixAchat.ToString() },
                { "PrixUnitaire", produit.PrixUnitaire.ToString() },
                { "QuantiteMinimale", produit.QuantiteMinimale.ToString() },
                { "QuantiteMaximale", produit.QuantiteMaximale.ToString() },
                { "CodeCategorie", "C001" },
            };
            /*var values = new Dictionary<string, string>
            {
                { "idProduit", Id},
                { "CodeProduit", produit.CodeProduit},
                { "DesignationProduit", produit.DesignationProduit },
                { "PrixAchat", produit.PrixAchat.ToString() },
                { "PrixUnitaire", produit.PrixUnitaire.ToString() },
                { "QuantiteMinimale", produit.QuantiteMinimale.ToString() },
                { "QuantiteMaximale", produit.QuantiteMaximale.ToString() },
                { "CodeCategorie", "C001" },
            };*/
            /**
             * Lorsque vous créez un objet FormUrlEncodedContent en passant le dictionnaire 
             * values au constructeur, les paires clé-valeur du dictionnaire sont converties 
             * en une chaîne de caractères au format URL-encodé. Cela signifie que chaque paire 
             * clé-valeur est convertie en une chaîne sous la forme clé=valeur et que ces paires sont séparées par des &.
             * 
             * Exemple : idProduit=0&CodeProduit=1234&DesignationProduit=ProduitTest&PrixAchat=10.5&PrixUnitaire=12.0&QuantiteMinimale=5&QuantiteMaximale=100&CodeCategorie=C001
             * 
             * Pourquoi utiliser FormUrlEncodedContent ?
             *   Compatibilité avec les serveurs web : De nombreux serveurs web attendent que les données 
             *   de formulaire soient envoyées dans ce format lorsqu'ils traitent des requêtes POST ou PUT. 
             *   Cela les aide à facilement extraire et interpréter les données envoyées par le client.
             *
             *   Standardisation : L'encodage des formulaires URL-encodés est une méthode standardisée pour 
             *   envoyer des données dans le corps d'une requête HTTP. Elle est largement utilisée et bien 
             *   supportée par les serveurs et les frameworks web.
             */
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            //var content = new FormUrlEncodedContent(values);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    /**
                     * définit l'URL de base pour les requêtes envoyées par ce client. 
                     * Cela signifie que pour toutes les requêtes faites avec ce client, 
                     * cette URL de base sera utilisée comme préfixe.
                     */
                    client.BaseAddress = new Uri(uriString: System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
                    //client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
                    /**
                     * Efface les en-têtes Accept existants
                     */
                    client.DefaultRequestHeaders.Accept.Clear();
                    /**
                     * Ajoute un en-tête pour indiquer que le client accepte les réponses au format JSON.
                     */
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Laravel Rest APi
                    HttpResponseMessage response = Id.Equals("0") ? client.PostAsync("api/produit", content).Result : client.PutAsync($"api/produit/{int.Parse(Id)}", content).Result;
                    //var response = Id.Equals("0") ? client.PostAsync("api/produit", content).Result : client.PutAsync($"api/produit/{int.Parse(Id)}", content).Result;

                    // Wcf Soap Api
                    //var response = (Id.Equals("0")) ? client.PostAsync("api/Produits/PostProduit", content).Result : client.PutAsync($"api/Produits/PutProduit/{int.Parse(Id)}", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        rep = true;
                        string responseData = response.Content.ReadAsStringAsync().Result;
                        //var responseData = response.Content.ReadAsStringAsync().Result;
                        Produit pro = JsonConvert.DeserializeObject<Produit>(responseData);

                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.ToString());
            }
            return rep;
        }


        /// <summary>
        /// Supprime un produit
        /// </summary>
        /// <returns></returns>
        public bool DeleteProduct()
        {
            //Produit produit = new Produit();
            bool rep = false;

            int idProduit = int.Parse(dgProduit.CurrentRow.Cells[0].Value.ToString());

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Laravel Rest Api
                    HttpResponseMessage response = client.DeleteAsync($"api/produit/{idProduit}").Result;

                    // Wcf Soap Api
                    //var response = client.DeleteAsync($"api/Produits/DeleteProduit/{idProduit}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        rep = true;
                        string responseData = response.Content.ReadAsStringAsync().Result;
                        Produit produit = JsonConvert.DeserializeObject<Produit>(responseData);
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception)
            {

            }
            return rep;
        }

        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            bool result = DeleteProduct();
            _ = result ? MessageBox.Show("Suppression effectuée") : MessageBox.Show("Erreur lors de la suppression");

            Effacer();
        }


        /// <summary>
        /// Permet de remplir les champs du formulaire lorsque l'on double click 
        /// sur une ligne du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgProduit_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /* txtIdProduit.Text = dgProduit.CurrentRow.Cells[0].Value.ToString();
             txtCode.Text = dgProduit.CurrentRow.Cells[1].Value.ToString();
             txtDesignation.Text = dgProduit.CurrentRow.Cells[2].Value.ToString();
             txtPrixAchat.Text = dgProduit.CurrentRow.Cells[3].Value.ToString();
             txtQteMin.Text = dgProduit.CurrentRow.Cells[4].Value.ToString();
             txtQteMax.Text = dgProduit.CurrentRow.Cells[5].Value.ToString();
             //cbbCategorie.SelectedValue = dgProduit.CurrentRow.Cells[6].Value;
             txtPrixVente.Text = dgProduit.CurrentRow.Cells[7].Value.ToString();*/

            int idProduit = int.Parse(dgProduit.CurrentRow.Cells[0].Value.ToString());
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Laravel Rest APi
                HttpResponseMessage response = client.GetAsync($"api/produit/{idProduit}").Result;

                // Wcf Soap APi
                //var response = client.GetAsync($"api/Produits/GetProduit/{idProduit}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    Produit produit = JsonConvert.DeserializeObject<Produit>(responseData);

                    txtIdProduit.Text = produit.idProduit.ToString();
                    txtCode.Text = produit.CodeProduit;
                    txtDesignation.Text = produit.DesignationProduit;
                    txtPrixAchat.Text = produit.PrixAchat.ToString();
                    txtQteMin.Text = produit.QuantiteMinimale.ToString();
                    txtQteMax.Text = produit.QuantiteMaximale.ToString();
                    //cbbCategorie.SelectedValue = produit.CodeCategorie;
                    txtPrixVente.Text = produit.PrixUnitaire.ToString();
                }
            }
        }


        /// <summary>
        /// Modifie les informations d'un produit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifier_Click(object sender, EventArgs e)
        {
            Produit p = new Produit
            {
                idProduit = int.Parse(txtIdProduit.Text),
                CodeProduit = txtCode.Text,
                DesignationProduit = txtDesignation.Text,
                PrixAchat = double.Parse(txtPrixAchat.Text),
                PrixUnitaire = double.Parse(txtPrixVente.Text),
                QuantiteMinimale = int.Parse(txtQteMin.Text),
                QuantiteMaximale = int.Parse(txtQteMax.Text),
                CodeCategorie = "C001"
            };
            _ = AddProduct(p);
            Effacer();
        }
    }
}

