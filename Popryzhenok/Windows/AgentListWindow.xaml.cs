using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Popryzhenok.DatabaseModel;

namespace Popryzhenok.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AgentListWindow : Window
    {
        private const int ItemsPerPage = 10;

        int PerPage = 10;
        public int Page { get; set; } = 0;

        private int _CurrentPageIndex = 0;

        public List<Agent> FilteredAgentList { get; set; }
        public AgentListWindow()
        {
            InitializeComponent();
            GenerateButton();
            var agentTypeList = DatabaseHelper.GetAgentsType();
            agentTypeList.Insert(0, new AgentType { Title = "Все" });
            FilterComboBox.ItemsSource = agentTypeList;
            UpdateAgentList();
           
        }

        public void UpdateAgentList()
        {
            if (PoiskTextBox is null || SortComboBox is null || FilterComboBox is null)
                return;

            var agents = DatabaseHelper.GetAgents();

            var allAgentCount = agents.Count;

            if (PoiskTextBox.Text.Length != 0)
            {
                agents = agents.Where(a => a.Email.ToLower().Contains(PoiskTextBox.Text) || a.Phone.ToLower().Contains(PoiskTextBox.Text)).ToList();
            }
            switch (((ComboBoxItem)SortComboBox.SelectedItem).Content.ToString())
            {
                case "Наименование по возрастанию":
                    agents = agents.OrderBy(a => a.Title).ToList();
                    break;
                case "Наименование по убыванию":
                    agents = agents.OrderByDescending(a => a.Title).ToList();
                    break;
                //Не робит
                case "Размер скидки по возрастанию":
                    agents = agents.OrderBy(a => a.Sales).ToList();
                    break;
                //Не робит
                case "Размер скидки по убыванию":
                    agents = agents.OrderByDescending(a => a.Sales).ToList();
                    break;
                case "Приоритет агента по возрастанию":
                    agents = agents.OrderBy(a => a.Priority).ToList();
                    break;
                case "Приоритет агента по убыванию":
                    agents = agents.OrderByDescending(a => a.Priority).ToList();
                    break;
            }
            if (((AgentType)FilterComboBox.SelectedItem).Title != "Все")
            {
                agents = agents.Where(a => a.AgentType == (AgentType)FilterComboBox.SelectedItem).ToList();
            }
            
            AgentListView.ItemsSource = agents.Skip(_CurrentPageIndex * 10).Take(10).ToList(); 
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView agentListView = sender as ListView;
            if (agentListView.SelectedItems.Count == 0)
            {
                ChangeDiscountButton.Visibility = Visibility.Hidden;
            }
            else
            {
                ChangeDiscountButton.Visibility = Visibility.Visible;
            }
        }

        private void ChangeDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            new ChangePriorityAgentWindow(AgentListView.SelectedItems.Cast<Agent>().ToList()).ShowDialog();
            UpdateAgentList();
        }

        private void PoiskTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgentList();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgentList();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgentList();
        }

        private void GenerateButton()
        {

            NumberButtonStackPanel.Children.Clear();
            int pageCount = Convert.ToInt32(Math.Floor((double)DatabaseHelper.GetAgents().Count / ItemsPerPage));
            FilteredAgentList = DatabaseHelper.GetAgents();

            for (int i = 0; i < pageCount; i++)
            {
                if (ItemsPerPage * i > FilteredAgentList.Count)
                    continue;

                Button newButton = new Button()
                {
                    Content = i + 1,
                    Background = Brushes.Transparent,
                    Height = 25
                };
                newButton.Click += NewButton_Click; ;

                NumberButtonStackPanel.Children.Add(newButton);
            }
        }


        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _CurrentPageIndex = Convert.ToInt32((sender as Button).Content.ToString()) - 1;
            UpdateAgentList();
        }

        private void LeftNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentPageIndex > 0)
            {
                RightNumberButton.IsEnabled = true;
                _CurrentPageIndex--;
                UpdateAgentList();
            }
            else
            {
                LeftNumberButton.IsEnabled = false;
            }
        }

        private void RightNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (((DatabaseHelper.GetAgents().Count / 10)-1) > _CurrentPageIndex)
            {
                _CurrentPageIndex++;
                LeftNumberButton.IsEnabled = true;
                UpdateAgentList();
            }
            else
            {
                RightNumberButton.IsEnabled = false;
            }
        }
    }
}
