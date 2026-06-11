using appClassePessoaBD.Model;
namespace appClassePessoaBD.Views;

public partial class TelaIncluirPessoa : ContentPage
{
    public TelaIncluirPessoa()
    {
        InitializeComponent();
    }
    /*
     * Trata o evento Clicked do ToolbarItem
     */
    private async void ToolbarItemClickedSalvar(object sender, EventArgs e)
    {
        try
        {
            //Verificando se os elementos Entry estão vazios ou nulos
            if ((string.IsNullOrWhiteSpace(txtNomePessoa.Text)))
            {
                DisplayAlert("Erro", "Verifique se a caixa de texto Nome da Pessoa está vazia !!!!", "OK");
                txtNomePessoa.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtIdadePessoa.Text))
            {
                DisplayAlert("Erro", "Verifique se a caixa de texto Idade da Pessoa está vazia !!!!", "OK");
                txtIdadePessoa.Focus();
            }
            else
            {
                /*
                 * Preenchendo o model Pessoa com os dados informados na interface gráfica.
                */
                Pessoa pessoa1 = new Pessoa
                {
                    pesNome = txtNomePessoa.Text,
                    pesIdade = Convert.ToInt32(txtIdadePessoa.Text),
                };
                /*
                * Chamando o método insert da cruSQLite para fazer a inserção do
                * novo registro no arquivo db3 com os dados da model preenchida acima.
                * O await denota que o código deve esperar o insert para prosseguir.
                */
                await App.Database.Insert(pessoa1);
                /*
                 * Avisando o usuário que deu certo.
                 */
                await DisplayAlert("Pessoa Cadastrada com Sucesso !!!!", "", "OK");
                /*
                 * Navegando para a tela de listagem. 
                 */
                await Navigation.PushAsync(new TelaListaPessoa());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro no Cadastro da Pessoa !!!!", ex.Message, "OK");
            txtNomePessoa.Text = "";
            txtIdadePessoa.Text = "";
            txtNomePessoa.Focus();
        }
    }
}
