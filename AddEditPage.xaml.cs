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
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace Yunusov_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentServise = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                _currentServise = SelectedService;
            DataContext = _currentServise;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");
            if (_currentServise.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");
            if (_currentServise.DiscountInt < 0 || _currentServise.DiscountInt > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");
            if (_currentServise.Duration == 0)
                errors.AppendLine("Укажите длительность услуги");
            if (_currentServise.Duration > 240)
                errors.AppendLine("Длительность не может быть больше 240 минут");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var context = ЮнусовАвтоСервисEntities1.GetContext();
            var existingService = context.Service.FirstOrDefault(p => p.Title == _currentServise.Title && p.ID != _currentServise.ID);

            if (existingService == null)
            {
                if (_currentServise.ID == 0)
                {
                    context.Service.Add(_currentServise);
                }
                else
                {
                    context.Entry(_currentServise).State = EntityState.Modified;
                }

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }
        }
    }
}