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
using TaskManager.DB;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для Add_Edit.xaml
    /// </summary>
    public partial class Add_Edit : Window
    {
        int _id = -1;
        public Add_Edit(int id)
        {
            InitializeComponent();
            _id = id;
            if (id > -1)
            {
                Output();
            }
        }

        private void Output()
        {
            using (MainContext db = new MainContext())
            {
                DB.Task editedTask = db.Tasks.FirstOrDefault(x => x.Id == _id);
                taskName.Text = editedTask.Name;
                taskDescription.Text = editedTask.Description;
                taskPriority.Text = editedTask.Priority;
                dateChoose.SelectedDate = editedTask.DeadlineDate;
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_id == -1 && dateChoose.SelectedDate != null)
            {
                MessageBoxResult result = MessageBox.Show("Добавить задачу?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (MainContext db = new MainContext())
                    {
                        DB.Task newTask = new DB.Task(taskName.Text, taskDescription.Text, DateTime.Now, (DateTime)dateChoose.SelectedDate, taskPriority.Text, 0);
                        db.Tasks.Add(newTask);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Задача успешно добавлена", "Уведомление");
                    this.DialogResult = true;
                    this.Close();
                }
            }
            else if (dateChoose.SelectedDate != null)
            {
                MessageBoxResult result = MessageBox.Show("Изменить задачу?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (MainContext db = new MainContext())
                    {
                        DB.Task newTask = new DB.Task(taskName.Text, taskDescription.Text, DateTime.Now, (DateTime)dateChoose.SelectedDate, taskPriority.Text, 0);
                        DB.Task editedTask = db.Tasks.FirstOrDefault(x => x.Id == _id);
                        if (editedTask != null)
                        {
                            editedTask.Name = taskName.Text;
                            editedTask.Description = taskDescription.Text;
                            editedTask.DeadlineDate = (DateTime)dateChoose.SelectedDate;
                            editedTask.Priority = taskPriority.Text;
                            db.SaveChanges();
                        }
                    }

                    this.DialogResult = true;
                    this.Close();
                    MessageBox.Show("Изменения сохранены", "Уведомление");
                }
            }
        }
    }
}
