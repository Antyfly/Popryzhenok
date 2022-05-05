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
using System.Windows.Shapes;
using Popryzhenok.DatabaseModel; 

namespace Popryzhenok.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangePriorityAgentWindow.xaml
    /// </summary>
    public partial class ChangePriorityAgentWindow : Window
    {
        private List<Agent> _AgentList;
        public ChangePriorityAgentWindow(List<Agent> agents)
        {
            InitializeComponent();
            _AgentList = agents;
            NewPriorityTextBox.Text = _AgentList.Max(m=> m.Priority).ToString();
            
        }

        private void ChangePriority_Click(object sender, RoutedEventArgs e)
        {
            if (NewPriorityTextBox.Text.Length == 0)
            {
                MessageBox.Show("Необходимо ввести значение",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newPriority = NewPriorityTextBox.Text;
            int intNewPriority = 0;
            try
            {
                intNewPriority = Convert.ToInt32(newPriority);
            }
            catch (Exception)
            {
                MessageBox.Show("Значение должно быть целым",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            if (intNewPriority<0)
            {
                MessageBox.Show("Значение должно быть положительным",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            foreach(var agent in _AgentList)
            {
                agent.Priority = Convert.ToInt32(NewPriorityTextBox.Text);
                DatabaseHelper.SaveChange();
            }
            MessageBox.Show("Приоритет изменен",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
