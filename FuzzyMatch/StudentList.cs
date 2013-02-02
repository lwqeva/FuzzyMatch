using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FuzzyStrings;
using System.Windows.Forms;

namespace FuzzyMatch
{
    // StudentList is in charge of managing student list
    // and provide sorted matching results w.r.t. the input.
    class StudentList
    {
        public StudentList(string path)
        {
            try
            {
                list = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                MessageBox.Show("Please put file: \"" + path + "\" under the working directory and restart the program.");
                Environment.Exit(0);
            }

            // replace '\t' from spreadsheet by space
            for (int i = 0; i < list.Length; ++i)
                list[i] = list[i].Replace('\t', ' ');

            grades = new Dictionary<string, double>();
            if (File.Exists("grades.txt"))
            {
                string[] lines = File.ReadAllLines("grades.txt");
                if (list.Length == lines.Length)
                {
                    for (int i = 0; i < list.Length; ++i)
                    {
                        if( lines[i] != "" )
                            grades.Add(list[i], double.Parse(lines[i]));
                    }
                }
            }
        }

        public void saveGrades(string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string student in list)
            {
                if( grades.ContainsKey(student) )
                    sb.AppendLine(grades[student].ToString());
                else
                    sb.AppendLine("");
            }
            File.WriteAllText(path, sb.ToString());
        }

        public string[] match(string name)
        {
            name = name.ToLower();

            // sort student list according to similarity
            SortedList<double, string> result = new SortedList<double,string>();
            foreach (string item in list)
            {
                double s = name.DiceCoefficient(item.ToLower());

                while (result.ContainsKey(s))
                    s += 1e-10;
                result.Add(s, item);
            }

            // fetch student list ordered by similarity
            List<string> output = new List<string>();
            foreach (KeyValuePair<double, string> pair in result.Reverse())
            {
                output.Add(pair.Value);
            }
            return output.ToArray<string>();
        }

        public double assignGrade(string name, double grade)
        {
            if (grades.ContainsKey(name))
            {
                double pre = grades[name];
                grades[name] = grade;
                return pre;
            }
            else
                grades.Add(name, grade);
            return -1;
        }
        public string getGrade(string name)
        {
            if (grades.ContainsKey(name))
                return grades[name].ToString();
            return "";
        }

        string[] list;
        Dictionary<string, double> grades;
    }
}
