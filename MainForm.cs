using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CourwWorkAutomataTheory
{
    public partial class MainForm : Form
    {
        readonly Lexeme lexeme = new Lexeme();
        List<Tuple<string, string>> list = new List<Tuple<string, string>>();
        List<Tuple<string, int>> list2 = new List<Tuple<string, int>>();
        public MainForm()
        {
            InitializeComponent();

            richTextBox1.Text = File.ReadAllText(@"C:\Users\PC\Documents\Курсовая Автоматы\CourseWork\CourseWorkAutomataTheory\bin\Debug\code.txt"); // <<<<<<<<<<<<<<<<<<<<<
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string allText = File.ReadAllText(openFileDialog.FileName);
                richTextBox1.Text = allText;
            }
        }

        private void btn_do_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            try
            {
                list = lexeme.GetLexeme(richTextBox1.Text);

                foreach (var item in list)
                {
                    if (item.Item1 != "\n")
                    {
                        dataGridView1.Rows.Add(item.Item1, item.Item2);
                    }

                    else
                    {
                        dataGridView1.Rows.Add("\\n", item.Item2);
                    }
                }

                int counter = GetMax();

                for (int i = 0; i < counter; i++)
                {
                    dataGridView3.Rows.Add();
                }
            }
            catch (Exception exeption)
            {
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                MessageBox.Show(exeption.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int GetMax()
        {
            int max = int.MinValue;
            int[] values = new int[] { lexeme.limiters.Count, lexeme.indentificators.Count, lexeme.literals.Count, lexeme.keyWords.Count };
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > max)
                    max = values[i];
            }
            return max;
        }
    }
}
