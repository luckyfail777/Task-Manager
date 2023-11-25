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
using TaskManager.DB;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            int id = -1;
            Add_Edit editTask = new Add_Edit(id);
            if (editTask.ShowDialog() == true)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            ToDo.ItemsSource = null;
            InProgress.ItemsSource = null;
            Done.ItemsSource = null;

            List<DB.Task> toDoList = new List<DB.Task>();
            List<DB.Task> WIPList = new List<DB.Task>();
            List<DB.Task> doneList = new List<DB.Task>();

            using (MainContext db = new MainContext())
            {
                var tasks = db.Tasks.ToList();

                for (int ind = 0; ind < tasks.Count; ind++)
                {
                    if (tasks[ind].Status == 0)
                    {
                        toDoList.Add(tasks[ind]);
                    }
                    else if (tasks[ind].Status == 1)
                    {
                        WIPList.Add(tasks[ind]);
                    }
                    else if (tasks[ind].Status == 2)
                    {
                        doneList.Add(tasks[ind]);
                    }
                }
            }

            ToDo.ItemsSource = toDoList;
            InProgress.ItemsSource = WIPList;
            Done.ItemsSource = doneList;
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var tag = int.TryParse(((Button)sender).Tag.ToString(), out int id);

            Add_Edit editTask = new Add_Edit(id);
            if (editTask.ShowDialog() == true)
            {
                Refresh();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var tag = int.TryParse(((Button)sender).Tag.ToString(), out int id);

            using (MainContext db = new MainContext())
            {
                DB.Task task = db.Tasks.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                }

                Refresh();
            }
        }

        private void ToWIP_Click(object sender, RoutedEventArgs e)
        {
            var tag = int.TryParse(((Button)sender).Tag.ToString(), out int id);

            using (MainContext db = new MainContext())
            {
                DB.Task task = db.Tasks.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    task.Status = 1;
                    db.SaveChanges();
                }

                Refresh();
            }
        }

        private void ToDone_Click(object sender, RoutedEventArgs e)
        {
            var tag = int.TryParse(((Button)sender).Tag.ToString(), out int id);

            using (MainContext db = new MainContext())
            {
                DB.Task task = db.Tasks.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    task.Status = 2;
                    db.SaveChanges();
                }

                Refresh();
            }
        }

        private void searchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (searchBar.Text != "")
            {
                ToDo.ItemsSource = null;
                InProgress.ItemsSource = null;
                Done.ItemsSource = null;

                List<DB.Task> toDoList = new List<DB.Task>();
                List<DB.Task> WIPList = new List<DB.Task>();
                List<DB.Task> doneList = new List<DB.Task>();

                using (MainContext db = new MainContext())
                {
                    var tasks = db.Tasks.ToList();
                    tasks = tasks.Where<DB.Task>(x => x.Name.Contains(searchBar.Text)).ToList();

                    for (int ind = 0; ind < tasks.Count; ind++)
                    {
                        if (tasks[ind].Status == 0)
                        {
                            toDoList.Add(tasks[ind]);
                        }
                        else if (tasks[ind].Status == 1)
                        {
                            WIPList.Add(tasks[ind]);
                        }
                        else if (tasks[ind].Status == 2)
                        {
                            doneList.Add(tasks[ind]);
                        }
                    }
                }

                ToDo.ItemsSource = toDoList;
                InProgress.ItemsSource = WIPList;
                Done.ItemsSource = doneList;
            }
            else
            {
                Refresh();
            }
        }
    }
}
