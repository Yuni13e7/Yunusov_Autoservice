using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Yunusov_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;
            var _currentClient = ЮнусовАвтоСервисEntities1.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }
        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            //_currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            Client selectedClient = ComboClient.SelectedItem as Client;
            _currentClientService.ClientID = selectedClient.ID;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                ЮнусовАвтоСервисEntities1.GetContext().ClientService.Add(_currentClientService);
            try
            {
                ЮнусовАвтоСервисEntities1.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 5)
            {
                TBEnd.Text = "";
                return;
            }
            if (!TimeSpan.TryParseExact(s, @"hh\:mm", null, out TimeSpan startTime))
            {
                MessageBox.Show("Введите корректное время в формате HH:mm (0-23 часов, 0-59 минут)");
                TBStart.Text = "";
                TBEnd.Text = "";
                return;
            }
            if (startTime.Hours > 23 || startTime.Minutes > 59)
            {
                MessageBox.Show("Введите корректное время: часы должны быть 0-23, минуты 0-59");
                TBStart.Text = "";
                TBEnd.Text = "";
                return;
            }
            int duration = _currentService.Duration;
            TimeSpan endTime = startTime.Add(TimeSpan.FromMinutes(duration));
            TBEnd.Text = endTime.ToString(@"hh\:mm");
        }
        private void TBStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9:]");
        }

    }
}
