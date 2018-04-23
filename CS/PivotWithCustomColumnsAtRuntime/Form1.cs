using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.PivotGrid;
using DevExpress.XtraPivotGrid;

namespace PivotWithCustomColumnsAtRuntime
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XtraReport1 report = new XtraReport1();             
            XRPivotGrid pivot = new XRPivotGrid();
            DataSet Products = new DataSet();
            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\nwind.mdb"))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT [Order Details].UnitPrice, [Order Details].Quantity, [Order Details].OrderID, Products.ProductName FROM ([Order Details] INNER JOIN Products ON [Order Details].ProductID = Products.ProductID) WHERE [Order Details].OrderID < 10250", connection);
                adapter.Fill(Products, "SalesInfo");
            }
            pivot.DataSource = Products;
            pivot.DataMember = "SalesInfo";           
            XRPivotGridField fieldProductName = new XRPivotGridField("ProductName", PivotArea.RowArea);
            XRPivotGridField fieldOrderID = new XRPivotGridField("OrderID", PivotArea.ColumnArea);
            XRPivotGridField fieldUnitPrice = new XRPivotGridField("UnitPrice", PivotArea.DataArea);
            XRPivotGridField fieldQuantity = new XRPivotGridField("Quantity", PivotArea.DataArea);
            XRPivotGridField fieldTotalPrice = new XRPivotGridField();
            fieldTotalPrice.Area = PivotArea.DataArea;
            fieldTotalPrice.FieldName = "TotalPrice";
            pivot.OptionsView.ShowColumnGrandTotals = false;
            pivot.Fields.AddRange(new XRPivotGridField[] { fieldProductName, fieldUnitPrice, fieldOrderID, fieldQuantity, fieldTotalPrice });
            pivot.CustomCellDisplayText += new EventHandler<DevExpress.XtraReports.UI.PivotGrid.PivotCellDisplayTextEventArgs>(pivot_CustomCellDisplayText);

            report.Detail.Controls.Add(pivot);
            report.ShowPreview();           
            
        }

        void pivot_CustomCellDisplayText(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellDisplayTextEventArgs e)
        {
            if (e.DataField.FieldName != "TotalPrice")
                return;
            decimal result = Convert.ToDecimal(e.GetFieldValue(((XRPivotGrid)sender).Fields["UnitPrice"])) * Convert.ToDecimal(e.GetFieldValue(((XRPivotGrid)sender).Fields["Quantity"]));
            e.DisplayText = string.Format("${0:n2}", result);
        }
    }
}