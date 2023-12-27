using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Tyuiu.GofmanDV.Sprint7.Project.V2
{
    public partial class FormMainMenu_GDV : Form
    {
        public BindingList<Info> infoList;
        private BindingSource bindingSource;
        private SortOrder currentSortOrder = SortOrder.Ascending;
        public FormMainMenu_GDV()
        {
            InitializeComponent();

            // Инициализируем список и источник данных
            infoList = new BindingList<Info>();
            bindingSource = new BindingSource(infoList, null);

            // Привязываем источник данных к DataGridView
            dataGridViewInfo_GDV.DataSource = bindingSource;

            dataGridViewInfo_GDV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void datagrid()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.Title = "Выберите CSV файл для открытия";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Читаем все строки из файла
                    string[] lines = File.ReadAllLines(filePath);

                    // Парсим заголовки столбцов
                    string[] headers = lines[0].Split(',');

                    // Очищаем старые данные в BindingList
                    infoList.Clear();

                    // Заполняем BindingList данными из файла
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] values = lines[i].Split(',');
                        infoList.Add(new Info
                        {
                            Name = values[0],
                            Adress = values[1],
                            Phone = values[2],
                            Cassa = Convert.ToInt32(values[3]),
                            Staff = Convert.ToInt32(values[4])
                        });
                    }

                    MessageBox.Show("Файл успешно открыт!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при открытии файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            datagrid();
        }

        private void buttonMagnit_GDV_Click(object sender, EventArgs e)
        {
            datagrid();
        }

        private void buttonPyaterochka_GDV_Click(object sender, EventArgs e)
        {
            datagrid();
        }
        public class Info
        {
            public string Name { get; set; }
            public string Adress { get; set; }
            public string Phone { get; set; }
            public int Cassa { get; set; }
            public int Staff { get; set; }

            // Конструктор по умолчанию
            public Info()
            {

            }

            public Info(string name, string adress, string phone, int cassa, int staff)
            {
                Name = name;
                Adress = adress;
                Phone = phone;
                Cassa = cassa;
                Staff = staff;
            }
        }
        private void SortDataGridViewByColumn(string columnName)
        {
            if (currentSortOrder == SortOrder.Ascending)
            {
                bindingSource.DataSource = new BindingList<Info>(infoList.OrderBy(x => typeof(Info).GetProperty(columnName).GetValue(x)).ToList());
                currentSortOrder = SortOrder.Descending;
            }
            else
            {
                bindingSource.DataSource = new BindingList<Info>(infoList.OrderByDescending(x => typeof(Info).GetProperty(columnName).GetValue(x)).ToList());
                currentSortOrder = SortOrder.Ascending;
            }
        }

        private void buttonSort_GDV_Click(object sender, EventArgs e)
        {
            try
            {
                string sortName = textBoxSortInput_GDV.Text;
                SortDataGridViewByColumn(sortName);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при вводе данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSearch_GDV_Click(object sender, EventArgs e)
        {
            SearchInDataGridView(textBoxSearch_GDV.Text);
        }
        private void SearchInDataGridView(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Если поле поиска пусто, отобразить все данные
                dataGridViewInfo_GDV.DataSource = bindingSource;
            }
            else
            {
                // Иначе выполните поиск и отобразить результаты
                var searchResults = infoList.Where(info =>
                    info.Name.ToString().Contains(searchText) ||
                    info.Adress.Contains(searchText) ||
                    info.Phone.Contains(searchText) ||
                    info.Cassa.ToString().Contains(searchText) ||
                    info.Staff.ToString().Contains(searchText)
                ).ToList();

                dataGridViewInfo_GDV.DataSource = new BindingList<Info>(searchResults);
            }
        }

        private void textBoxSearch_GDV_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Проект выполнил студент группы СМАРТб-23-1 Гофман Даниил Викторович", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void buttonSaveFile_GDV_Click(object sender, EventArgs e)
        {
            try
            {
                string directoryPath = $@"{Directory.GetCurrentDirectory()}";
                string fileName = "InfoAboutStore.csv";
                string path = Path.Combine(directoryPath, fileName);


                // Создаем строку для записи заголовков столбцов
                string header = string.Join(",", dataGridViewInfo_GDV.Columns.Cast<DataGridViewColumn>().Select(column => column.HeaderText));

                // Создаем строки для каждой записи данных
                List<string> lines = new List<string>();
                foreach (DataGridViewRow row in dataGridViewInfo_GDV.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string line = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value.ToString()));
                        lines.Add(line);
                    }
                }

                // Соединяем заголовок и строки вместе
                string content = header + Environment.NewLine + string.Join(Environment.NewLine, lines);

                // Записываем содержимое в файл
                File.WriteAllText(path, content);

                DialogResult dialogResult = MessageBox.Show("Файл " + path + " сохранен успешно!\nОткрыть его в блокноте?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    System.Diagnostics.Process txt = new System.Diagnostics.Process();
                    txt.StartInfo.FileName = "notepad.exe";
                    txt.StartInfo.Arguments = path;
                    txt.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonHelpProgram_GDV_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В данном приложении вы можете выбрать 1 из магазинов, чтобы узнать об его названии, адрессе, номере владельца филиала, количестве касс и количестве сотрудников данного филиала. При желании можно произвести сортировку по возрастанию/убыванию определенного столбца, произвести поиск, или же сохранить файл.", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

