using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsLAB6
{
    public partial class Form : System.Windows.Forms.Form
    {
        const string MESSAGE1 = "1. Просмотр БД";
        const string MESSAGE2 = "2. Добавить запись в конец БД";
        const string MESSAGE3 = "3. Добавить запись в БД по номеру";
        const string MESSAGE4 = "4. Редактировать запись";
        const string MESSAGE5 = "5. Удаление записи по номеру";
        const string MESSAGE6 = "6. Подсчёт количества записей в БД";
        const string MESSAGE7 = "7. Поиск учеников из одного класса";
        const string MESSAGE8 = "8. Сортировка по номеру класса";
        const string MESSAGE9 = "9. Сортировка по среднему баллу за год";
        const string MESSAGE10 = "10. Сортировка по дате рождения";
        const string MESSAGE11 = "11. Нахождение среднего балла среди всех учеников";
        string fileName; 
        int checkOption = 0;
        public Form()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            textBox1.Visible = true;
            label7.Text = "Введите название файла:";
            button2.Visible = false; 
            button3.Visible = false;
            label8.Visible = false;
            checkOption = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            textBox1.Visible = true;
            label7.Text = "Введите путь к файлу:";
            button2.Visible = false; 
            button3.Visible = false;
            label8.Visible = false;
            checkOption = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                fileName = textBox1.Text;
                if (!fileName.Contains(".txt")) fileName += ".txt";
                if (checkOption == 1)
                {
                    Protocol.Create(fileName);
                }
                if (checkOption == 2)
                {
                     if (!File.Exists(fileName))
                     {
                         Result.Text = "";
                         Result.Text = "Данный файл не существует. Повторите попытку";
                         textBox1.Text = "";
                         return;
                     }
                }
                Protocol.Open(fileName);
                groupBox1.Visible = true; Menu.Visible = true; C.Visible = true;
                Start.Visible = true; label7.Visible = false; textBox1.Visible = false;
                label6.Visible = true; Dop.Visible = true; DopText.Visible = true; button1.Visible = false;
                Result.Text = "";
            }
            catch
            {
                Result.Text = "Error";
            }
        }

        private void Menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            Result.Text = ""; FIO.Text = ""; Year.Text = ""; Clas.Text = "";
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            DopText.Text = ""; Score.Text = ""; Teacher.Text = ""; DopText.Text = ""; Dop.Text = "";
            if (Menu.Text == MESSAGE1 || Menu.Text == MESSAGE2 || Menu.Text == MESSAGE6 
                || Menu.Text == MESSAGE8 || Menu.Text == MESSAGE9 || Menu.Text == MESSAGE10 
                || Menu.Text == MESSAGE11)
            {
                groupBox2.Visible = false;
                if (Menu.Text == MESSAGE1 || Menu.Text == MESSAGE6 || Menu.Text == MESSAGE8 
                    || Menu.Text == MESSAGE9 || Menu.Text == MESSAGE10 || Menu.Text == MESSAGE11)
                {
                    groupBox1.Visible = false;
                }
            }
            if (Menu.Text == MESSAGE5 || Menu.Text == MESSAGE7)
            {
                groupBox1.Visible = false;
                if (Menu.Text == MESSAGE5 || Menu.Text == MESSAGE4)
                {
                    DopText.Text = "Номер ученика";
                }
                if (Menu.Text == MESSAGE7)
                {
                    DopText.Text = "Номер класса";
                }
            }
            if (Menu.Text == MESSAGE3)
            {
                DopText.Text = "Номер ученика";
            }
            if (Menu.Text == MESSAGE4)
            {
                DopText.Text = "Номер ученика";
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            int check = 0;
            StreamWriter fileProtocol = new StreamWriter(fileName, true);
            try
            {
                if (Menu.Text == MESSAGE2 || Menu.Text == MESSAGE3)
                {
                    students newStudent = new students();
                    newStudent.FIO = FIO.Text;
                    newStudent.year = Convert.ToDateTime(Year.Text);
                    newStudent.clas = int.Parse(Clas.Text);
                    newStudent.score = float.Parse(Score.Text);
                    newStudent.teacher = Teacher.Text;
                    if (Menu.Text == MESSAGE2)
                        Students.AddStudent(fileProtocol, newStudent);
                    else
                    {
                        int n = int.Parse(Dop.Text);
                        Students.AddNum(n, fileProtocol, newStudent);
                    }
                    check = 1;
                }
                if (Menu.Text == MESSAGE4)
                {
                    students student = new students();
                    int[] checkArray = new int[5] { 0, 0, 0, 0, 0 };
                    int numberStudent = int.Parse(Dop.Text);
                    if (FIO.Text != "") { student.FIO = FIO.Text; checkArray[0] = 1; }
                    if (Year.Text != "") { student.year = Convert.ToDateTime(Year.Text); checkArray[1] = 1; }
                    if (Clas.Text != "") { student.clas = int.Parse(Clas.Text); checkArray[2] = 1; }
                    if (Score.Text != "") { student.score = float.Parse(Score.Text); checkArray[3] = 1; }
                    if (Teacher.Text != "") { student.teacher = Teacher.Text; checkArray[4] = 1; }
                    Students.Red(numberStudent, fileProtocol, student, checkArray);
                    check = 1;
                }
                if (Menu.Text == MESSAGE5)
                {
                    int numberStudent = int.Parse(Dop.Text);
                    Students.Delete(numberStudent, fileProtocol);
                    check = 1;
                }
                if (Menu.Text == MESSAGE6)
                {
                    int count = Students.Count();
                    Result.Text = $"Количество учеников в БД: {count}";
                    fileProtocol.WriteLine($"Просмотр количества учеников в БД ({count}) \t| " + DateTime.Now);
                }
                if (Menu.Text == MESSAGE7)
                {
                    int numberClass = int.Parse(Dop.Text);
                    List<students> studentsArray = new List<students>();
                    studentsArray = Students.SearchClass(numberClass, fileProtocol);
                    if (studentsArray.Count() > 0)
                    {
                        Result.Text += $"{WindowsFormsLAB6.students.Head}" + Environment.NewLine;
                        foreach (var person in studentsArray)
                        {
                            Result.Text += person.ToString() + Environment.NewLine;
                        }
                    }
                    else Result.Text = "Ученики из этого класса в БД отсутствуют";
                    studentsArray.Clear();
                }
                if (Menu.Text == MESSAGE8 || Menu.Text == MESSAGE9 || Menu.Text == MESSAGE10)
                {
                    List<students> studentsArray = new List<students>();
                    if (Menu.Text == MESSAGE8)
                    {
                        studentsArray = Students.Sort(1, fileProtocol);
                    }
                    if (Menu.Text == MESSAGE9)
                    {
                        studentsArray = Students.Sort(2, fileProtocol);
                    }
                    if (Menu.Text == MESSAGE10)
                    {
                        studentsArray = Students.Sort(3, fileProtocol);
                    }
                    Result.Text += $"{WindowsFormsLAB6.students.Head}" + Environment.NewLine;
                    foreach (var person in studentsArray)
                    {
                        Result.Text += person.ToString() + Environment.NewLine;
                    }
                    studentsArray.Clear();
                }
                if (Menu.Text == MESSAGE11)
                {
                    float sum;
                    sum = Students.Sum(fileProtocol);
                    Result.Text = $"Средний балл среди всех учеников: {sum}";
                }
                if (Menu.Text == MESSAGE1 || check == 1)
                {
                    Result.Text += $"{students.Head}" + Environment.NewLine;
                    List<students> studentsArray = Students.ViewStudent(fileProtocol, 1);
                    for (int i = 0; i < studentsArray.Count(); i++)
                    {
                        Result.Text += studentsArray[i].ToString() + Environment.NewLine;
                    }
                    studentsArray.Clear();
                    check = 0;
                }
                fileProtocol.Close();
            }
            catch
            {
                fileProtocol.Close();
                Result.Text = "Error";
            }
        }

        private void C_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            FIO.Text = "";
            Year.Text = "";
            Clas.Text = "";
            Score.Text = "";
            Teacher.Text = "";
            DopText.Text = "";
            groupBox2.Visible = true;
            Result.Text = "";
            Menu.Text = "Меню:";
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            try
            {
                Protocol.Close(fileName);
            }
            catch
            {
                this.Close();
            }
        }
    }
}