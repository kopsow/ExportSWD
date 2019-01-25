using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ExportSWD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<string, string> bazy = new Dictionary<string, string>();
        

        private string file;

        private string path;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileNames[0];
                path = Path.GetDirectoryName(openFileDialog1.FileName);
                using (StreamReader sr = new StreamReader(openFileDialog1.FileNames[0]))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = line;
                    file = line;
                }
                dodajKomunikat("Wczytano plik konfiguracyjny", Color.Green);
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.ValidateChildren())
            {
               
                foreach (KeyValuePair<string, string> val in bazy)
                {
                    replace(val.Key, val.Value);
                }
            } else
            {
                
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bazy.Add("DZIAŁOSZYN-miasto_ZINTEGROWANA", "1009014");
            bazy.Add("DZIAŁOSZYN-obszar wiejski", "1009015");
            bazy.Add("KIEŁCZYGŁÓW", "1009022");
            bazy.Add("KIEŁCZYGŁÓW_ZINTEGROWANA", "1009022");
            bazy.Add("NOWA BRZEŹNICA", "1009032");
            bazy.Add("PAJĘCZNO-miasto", "1009044");
            bazy.Add("PAJĘCZNO-obszar wiejski", "1009045");
            bazy.Add("RZĄŚNIA", "1009052");
            bazy.Add("SIEMKOWICE","1009062");
            bazy.Add("SIEMKOWICE_ZINTEGROWANA", "1009062");
            bazy.Add("STRZELCE WIELKIE", "1009072");
            bazy.Add("SULMIERZYCE", "1009082");
            ColumnHeader header = new ColumnHeader();
            header.Text = "";
            header.Name = "col1";
            header.Width = 500;
            listView1.Columns.Add(header);
           if (Properties.Resources.EWOPIS.Length >0)
            {
                textBox2.Text = Properties.Resources.EWOPIS;
            }
           
        }

        private void replace(string nazwa_pliku,string nazwa_bazy)
        {
            string plik_swd = nazwa_pliku;

            string output_1 = Regex.Replace(file, @"^EFile=D:\\programy\\ewopis6\\SWDE\\[0-9]{7}\.swd", "EFile="+path+@"\"+nazwa_bazy+"_"+plik_swd+".swd", RegexOptions.Multiline);
            string output_2 = Regex.Replace(output_1, @"^GminaId\=.*$", @"GminaId="+nazwa_pliku , RegexOptions.Multiline);
            string output_3 = Regex.Replace(output_2, @"Baza=srv-gndb.powiatpajeczno.local:D:\\bazy\\ewopis\\POWIAT1009\\[0-9]{7}\.fdb", @"Baza=srv-gndb.powiatpajeczno.local:D:\bazy\ewopis\POWIAT1009\"+nazwa_bazy+".fdb", RegexOptions.IgnoreCase);
            
            ListViewItem item = new ListViewItem();
            item.Text = string.Format("Tworzę plik wynikowy dla bazy{0} - gmina {1}", nazwa_bazy, nazwa_pliku);
            listView1.Items.Add(item);
            if (!Directory.Exists(path + @"\wsady"))
            {
                Directory.CreateDirectory(path + @"\wsady");
            }
            using (StreamWriter writer = new StreamWriter(path+@"\wsady\"+plik_swd+".ini", false, Encoding.Unicode))
            {
                writer.WriteLine(output_3);
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text.Length==0)
            {
                errorProvider1.SetError(textBox1, "Wskaż plik konfiguracyjny");
                e.Cancel = true;
                dodajKomunikat("Brak wskazanego pliku konfiguracyjnego", Color.Red);

            } else
            {
                errorProvider1.SetError(textBox1, "");
                e.Cancel = false;
            }
        }

        private void dodajKomunikat(string komunikat,Color kolor)
        {
            
                ListViewItem item = new ListViewItem();
                item.Text = komunikat;
                item.ForeColor = kolor;
                listView1.Items.Add(item);
            
        }
        private void dodajKomunikat(string komunikat)
        {
            ListViewItem item = new ListViewItem();
            item.Text = komunikat;
            item.ForeColor = Color.Black;
            listView1.Items.Add(item);
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text.Length ==0)
            {
                errorProvider1.SetError(textBox2, "Brak ścieżki do EWOPIS-u");
                e.Cancel = true;
                dodajKomunikat("Brak ścieżki do EWOPIS-u",Color.Red);
            } else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox2, "");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           string result = Path.GetExtension(path);
        }
    }
}
