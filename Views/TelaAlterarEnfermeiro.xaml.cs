using appProvaA1Enfermeiro.Model;
namespace appProvaA1Enfermeiro.Views;

public partial class TelaAlterarEnfermeiro : ContentPage
{
    public TelaAlterarEnfermeiro()
    {
        InitializeComponent();
    }
    private async void ToolbarItemClickedSalvar(object sender, EventArgs e)
    {
        try
        {
            /*
             * Obtém qual foi a Enfermeiro anexada no BindingContext da página no momento que
             * ela foi criada e enviada para navegação.
             */
            Enfermeiro EnfermeiroAnexado = BindingContext as Enfermeiro;

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
                 * Preenchendo a model de acordo com os valores dos Entry. Note que recuperamos a enfID
                 * do BindingContext, como feito acima.
                 */
                Enfermeiro enfermeiro1 = new Enfermeiro
                {
                    enfID = EnfermeiroAnexado.enfID,
                    enfNome = txtNomeEnfermeiro.Text,
                    enfEspecialidade = txtEspecialidadeEnfermeiro.Text,
                };
                /*
                 * Método para atualizar o registro no arquivo db3. Note que o método recebe um model
                 * preenchido e neste deve conter o enfID para que seja feito o Where(Alteração)
                 * no comando Update.
                 */
                await App.Database.Update(enfermeiro1);

                await DisplayAlert("Enfermeiro Alterado com Sucesso !!!!", "", "OK");

                await Navigation.PushAsync(new TelaListaEnfermeiro());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro na Alteração do Enfermeiro !!!!", ex.Message, "OK");
        }
    }
}