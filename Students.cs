using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsLAB6 
{
    [Serializable]
    internal struct students
    {
        public string FIO;
        public int number;
        public int clas;
        public DateTime year;
        public string teacher;
        public float score;
        public override string ToString()
        {
            return $"{number,2} {FIO,40} {year.ToShortDateString(),22} {clas,18} {score,26} {teacher,57}";
        }

        public static string Head
        {
            get
            {
                return $"Номер {"ФИО",30} {"Дата Рождения",38} {"Класс",15} {"Средний балл за год",26} {"ФИО классного руководителя",40}";
            }
        }
    }


    internal class Students
    {
        private static string nameFileStudents = "Students.dat";

        public static List<students> ViewStudent(StreamWriter fileProtocol, int check)
        {
            List<students> students = new List<students>();
            try
            {
                using (FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    while (fileStudents.Position < fileStudents.Length)
                    {
                        students.Add((students)bf.Deserialize(fileStudents));
                    }
                }
                if (check == 1)
                    Protocol.WriteProtocol("Просмотр базы данных \t| ", fileProtocol);
                return students;
            }
            catch (Exception e)
            {
                if (check == 1)
                    Protocol.WriteProtocol("Не удалось вывести БД \t| ", fileProtocol);
                return students;
            }
        }
        public static void AddStudent(StreamWriter fileProtocol, students addStudent)
        {
            try
            {
                using (FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate))
                {
                    int count = 0;
                    students student;
                    BinaryFormatter bf = new BinaryFormatter();
                    while (fileStudents.Position < fileStudents.Length)
                    {
                        student = (students)bf.Deserialize(fileStudents);
                        count++;
                    }
                    addStudent.number = count + 1;
                    bf.Serialize(fileStudents, addStudent);
                }
                Protocol.WriteProtocol("Добавлен новый ученик в конец базы данных \t| ", fileProtocol);
            }
            catch (Exception ex)
            {
                Protocol.WriteProtocol("При добавлении записи произошла ошибка \t| ", fileProtocol);
            }
        }
        public static void Red(int numberStudent, StreamWriter fileProtocol, students newStudent, int[] checkArray)
        {
            if (numberStudent > Count() || numberStudent < 1)
            {
                Console.WriteLine("Такого номера не существует");
            }
            else
            {
                try
                {
                    List<students> students = new List<students>();
                    FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate);
                    BinaryFormatter bf = new BinaryFormatter();
                    while (fileStudents.Position < fileStudents.Length)
                    {
                        students.Add((students)bf.Deserialize(fileStudents));
                        
                        if (students[students.Count() - 1].number == numberStudent)
                        {
                            var str = students[students.Count() - 1]; 
                            if (checkArray[0] == 1) str.FIO = newStudent.FIO;
                            if (checkArray[1] == 1) str.year = newStudent.year;
                            if (checkArray[2] == 1) str.clas = newStudent.clas;
                            if (checkArray[3] == 1) str.score = newStudent.score;
                            if (checkArray[4] == 1) str.teacher = newStudent.teacher;
                            students[students.Count() - 1] = str;
                        }
                    }
                    fileStudents.Close();
                    FileStream file = new FileStream(nameFileStudents, FileMode.Create);
                    for (int i = 0; i < students.Count(); i++)
                    {
                        bf.Serialize(file, students[i]);
                    }
                    file.Close();
                    students.Clear();
                    Protocol.WriteProtocol($"Изменен элемент БД под номером {numberStudent} \t| ", fileProtocol);
                }
                catch (Exception e)
                {
                    Protocol.WriteProtocol($"Не удалось изменить элемент БД под номером {numberStudent} \t| ", fileProtocol);
                }
            }
        }
        public static void Delete(int numberStudent, StreamWriter fileProtocol)
        {
            try
            {
                List<students> students = new List<students>();
                FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                while (fileStudents.Position < fileStudents.Length)
                {
                    students.Add((students)bf.Deserialize(fileStudents));
                    if (students[students.Count() - 1].number == numberStudent) students.RemoveAt(students.Count() - 1);
                    if (students[students.Count() - 1].number > numberStudent)
                    {
                        var str = students[students.Count() - 1];
                        str.number = students.Count();
                        students[students.Count() - 1] = str;
                    }
                }
                fileStudents.Close();
                FileStream file = new FileStream(nameFileStudents, FileMode.Create);
                for (int i = 0; i < students.Count(); i++)
                {
                    bf.Serialize(file, students[i]);
                }
                file.Close();
                students.Clear();
                Protocol.WriteProtocol($"Удален ученик под номером {numberStudent} \t| ", fileProtocol);
            }
            catch (Exception e)
            {
                Protocol.WriteProtocol($"Не удалось удалить ученика под номером {numberStudent} \t| ", fileProtocol);
            }
        }
        public static void AddNum(int numberStudent, StreamWriter fileProtocol, students newStudent)
        {
            try
            {
                List<students> students = new List<students>();
                FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                while (fileStudents.Position < fileStudents.Length)
                {
                    students.Add((students)bf.Deserialize(fileStudents));
                    if (students[students.Count() - 1].number > numberStudent)
                    {
                        var str = students[students.Count() - 1];
                        str.number = students.Count();
                        students[students.Count() - 1] = str;
                    }
                    if (students[students.Count() - 1].number == numberStudent)
                    {
                        var str = students[students.Count() - 1];
                        str.number = students.Count() + 1;
                        students[students.Count() - 1] = str;
                        newStudent.number = numberStudent;
                        students.Insert(numberStudent - 1, newStudent);
                    }
                }
                fileStudents.Close();
                FileStream file = new FileStream(nameFileStudents, FileMode.Create);
                for (int i = 0; i < students.Count(); i++)
                {
                    bf.Serialize(file, students[i]);
                }
                file.Close();
                students.Clear();
                Protocol.WriteProtocol($"Добавлен новый ученик под номером {numberStudent} \t| ", fileProtocol);
            }
            catch (Exception e)
            {
                Protocol.WriteProtocol($"Не удалось добавить нового ученика под номером {numberStudent} \t| ", fileProtocol);
            }
        }
        public static List<students> SearchClass(int numberClass, StreamWriter fileProtocol)
        {
            List<students> studentsArray = new List<students>();
            try
            {
                FileStream fileStudents = new FileStream(nameFileStudents, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                students student;
                while (fileStudents.Position < fileStudents.Length)
                {
                    student = (students)bf.Deserialize(fileStudents);
                    if (numberClass == student.clas)
                    {
                        studentsArray.Add(student);
                    }
                }
                fileStudents.Close();
                Protocol.WriteProtocol($"Выполнен поиск учеников определённого({numberClass}) класса \t| ", fileProtocol);
                return studentsArray;
            }
            catch (Exception e)
            {
                Protocol.WriteProtocol($"Не удалось выполнить поиск учеников определённого({numberClass}) класса \t| ", fileProtocol);
                return studentsArray;
            }
        }
        public static List<students> Sort(int checkOptions, StreamWriter fileProtocol)
        {
            List<students> studentsArray = new List<students>();
            try
            {
                students student;
                FileStream fileStudent = new FileStream(nameFileStudents, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                while (fileStudent.Position < fileStudent.Length)
                {
                    studentsArray.Add((students)bf.Deserialize(fileStudent));
                }
                fileStudent.Close();
                switch (checkOptions)
                {
                    case 1:
                        for (int i = 0; i < studentsArray.Count(); i++)
                            for (int j = 0; j < studentsArray.Count() - 1; j++)
                                if (studentsArray[j].clas > studentsArray[j + 1].clas)
                                {
                                    student = studentsArray[j + 1];
                                    studentsArray[j + 1] = studentsArray[j];
                                    studentsArray[j] = student;
                                }
                        Protocol.WriteProtocol("Выполнена сортировка учеников по номеру класса \t| ", fileProtocol);
                        break;
                    case 2:
                        for (int i = 0; i < studentsArray.Count(); i++)
                            for (int j = 0; j < studentsArray.Count() - 1; j++)
                                if (studentsArray[j].score > studentsArray[j + 1].score)
                                {
                                    student = studentsArray[j + 1];
                                    studentsArray[j + 1] = studentsArray[j];
                                    studentsArray[j] = student;
                                }
                        Protocol.WriteProtocol("Выполнена сортировка учеников по среднему баллу за год \t| ", fileProtocol);
                        break;
                    case 3:
                        for (int i = 0; i < studentsArray.Count(); i++)
                            for (int j = 0; j < studentsArray.Count() - 1; j++)
                                if (studentsArray[j].year > studentsArray[j + 1].year)
                                {
                                    student = studentsArray[j + 1];
                                    studentsArray[j + 1] = studentsArray[j];
                                    studentsArray[j] = student;
                                }
                        Protocol.WriteProtocol("Выполнена сортировка учеников по дате рождения \t| ", fileProtocol);
                        break;
                }
                return studentsArray;
            }
            catch (Exception e)
            {
                switch (checkOptions)
                {
                    case 1:
                        Protocol.WriteProtocol("Не удалось выполнить сортировку учеников по номеру класса \t| ", fileProtocol);
                        break;
                    case 2:
                        Protocol.WriteProtocol("Не удалось выполненить сортировку учеников по среднему баллу за год \t| ", fileProtocol);
                        break;
                    case 3:
                        Protocol.WriteProtocol("Не удалось выполнить сортировку учеников по дате рождения \t| ", fileProtocol);
                        break;
                }
                return studentsArray;
            }
        }
        public static float Sum(StreamWriter fileProtocol)
        {
            float sum = 0;
            try
            {
                List<students> studentsArray = new List<students>();
                FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                while (fileStudents.Position < fileStudents.Length)
                {
                    studentsArray.Add((students)bf.Deserialize(fileStudents));
                }
                fileStudents.Close();
                for (int i = 0; i < studentsArray.Count(); i++)
                {
                    sum += studentsArray[i].score;
                }
                sum = sum / Count();
                Protocol.WriteProtocol("Выполнен поиск среднего арифметического баллов среди всех учеников \t| ", fileProtocol);
                return sum;
            }
            catch
            {
                Protocol.WriteProtocol("Не удалось выполнить поиск среднего арифметического баллов среди всех учеников \t| ", fileProtocol);
                return sum;
            }
        }
        public static int Count()
        {
            int count = 0;
            try
            {
                using (FileStream fileStudents = new FileStream(nameFileStudents, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    students student;
                    while (fileStudents.Position < fileStudents.Length)
                    {
                        student = (students)bf.Deserialize(fileStudents);
                        count++;
                    }
                }
            }
            catch (Exception e) { }
            return count;
        }
    }
}
