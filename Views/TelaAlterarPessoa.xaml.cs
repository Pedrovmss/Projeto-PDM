using appClassePessoaBD.Model;
namespace appClassePessoaBD.Views;

public partial class TelaAlterarPessoa : ContentPage
{
    public TelaAlterarPessoa()
    {
        InitializeComponent();
    }
    private async void ToolbarItemClickedSalvar(object sender, EventArgs e)
    {
        try
        {
            /*
             * Obtém qual foi a Pessoa anexada no BindingContext da página no momento que
             * ela foi criada e enviada para navegação.
             */
            Pessoa PessoaAnexada = BindingContext as Pessoa;

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
                 * Preenchendo a model de acordo com os valores dos Entry. Note que recuperamos a pesID
                 * do BindingContext, como feito acima.
                 */
                Pessoa pessoa1 = new Pessoa
                {
                    pesID = PessoaAnexada.pesID,
                    pesNome = txtNomePessoa.Text,
                    pesIdade = Convert.ToInt32(txtIdadePessoa.Text),
                };
                /*
                 * Método para atualizar o registro no arquivo db3. Note que o método recebe um model
                 * preenchido e neste deve conter o pesID para que seja feito o Where(Alteração)
                 * no comando Update.
                 */
                await App.Database.Update(pessoa1);

                await DisplayAlert("Pessoa Alterada  com Sucesso !!!!", "", "OK");

                await Navigation.PushAsync(new TelaListaPessoa());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro na Alteração da Pessoa !!!!", ex.Message, "OK");
        }
    }
}