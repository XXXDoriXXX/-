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
    public partial class redactcs : Form
    {
        public redactcs(Product product)
        {
            InitializeComponent();

            textBox1.Text = product.Name;
            textBox2.Text = product.Article;
            textBox3.Text = product.Quantity.ToString();
            textBox4.Text = product.Manufacturer;
            textBox5.Text = product.ArrivalDate.ToShortDateString();
            textBox6.Text = product.UnitPrice.ToString();
        }
        public Product EditedProduct { get; private set; }

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

            EditedProduct = new Product(name, article, quantity, manufacturer, arrivalDate, unitPrice);
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
