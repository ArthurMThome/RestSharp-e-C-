/// Arthur M. Thomé
/// 15 AUG 2020
/// Arthur Martins Thomé - Projeto RestSharp - Via CEP.

#region Statements

using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;

using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

#endregion

namespace ConsultaAPIViaCEP
{
    public partial class FrmMain : Form
    {
        #region Form Methods

        public FrmMain ( )
        {
            InitializeComponent ( );
        }

        #endregion

        #region Buttons Click

        /// <summary>
        /// Função chamada ao clicar no botão de consultar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click ( object sender, EventArgs e )
        {
            // Primeira coisa a fazer quando clicar no botão é limpar os campos.
            txtUF.Text = "";
            txtGIA.Text = "";
            txtCEP.Text = "";
            txtIBGE.Text = "";
            txtBairro.Text = "";
            txtUnidade.Text = "";
            txtLogradouro.Text = "";
            txtLocalidade.Text = "";
            txtComplemento.Text = "";

            try
            {
                // Checar se o usuário digitou os 9 dígitos no campo do CEP.
                if ( txtCEPBuscar.TextLength == 8 )
                {
                    //Formatar a string do BaseURL.
                    string _baseURL = string.Format ( "https://viacep.com.br/ws/{0}/json/", txtCEPBuscar.Text );
                    // Abrir Client do RestSharp.
                    RestClient _restClient = new RestClient ( _baseURL );
                    // Inicializar a requisição completa.
                    RestRequest _restRequest = new RestRequest ( Method.GET );
                    // Trazer os objetos de retorno, vai informar se foi bem sucedida ou não.
                    IRestResponse _restResponse = _restClient.Execute ( _restRequest );

                    // Tratando possíveis erros.
                    // ContentLength -1 quer dizer "BadRequest", alguma coisa aconteceu de errado na requisição.
                    if ( _restResponse.StatusCode == System.Net.HttpStatusCode.BadRequest )
                    {
                        // Mostrar ums mensagem para informar o usuário.
                        MessageBox.Show ( "Houve um problema com sua requisição: " + _restResponse.Content, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                    }
                    else
                    {
                        // Pegar os dados do JSON e transformar em um objeto da classe DadosRetorno.
                        DadosRetorno _dadosRetorno = new JsonDeserializer ( ).Deserialize < DadosRetorno > ( _restResponse );

                        // Checar se o CEP de retorno é null.
                        if ( string.IsNullOrEmpty ( _dadosRetorno.cep ) )
                        {
                            /*
                             * Por algum motivo que eu desconheço, essa formatação nao está funcionando.
                             * Quando eu descobrir o motivo, vou arrumar o código!
                            */
                            // Formatar cep para mostrar no erro.
                            string _cep = string.Format ( "{0:00000-000}", txtCEPBuscar.Text );

                            MessageBox.Show ( "CEP: " + _cep + ", não encontrado. ", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                            return;
                        }

                        // Preencher os campos do Windons Forms.
                        txtUF.Text = _dadosRetorno.uf;
                        txtGIA.Text = _dadosRetorno.gia;
                        txtCEP.Text = _dadosRetorno.cep;
                        txtIBGE.Text = _dadosRetorno.ibge;
                        txtBairro.Text = _dadosRetorno.bairro;
                        txtUnidade.Text = _dadosRetorno.unidade;
                        txtLogradouro.Text = _dadosRetorno.logradouro;
                        txtLocalidade.Text = _dadosRetorno.localidade;
                        txtComplemento.Text = _dadosRetorno.complemento;

                        // Limpar texto do label de CEP para pesquisar.
                        txtCEPBuscar.Text = "";
                    }
                }
                else
                {
                    // Erro pois o usuário nao digitou os 9 digitos do CEP.
                    MessageBox.Show ( "O CEP não está completo, você esqueceu algum dígito!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                }
            }
            catch ( Exception erro )
            {
                // Algum erro sem tratamento, mostrar um erro geral.
                MessageBox.Show ( "Erro geral ao consultar a API: " + erro.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        /// <summary>
        /// Função chamada ao clicar no botão sair.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSair_Click ( object sender, EventArgs e )
        {
            // Mostrar Message Box perguntando se o usuário quer mesmo sair da aplicação.
            if ( MessageBox.Show ( "Obrigado por utilizar minha aplicação. " +
                                   "\nEspero que tenha gostado! " +
                                   "\n\nSair da aplicação?", "Saindo...", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
            {
                // Caso ele clique no botão "YES", fechar a aplicação;
                Application.Exit ( );
            }
        }

        #endregion
    }

    public class DadosRetorno
    {
        public string uf { get; set; }
        public string cep { get; set; }
        public string gia { get; set; }
        public string ibge { get; set; }
        public string bairro { get; set; }
        public string unidade { get; set; }
        public string logradouro { get; set; }
        public string localidade { get; set; }
        public string complemento { get; set; }
    }
}
