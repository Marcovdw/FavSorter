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
        private int b1l, b1u, b2l, b2u;

        // Counters for traversing sublists
        private int c1, c2;

        // Temporary array to sort result of submerge
        private string[] temp;

        // Array holding the bounds of sublists to be merged
        private Tuple<int, int>[] rounds;

        // Current round
        private int round;

        // File to write result back towards
        private string fileName;

        public Form()
        {
            InitializeComponent();
        }

        private void Next_round()
        {
            // Write result of submerge back to input
            for (int i = 0; i < temp.Length; i++)
                elements[b1l + i] = temp[i];

            // Terminate if all rounds are performed
            if (++round == rounds.Length)
            {
                // Write and show results
                System.IO.File.WriteAllLines(fileName, elements);
                Process.Start("notepad.exe", fileName);

                // Reset buttons and lables
                button1.Enabled = false;
                button1.Text = "";
                button2.Enabled = false;
                button2.Text = "";
                button3.Enabled = true;
                button3.Text = "Load file";
                label1.Text = "Done";
            }
            else
            {
                // Reset counters
                c1 = 0;
                c2 = 0;

                // Reset bounds
                Tuple<int, int> t = rounds[round];
                int h = (t.Item2 - t.Item1) / 2;
                b1l = t.Item1;
                b1u = t.Item1 + h;
                b2l = t.Item1 + h + 1;
                b2u = t.Item2;

                // Clear temp
                temp = new string[t.Item2 - t.Item1 + 1];

                // Set first matchup
                button1.Text = elements[b1l];
                button2.Text = elements[b2l];
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
                // Read input and store to elements
                fileName = openFileDialog.FileName;
                elements = System.IO.File.ReadAllLines(fileName);

                // Set buttons, labels and variables
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = false;
                label1.Text = "Versus";
                temp = new string[0];
                round = -1;

                // Set rounds array
                rounds = new Tuple<int, int>[elements.Length - 1];
                Set_rounds(0, elements.Length - 1, 0, elements.Length - 2);

                // Begin
                Next_round();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            match(ref button1, ref c1, ref c2);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            match(ref button2, ref c2, ref c1);
        }

        private void match(ref Button button, ref int cw, ref int cl)
        {
            // Set bounds
            int bwl, bwu, bll, blu;
            if(button == button1)
            {
                bwl = b1l;
                bwu = b1u;
                bll = b2l;
                blu = b2u;
            }
            else
            {
                bwl = b2l;
                bwu = b2u;
                bll = b1l;
                blu = b1u;
            }

            // Write winning element to temp array
            temp[cw + cl] = elements[bwl + cw++];
            if (bwl + cw > bwu)
            {
                // A sublist is exhausted, add remaining elements in order
                for (int i = bll + cl; i <= blu; i++)
                    temp[cw + cl++] = elements[i];
                // Go to next round
                Next_round();
            }
            else
                // Next matchup
                button.Text = elements[bwl + cw];
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
