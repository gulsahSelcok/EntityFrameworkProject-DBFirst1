using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBFirst_ProductCRUD
{
    public partial class Form1 : Form
    {
        //partial anlamı başka alanda da bu alanın bir parçası olablir anlamına gelir. 
        public Form1()
        {
            InitializeComponent();
        }
        NorthwindEntities db = new NorthwindEntities();
        public static int choosenProduct;
        private void Form1_Load(object sender, EventArgs e)
        {
            SupplierFill();
            CategoryFill();
            DataFill();
        }
        public void SupplierFill()
        {
            var supplier = db.Suppliers.Select(x=> new {
                SupID=x.SupplierID,
                ComName=x.CompanyName
            }).ToList();

            cbSupplier.DisplayMember = "ComName";
            cbSupplier.ValueMember = "SupID";
            cbSupplier.DataSource = supplier;        
        }
        public void CategoryFill()
        {
            var category = db.Categories.Select(x=> new {
                CatName= x.CategoryName,
                CatId=x.CategoryID
            }).ToList();

            cbCategory.DisplayMember = "CatName";
            cbCategory.ValueMember = "CatId";
            cbCategory.DataSource = category;
        }

        private void DataFill()
        {
            var productList = db.Products.Select(x => new
            {
                x.ProductID,                
                x.ProductName,
                x.Category.CategoryName,
                x.Supplier.CompanyName,
                x.UnitPrice,
                x.UnitsInStock
            }).Where(x => x.ProductName.Contains(txtSearch.Text)).ToList();

            dataGridView1.DataSource = productList;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataFill();
        }       
       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Product product = new Product();
            product.ProductName = txtProductName.Text;
            product.UnitsInStock = Convert.ToInt16(txtStock.Text);
            product.UnitPrice = Convert.ToDecimal(txtPrice.Text);
            product.CategoryID = (int)cbCategory.SelectedValue;
            product.SupplierID = (int)cbSupplier.SelectedValue;
            db.Products.Add(product);
            db.SaveChanges();
            DataFill();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            choosenProduct = (int)dataGridView1.CurrentRow.Cells[0].Value;
            Product product = db.Products.Where(x=>x.ProductID == choosenProduct).FirstOrDefault();
            txtProductName.Text = product.ProductName;
            txtStock.Text = product.UnitsInStock.ToString();
            txtPrice.Text = product.UnitPrice.ToString();
            cbCategory.SelectedValue = product.CategoryID;
            cbSupplier.SelectedValue = product.SupplierID;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Product product = db.Products.Where(x => x.ProductID == choosenProduct).FirstOrDefault();
            product.ProductName = txtProductName.Text;
            product.UnitsInStock = Convert.ToInt16(txtStock.Text);
            product.UnitPrice = Convert.ToDecimal(txtPrice.Text);
            product.CategoryID = (int)cbCategory.SelectedValue;
            product.SupplierID = (int)cbSupplier.SelectedValue;
            db.SaveChanges();
            DataFill();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Product product = db.Products.Where(x => x.ProductID == choosenProduct).FirstOrDefault();
            db.Products.Remove(product);
            DataFill();
        }
    }
}
