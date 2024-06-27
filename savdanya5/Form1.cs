using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;
namespace savdanya5
{
    public struct Product
    {
        public Product(string name, string article, int quantity, string manufacturer, DateTime arrivalDate, decimal unitPrice)
        {
            Name = name;
            Article = article;
            Quantity = quantity;
            Manufacturer = manufacturer;
            ArrivalDate = arrivalDate;
            UnitPrice = unitPrice;
        }

        public string Name { get; set; }
        public string Article { get; set; }
        public int Quantity { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal UnitPrice { get; set; }
    }
   
    public partial class Tovar : Form
    {
        private List<Product> products;
        public Tovar()
        {
            InitializeComponent(); products = new List<Product>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        

            using (var AddProductForm = new addform())
            {
                if (AddProductForm.ShowDialog() == DialogResult.OK)
                {
                    AddProduct(AddProductForm.NewProduct);
                }
            }

        }
        private void AddProduct(Product product)
        {
            products.Add(product);
            dataGridView1.Rows.Add(product.Name, product.Article, product.Quantity, product.Manufacturer, product.ArrivalDate.ToShortDateString(), product.UnitPrice);
        }
        private void UpdateProduct(int index, Product product)
        {
            products[index] = product;
            var row = dataGridView1.Rows[index];
            row.Cells[0].Value = product.Name;
            row.Cells[1].Value = product.Article;
            row.Cells[2].Value = product.Quantity;
            row.Cells[3].Value = product.Manufacturer;
            row.Cells[4].Value = product.ArrivalDate.ToShortDateString();
            row.Cells[5].Value = product.UnitPrice;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                var selectedProduct = products[selectedIndex];

                using (var EditProductForm = new redactcs(selectedProduct))
                {
                    if (EditProductForm.ShowDialog() == DialogResult.OK)
                    {
                        UpdateProduct(selectedIndex, EditProductForm.EditedProduct);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }
        int rowindex;
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        private void DeleteProduct(int index)
        {
            products.RemoveAt(index);
            dataGridView1.Rows.RemoveAt(index);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                var result = MessageBox.Show("Are you sure you want to delete this product?", "Delete Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    DeleteProduct(selectedIndex);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }
        private void SaveProductsToExcel(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Товари");
                worksheet.Cell(1, 1).Value = "Назва товару";
                worksheet.Cell(1, 2).Value = "Артикул";
                worksheet.Cell(1, 3).Value = "Кількість";
                worksheet.Cell(1, 4).Value = "Виробник";
                worksheet.Cell(1, 5).Value = "Дата надходження";
                worksheet.Cell(1, 6).Value = "Ціна за одиницю";

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    worksheet.Cell(i + 2, 1).Value = product.Name;
                    worksheet.Cell(i + 2, 2).Value = product.Article;
                    worksheet.Cell(i + 2, 3).Value = product.Quantity;
                    worksheet.Cell(i + 2, 4).Value = product.Manufacturer;
                    worksheet.Cell(i + 2, 5).Value = product.ArrivalDate;
                    worksheet.Cell(i + 2, 6).Value = product.UnitPrice;
                }

                workbook.SaveAs(filePath);
            }
        }

        private void LoadProductsFromExcel(string filePath)
        {
            products.Clear();
            dataGridView1.Rows.Clear();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); 

                foreach (var row in rows)
                {
                    var product = new Product
                    {
                        Name = row.Cell(1).GetValue<string>(),
                        Article = row.Cell(2).GetValue<string>(),
                        Quantity = row.Cell(3).GetValue<int>(),
                        Manufacturer = row.Cell(4).GetValue<string>(),
                        ArrivalDate = row.Cell(5).GetValue<DateTime>(),
                        UnitPrice = row.Cell(6).GetValue<decimal>()
                    };

                    AddProduct(product);
                }
            }
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Зберегти як Excel файл";
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                SaveProductsToExcel(saveFileDialog.FileName);
                MessageBox.Show("Файл успішно збережено.");
            }
        }

        private void прочитатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProductsFromExcel(openFileDialog.FileName);
                }
            }
        }

        private void очиститиВсіДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            products.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
        }
        private void FilterProducts()
        {
            var filteredProducts = products.Where(p =>
                (string.IsNullOrEmpty(textBox1.Text) || p.Name.Contains(textBox1.Text)) &&
                (string.IsNullOrEmpty(textBox2.Text) || p.Article.Contains(textBox2.Text)) &&
                (string.IsNullOrEmpty(textBox3.Text) || p.Quantity.ToString().Contains(textBox3.Text)) &&
                (string.IsNullOrEmpty(textBox4.Text) || p.Manufacturer.Contains(textBox4.Text)) &&
                (string.IsNullOrEmpty(textBox5.Text) || p.ArrivalDate.ToShortDateString().Contains(textBox5.Text)) &&
                (string.IsNullOrEmpty(textBox6.Text) || p.UnitPrice.ToString().Contains(textBox6.Text))
            ).ToList();

            dataGridView2.Rows.Clear();

            foreach (var product in filteredProducts)
            {
                dataGridView2.Rows.Add(product.Name, product.Article, product.Quantity, product.Manufacturer, product.ArrivalDate.ToShortDateString(), product.UnitPrice);
            }

        }
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }
    }
}
