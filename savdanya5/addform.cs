using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace savdanya5
{
    public partial class addform : Form
    {
        public addform()
        {
            InitializeComponent();
        }
        public Product NewProduct { get; private set; }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string article = textBox2.Text;
            int quantity = int.Parse(textBox3.Text);
            string manufacturer = textBox4.Text;
            DateTime arrivalDate;
            if (!DateTime.TryParse(textBox5.Text, out arrivalDate))
            {
                MessageBox.Show("Invalid date format. Please enter a valid date.");
                return;
            }
            decimal unitPrice = decimal.Parse(textBox6.Text);

            NewProduct = new Product(name, article, quantity, manufacturer, arrivalDate, unitPrice);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
