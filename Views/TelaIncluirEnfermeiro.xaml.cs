using appProvaA1Enfermeiro.Model;
namespace appProvaA1Enfermeiro.Views;

public partial class TelaIncluirEnfermeiro : ContentPage
{
    public TelaIncluirEnfermeiro()
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
            if ((string.IsNullOrWhiteSpace(txtNomeEnfermeiro.Text)))
            {
                DisplayAlert("Erro", "Verifique se a caixa de texto Nome do Enfermeiro está vazia !!!!", "OK");
                txtNomeEnfermeiro.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtEspecialidadeEnfermeiro.Text))
            {
                DisplayAlert("Erro", "Verifique se a caixa de texto Especialidade do Enfermeiro está vazia !!!!", "OK");
                txtEspecialidadeEnfermeiro.Focus();
            }
            else
            {
                /*
                 * Preenchendo o model Enfermeiro com os dados informados na interface gráfica.
                */
                Enfermeiro enfermeiro1 = new Enfermeiro
                {
                    enfNome = txtNomeEnfermeiro.Text,
                    enfEspecialidade = txtEspecialidadeEnfermeiro.Text,
                };
                /*
                * Chamando o método insert da cruSQLite para fazer a inserção do
                * novo registro no arquivo db3 com os dados da model preenchida acima.
                * O await denota que o código deve esperar o insert para prosseguir.
                */
                await App.Database.Insert(enfermeiro1);
                /*
                 * Avisando o usuário que deu certo.
                 */
                await DisplayAlert("Enfermeiro Cadastrado com Sucesso !!!!", "", "OK");
                /*
                 * Navegando para a tela de listagem. 
                 */
                await Navigation.PushAsync(new TelaListaEnfermeiro());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro no Cadastro do Enfermeiro !!!!", ex.Message, "OK");
            txtNomeEnfermeiro.Text = "";
            txtEspecialidadeEnfermeiro.Text = "";
            txtNomeEnfermeiro.Focus();
        }
    }
}
