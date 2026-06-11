using appProvaA1Enfermeiro.Model;
using System.Collections.ObjectModel;

namespace appProvaA1Enfermeiro.Views;

public partial class TelaListaEnfermeiro : ContentPage
{
    /*
     * A ObservableCollection é uma classe que armazena um array de objetos do tipo de Enfermeiro.
     * Utilizamos essa classe quando estamos apresentando um array de objetos ao usuário. Diferencial
     * dessa classe é que toda vez que um item é add, removido ou modificado no array de objetos a interface
     * gráfica também é atualizada. Assim as modificações feitas no array sempre estão atualizadas para o usuário.
     */
    ObservableCollection<Enfermeiro> listagemEnfermeiros = new ObservableCollection<Enfermeiro>();
    public TelaListaEnfermeiro()
    {
        InitializeComponent();
        /*
        * Referenciando a fonte itens (a serem mostrados ao usuário) a ListView é a ObservableCollection 
        * definida acima. Fazendo essa definição no construtor estamos amarrando a fonte de dados da ListView assim
        * que ela é criada.
        */
        lstEnfermeiros.ItemsSource = listagemEnfermeiros;
    }
    /*
     * Tratamento do evento de clique no ToolBarItem que fará a navegação da tela de listagem 
     * até a tela de cadastro de novo Enfermeiro. A navegação está envolvida em um try catch
     * e se algum problema acontecer a mensagem da exceção será mostrada ao usuário via DisplayAlert
     */
    private async void irTelaIncluirEnfermeiro(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new TelaIncluirEnfermeiro());

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro no Cadastro do Enfermeiro !!!!", ex.Message, "OK");
        }
    }
    /*
     * Método executado quando a página é exibida ao usuário.
     */
    protected async override void OnAppearing()
    {
        try
        {
            listagemEnfermeiros.Clear();
            List<Enfermeiro> temp = await App.Database.GetAll();
            temp.ForEach(i => listagemEnfermeiros.Add(i));
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
    private async void excluirEnfermeiro(object sender, EventArgs e)
    {
        try
        {
            /*
             * Reconhecendo qual foi a linha do ListView que disparou o evento de exclusão.
             */
            MenuItem itemSelecionado = sender as MenuItem;
            /*
            * Obtendo qual foi a Enfermeiro que estava anexada no BindingContext
            */
            Enfermeiro enfermeiroSelecionado = itemSelecionado.BindingContext as Enfermeiro;
            /*
            * Perguntando ao usuário se ele realmente deseja remover. Note o await para aguardar
            * a resposta do usuário antes de prosseguir com o código.
            */
            bool confirmacao = await DisplayAlert("Tem certeza que quer excluir o Enfermeiro?", $"Excluir {enfermeiroSelecionado.enfNome}", "Sim", "Não");
            if (confirmacao)
            {
                /*
                 * Removendo o registro do db3 via método Delete da classe crudSQLite
                 */
                await App.Database.Delete(enfermeiroSelecionado.enfID);
                /*
                 * Removendo o item da ObservableCollection também, que é automaticamente
                 * removida da visão do usuário na ListView.
                 */
                listagemEnfermeiros.Remove(enfermeiroSelecionado);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro na Exclusão do Enfermeiro !!!!", ex.Message, "OK");
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
            lstEnfermeiros.IsRefreshing = true;
            /*
            * Limpando a ObservableCollection antes de add os itens vindos da busca.
            */
            listagemEnfermeiros.Clear();
            List<Enfermeiro> temp = await App.Database.Search(busca);
            temp.ForEach(i => listagemEnfermeiros.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro na Busca de Enfermeiros !!!!", ex.Message, "OK");
        }
        finally
        {
            lstEnfermeiros.IsRefreshing = false;
        }
    }
    /*
     * Trata o evento ItemSelected da ListView navegando para a página de detalhes.
     */
    private void lstEnfermeirosItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        /*
         * Forma contraída de definir o BindingContext da página TelaAlterarEnfermeiro como sendo a
         * Enfermeiro que foi selecionada na ListView (item da ListView) e em seguida já
         * redirecionando na navegação.
         */
        try
        {
            Enfermeiro enfermeiro1 = e.SelectedItem as Enfermeiro;
            Navigation.PushAsync(new TelaAlterarEnfermeiro
            {
                BindingContext = enfermeiro1,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro Desconhecido na Seleção de Enfermeiro !!!!", ex.Message, "OK");
        }
    }
    private async void refCarregando(object sender, EventArgs e)
    {
        try
        {
            listagemEnfermeiros.Clear();
            List<Enfermeiro> temp = await App.Database.GetAll();
            temp.ForEach(i => listagemEnfermeiros.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro Desconhecido no carregamento de Enfermeiros !!!!", ex.Message, "OK");
        }
        finally
        {
            lstEnfermeiros.IsRefreshing = false;
        }
    }
}