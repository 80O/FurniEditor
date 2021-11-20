using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FurniEditor
{
    public class Form1 : Form
    {
        private IContainer components = (IContainer)null;
        public string CurrentFurni;
        public string CurrentShortName;
        public string OpenedFile;
        public string OriginalFile;
        private Button button1;
        private OpenFileDialog openFileDialog1;
        private Label label1;
        private ListBox listBox1;
        private RichTextBox richTextBox1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Label label2;

        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Directory.Exists("furni"))
                return;
            Directory.CreateDirectory("furni");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num = (int)this.openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.CurrentFurni = this.openFileDialog1.SafeFileName;
            this.CurrentShortName = this.CurrentFurni.Replace(".swf", "");
            this.OriginalFile = this.openFileDialog1.FileName;
            this.label1.Text = this.CurrentShortName;
            if (!Directory.Exists("furni\\" + this.CurrentShortName))
            {
                Directory.CreateDirectory("furni\\" + this.CurrentShortName);
                File.Copy(this.OriginalFile, "furni\\" + this.CurrentShortName + "\\" + this.CurrentFurni);
                this.decompile();
            }
            else
            {
                int num = (int)MessageBox.Show("A project folder for that furni already exists. The existing project will be opened.");
            }
            string[] files = Directory.GetFiles("furni\\" + this.CurrentShortName, "*.bin");
            this.listBox1.Items.Clear();
            this.richTextBox1.Clear();
            foreach (string str in files)
                this.listBox1.Items.Add((object)str.Replace("furni\\" + this.CurrentShortName + "\\", ""));
            this.button3.Enabled = true;
            this.button4.Enabled = true;
            this.button5.Enabled = true;
            this.button2.Enabled = false;
        }

        private void decompile()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "edit.bat",
                Arguments = "d " + this.CurrentShortName,
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
        }

        private void compile(string name, string id)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "edit.bat",
                Arguments = "c " + name + " " + id,
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OpenedFile = this.listBox1.SelectedItem.ToString();
            this.richTextBox1.LoadFile("furni\\" + this.CurrentShortName + "\\" + this.listBox1.SelectedItem, RichTextBoxStreamType.PlainText);
            this.button2.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e) //Crack
        {
            foreach (string CurrentFile in this.listBox1.Items)
            {
                richTextBox1.LoadFile("furni\\" + this.CurrentShortName + "\\" + CurrentFile, RichTextBoxStreamType.PlainText);

                if (richTextBox1.Text.Contains("<graphics>"))
                    continue;
                if (!richTextBox1.Text.Contains("<visualizationData"))
                    continue;
                //<visualizationData type="tcar"><graphics>
                //</graphics></visualizationData>

                string replace1 = "<visualizationData type=\"" + CurrentShortName + "\">";
                string replace2 = "<visualizationData type=\"" + CurrentShortName + "\"><graphics>";
                richTextBox1.Text = richTextBox1.Text.Replace(replace1, replace2);
                
                replace1 = "</visualizationData>";
                replace2 = "</graphics></visualizationData>";
                richTextBox1.Text = richTextBox1.Text.Replace(replace1, replace2);

                richTextBox1.SaveFile("furni\\" + this.CurrentShortName + "\\" + CurrentFile, RichTextBoxStreamType.PlainText);
            }
            this.compile(this.CurrentShortName, new Regex("-(.*).bin").Match(this.OpenedFile).Groups[1].ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SaveFile("furni\\" + this.CurrentShortName + "\\" + this.OpenedFile, RichTextBoxStreamType.PlainText);
            this.compile(this.CurrentShortName, new Regex("-(.*).bin").Match(this.OpenedFile).Groups[1].ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.Copy("furni\\" + this.CurrentShortName + "\\" + this.CurrentFurni, this.OriginalFile, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = "furni\\" + this.CurrentShortName;
            string environmentVariable = Environment.GetEnvironmentVariable("WINDIR");
            new Process()
            {
                StartInfo =
                {
                    FileName = (environmentVariable + "\\explorer.exe"),
                    Arguments = str
                }
            }.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load Furni";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Select your furni";
            this.openFileDialog1.Filter = "Furni|*.swf";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "No Furni Loaded";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 33);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(167, 186);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(188, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(385, 186);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(407, 226);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 45);
            this.button2.TabIndex = 4;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(293, 226);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 45);
            this.button3.TabIndex = 5;
            this.button3.Text = "Open Project folder";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(188, 226);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(99, 45);
            this.button4.TabIndex = 6;
            this.button4.Text = "Replace Original";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(504, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "By : Leenster";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(497, 226);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 45);
            this.button5.TabIndex = 8;
            this.button5.Text = "Crack n\' Save";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(585, 281);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Furni Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
