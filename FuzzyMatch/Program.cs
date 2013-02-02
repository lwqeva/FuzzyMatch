using System;
using System.Collections.Generic;
using System.Text;
using FuzzyStrings;

namespace FuzzyMatch
{
    class Program
    {
        public static StudentList students;
        public static string student;
        static void Main(string[] args)
        {
            students = new StudentList("StudentNames.txt");

            MainForm form = new MainForm();
            form.ShowDialog();
        }
    }
}
