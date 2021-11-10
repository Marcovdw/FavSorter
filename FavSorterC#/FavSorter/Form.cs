using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form : System.Windows.Forms.Form
    {
        // Elements to be sorted
        private string[] elements;

        // Bounds of sublists in input
        private int bll, blu, brl, bru;

        // Counters for traversing sublists
        private int cl, cr;

        // Temporary array to sort result of submerge
        private string[] temp = new string[0];

        // Array holding the bounds of sublists to be merged
        private Tuple<int, int>[] rounds;

        // Current round
        private int round = -1;

        // File to write result back towards
        private string fileName;

        public Form()
        {
            InitializeComponent();
        }

        private void Next_round()
        {
            // Terminate if all rounds are performed
            if (++round == rounds.Length)
            {
                System.IO.File.WriteAllLines(fileName, elements);
                button1.Enabled = false;
                button2.Enabled = false;
                label1.Text = "Done";
                Process.Start("notepad.exe", fileName);
            }
            else
            {
                // Write result of submerge back to input
                for (int i = 0; i < temp.Length; i++)
                    elements[bll + i] = temp[i];
                // Reset counters
                cl = 0;
                cr = 0;
                // Reset bounds
                Tuple<int, int> t = rounds[round];
                int h = (t.Item2 - t.Item1) / 2;
                bll = t.Item1;
                blu = t.Item1 + h;
                brl = t.Item1 + h + 1;
                bru = t.Item2;
                // Clear temp
                temp = new string[t.Item2 - t.Item1 + 1];
                // Set first matchup
                button1.Text = elements[bll];
                button2.Text = elements[brl];
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = "C:\\",
                DefaultExt = ".txt",
                Filter = "txt files (*.txt)|*.txt"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                button3.Enabled = false;
                button1.Enabled = true;
                button2.Enabled = true;
                label1.Text = "Versus";
                // Read input and store to elements
                elements = System.IO.File.ReadAllLines(fileName);
                // Set rounds array
                rounds = new Tuple<int, int>[elements.Length - 1];
                Set_rounds(0, elements.Length - 1, 0, elements.Length - 2);
                // Begin
                Next_round();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Write left element to temp array
            temp[cl + cr] = elements[bll + cl++];
            if (bll + cl > blu)
            {
                // Left sublist is exhausted, add remaining right elements in order
                for (int i = brl + cr; i <= bru; i++)
                    temp[cl + cr++] = elements[i];
                // Go to next round
                Next_round();
            }
            else
                // Next matchup
                button1.Text = elements[bll + cl];
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // Write right element to temp array
            temp[cl + cr] = elements[brl + cr++];
            if (brl + cr > bru)
            {
                // Right sublist is exhausted, add remaining right elements in order
                for (int i = bll + cl; i <= blu; i++)
                    temp[cl++ + cr] = elements[i];
                // Go to next round
                Next_round();
            }
            else
                // Next matchup
                button2.Text = elements[brl + cr];
        }

        private void Set_rounds(int el, int eu, int rl, int ru)
        {
            // No need to match against itself
            if (el < eu)
            {
                // Add round to back of array
                rounds[ru] = new Tuple<int, int>(el, eu);
                // Create a round for the left and right half of the input
                int h = (eu - el) / 2;
                int he = el + h;
                int hr = rl + h;
                Set_rounds(el, he, rl, hr - 1);
                Set_rounds(he + 1, eu, hr, ru - 1);
            }
        }
    }
}
