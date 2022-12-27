using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace CourwWorkAutomataTheory
{
    public partial class MainForm : Form
    {
        readonly SyntaxAnalyzer syntax = new SyntaxAnalyzer();

        readonly LexemeAnalyzer lexeme = new LexemeAnalyzer();
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
            try { 
            
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

                list2 = lexeme.GetTccTable();

                foreach (var item in list2)
                {
                    dataGridView2.Rows.Add(item.Item1, item.Item2);
                }

                for (int i = 0; i < lexeme.keyWords.Count; i++)
                {
                    dataGridView3.Rows[i].Cells[0].Value = lexeme.keyWords[i];
                }

                for (int i = 0; i < lexeme.literals.Count; i++)
                {
                    dataGridView3.Rows[i].Cells[1].Value = lexeme.literals[i];
                }

                for (int i = 0; i < lexeme.indentificators.Count; i++)
                {

                    dataGridView3.Rows[i].Cells[2].Value = lexeme.indentificators[i];
                }

                for (int i = 0; i < lexeme.limiters.Count; i++)
                {
                    if (lexeme.limiters[i] != "\n")
                    {
                        dataGridView3.Rows[i].Cells[3].Value = lexeme.limiters[i];
                    }

                    else
                    {
                        dataGridView3.Rows[i].Cells[3].Value = "\\n";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
            }

            try
            {
                syntax.CheckSyntax(list2, lexeme);
                MessageBox.Show("Разбор прошёл успешно", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(syntax.log);
                syntax.Reset();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                syntax.Reset();
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
