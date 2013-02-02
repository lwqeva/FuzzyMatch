using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuzzyMatch
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            listView.Columns.Add("ID",120);
            listView.Columns.Add("Name",320);
            listView.Columns.Add("Grade",70);
            listView.View = View.Details;
            buttonAdd.Enabled = false;
            buttonSave.Enabled = false;
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {
            listView.Items.Clear();
            bool isFirst = true;

            // if this TextChanged event is activated by buttonAdd
            string name = textName.Text;
            if (name == "")
            {   // then set first student as the newed added one
                name = Program.student;
                Program.student = "";  // empty selected student
                isFirst = false;
            }
 
            foreach (string student in Program.students.match(name))
            {
                if (isFirst)
                {
                    Program.student = student;
                    isFirst = false;
                }
                ListViewItem item = new ListViewItem();
                item.Text = student.Substring(0, 9);
                item.SubItems.Add(student.Substring(9));
                item.SubItems.Add(Program.students.getGrade(student));
                listView.Items.Add(item);
            }
            if (textName.Text == "" || textGrade.Text == "")
                buttonAdd.Enabled = false;
            else
                buttonAdd.Enabled = true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            double pre;
            try
            {
                pre = Program.students.assignGrade(Program.student,
                double.Parse(textGrade.Text));
            }catch(Exception nondouble){
                MessageBox.Show("The grade should be a nonnegative numerical value!","Illegal Grade Value!");
                return;
            }
            
            if (pre >= 0)
            {
                if (MessageBox.Show("Rewrite previous grade?", "Repeated Input!", MessageBoxButtons.YesNo)
                        == DialogResult.No)
                {
                    Program.students.assignGrade(Program.student, pre);
                    return;
                }
            }
            textGrade.Text = "";
            textName.Text = "";
            this.TextChanged += new EventHandler(textName_TextChanged);
            textGrade.Focus();
            buttonSave.Enabled = true;
        }

        private void textGrade_TextChanged(object sender, EventArgs e)
        {
            if (textName.Text == "" || textGrade.Text == "")
                buttonAdd.Enabled = false;
            else
                buttonAdd.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Program.students.saveGrades("grades.txt");
            buttonSave.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (buttonSave.Enabled)
            {
                if (MessageBox.Show("Do you want to save current grades?", "Save grades", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Program.students.saveGrades("grades.txt");
                }
            }
        }

        bool isShortcut = false;
        private void match_Shortcuts(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Back)
            {
                textGrade.Text = "";
                textName.Text = "";
                this.TextChanged += new EventHandler(textName_TextChanged);
                textGrade.Focus();
                isShortcut = true;
            }
        }
        private void filter_Shortcuts(object sender, KeyPressEventArgs e)
        {
            if (isShortcut)
            {
                isShortcut = false;
                e.Handled = true;
            }
        }
    }
}
