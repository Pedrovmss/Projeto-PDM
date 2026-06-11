using appClassePessoaBD.Model;
using System.Collections.ObjectModel;

namespace appClassePessoaBD.Views;

public partial class TelaListaPessoa : ContentPage
{
    /*
     * A ObservableCollection é uma classe que armazena um array de objetos do tipo de Pessoa.
     * Utilizamos essa classe quando estamos apresentando um array de objetos ao usuário. Diferencial
     * dessa classe é que toda vez que um item é add, removido ou modificado no array de objetos a interface
     * gráfica também é atualizada. Assim as modificações feitas no array sempre estão atualizadas para o usuário.
     */
    ObservableCollection<Pessoa> listagemPessoas = new ObservableCollection<Pessoa>();
    public TelaListaPessoa()
    {
        InitializeComponent();
        /*
        * Referenciando a fonte itens (a serem mostrados ao usuário) a ListView é a ObservableCollection 
        * definida acima. Fazendo essa definição no construtor estamos amarrando a fonte de dados da ListView assim
        * que ela é criada.
        */
        lstPessoas.ItemsSource = listagemPessoas;
    }
    /*
     * Tratamento do evento de clique no ToolBarItem que fará a navegação da tela de listagem 
     * até a tela de cadastro de nova Pessoa. A navegação está envolvida em um try catch
     * e se algum problema acontecer a mensagem da exceção será mostrada ao usuário via DisplayAlert
     */
    private async void irTelaIncluirPessoa(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new TelaIncluirPessoa());

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro no Cadastro da Pessoa !!!!", ex.Message, "OK");
        }
    }
    /*
     * Método executado quando a página é exibida ao usuário.
     */
    protected async override void OnAppearing()
    {
        try
        {
            listagemPessoas.Clear();
            List<Pessoa> temp = await App.Database.GetAll();
            temp.ForEach(i => listagemPessoas.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro Desconhecido no Carregamento da Lista !!!!", ex.Message, "OK");
        }
    }
    /*
     * Trata o evento Clicked do MenuItem da ViewCell.ContextActions perguntando ao usuário
     * se ele realmente deseja remover aquele item do arquivo db3
     */
    private async void excluirPessoa(object sender, EventArgs e)
    {
        try
        {
            /*
             * Reconhecendo qual foi a linha do ListView que disparou o evento de exclusão.
             */
            MenuItem itemSelecionado = sender as MenuItem;
            /*
            * Obtendo qual foi a Pessoa que estava anexada no BindingContext
            */
            Pessoa pessoaSelecionada = itemSelecionado.BindingContext as Pessoa;
            /*
            * Perguntando ao usuário se ele realmente deseja remover. Note o await para aguardar
            * a resposta do usuário antes de prosseguir com o código.
            */
            bool confirmacao = await DisplayAlert("Tem Certeza que quer excluir a Pessoa?", $"Excluir {pessoaSelecionada.pesNome}", "Sim", "Não");
            if (confirmacao)
            {
                /*
                 * Removendo o registro do db3 via método Delete da classe crudSQLite
                 */
                await App.Database.Delete(pessoaSelecionada.pesID);
                /*
                 * Removendo o item da ObservableCollection também, que é automaticamente
                 * removida da visão do usuário na ListView.
                 */
                listagemPessoas.Remove(pessoaSelecionada);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro na Exclusão da Pessoa !!!!", ex.Message, "OK");
        }
    }
    /*
     * Trata o evento TextChanged da SearchBar recebendo os novos valores digitados
     */
    private async void txtBuscar(object sender, TextChangedEventArgs e)
    {
        try
        {
            /*
             * Obtendo o valor que foi digitado no Search
             */
            string busca = e.NewTextValue;
            lstPessoas.IsRefreshing = true;
            /*
            * Limpando a ObservableCollection antes de add os itens vindos da busca.
            */
            listagemPessoas.Clear();
            List<Pessoa> temp = await App.Database.Search(busca);
            temp.ForEach(i => listagemPessoas.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro na Busca de Pessoas !!!!", ex.Message, "OK");
        }
        finally
        {
            lstPessoas.IsRefreshing = false;
        }
    }
    /*
     * Trata o evento ItemSelected da ListView navegando para a página de detalhes.
     */
    private void lstPessoasItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        /*
         * Forma contraída de definir o BindingContext da página TelaAlterarPessoa como sendo a
         * Pessoa que foi selecionada na ListView (item da ListView) e em seguida já
         * redirecionando na navegação.
         */
        try
        {
            Pessoa pessoa1 = e.SelectedItem as Pessoa;
            Navigation.PushAsync(new TelaAlterarPessoa
            {
                BindingContext = pessoa1,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro Desconhecido na Seleção de Pessoa !!!!", ex.Message, "OK");
        }
    }
    private async void refCarregando(object sender, EventArgs e)
    {
        try
        {
            listagemPessoas.Clear();
            List<Pessoa> temp = await App.Database.GetAll();
            temp.ForEach(i => listagemPessoas.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro Desconhecido no carregamento de Pessoas !!!!", ex.Message, "OK");
        }
        finally
        {
            lstPessoas.IsRefreshing = false;
        }
    }
}