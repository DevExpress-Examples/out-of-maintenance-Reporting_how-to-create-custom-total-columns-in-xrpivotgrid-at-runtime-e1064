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
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.XtraReports.UI.CrossTab;
using DevExpress.XtraPrinting;

namespace PivotWithCustomColumnsAtRuntime {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            using (XtraReport1 report = new XtraReport1()) {

                XRCrossTab crossTab = new XRCrossTab();
                report.Detail.Controls.Add(crossTab);

                SqlDataSource ds = new SqlDataSource(new Access97ConnectionParameters(@"|DataDirectory|\nwind.mdb", String.Empty, String.Empty));
                ds.Queries.Add(SelectQueryFluentBuilder
                    .AddTable("Order Details")
                    .SelectColumns(
                        "UnitPrice",
                        "Quantity",
                        "OrderID"
                    )
                    .Join("Products", "ProductID")
                    .SelectColumns(
                        "ProductName"
                    )
                    .Filter("[Order Details].[OrderID] < 10250")
                    .Build("SalesInfo"));

                crossTab.DataSource = ds;
                crossTab.DataMember = "SalesInfo";

                crossTab.RowFields.Add(new CrossTabRowField() { FieldName = "ProductName" });
                crossTab.ColumnFields.Add(new CrossTabColumnField() { FieldName = "OrderID" });
                crossTab.DataFields.Add(new CrossTabDataField() { FieldName = "UnitPrice" });
                crossTab.DataFields.Add(new CrossTabDataField() { FieldName = "Quantity" });
                crossTab.DataFields.Add(new CrossTabDataField());
                crossTab.GenerateLayout();
                /*
                +---------------------+---------------------------------------+---------------------------------------+
                | ProductName         | [OrderID]                             | Grand total                           |
                |                     +-------------+------------+------------+-------------+------------+------------+
                |                     | Unit Price  | Quantity   | Empty cell | UnitPrice   | Quantity   | Empty cell |      
                +---------------------+-------------+------------+------------+-------------+------------+------------+
                | [ProductName]       | [UnitPrice] | [Quantity] | Empty cell | [UnitPrice] | [Quantity] | Empty cell |     
                +---------------------+-------------+------------+------------+-------------+------------+------------+                
                | Grand Total         |             |            |            |             |            |            |
                +---------------------+-------------+------------+------------+-------------+------------+------------+
                */

                //Adjust generated cells
                foreach (var c in crossTab.ColumnDefinitions) {
                    //Enable auto-width for all columns
                    c.AutoWidthMode = DevExpress.XtraReports.UI.AutoSizeMode.ShrinkAndGrow;
                }

                foreach (XRCrossTabCell c in crossTab.Cells) {
                    if (c.DataLevel == 0 && c.RowIndex != 1) {
                        //Adjust format string for the "UnitPrice" cells
                        c.TextFormatString = "{0:c}";
                    }

                    if (c.RowIndex == 0 && c.ColumnLevel == 0) {
                        //Adjust format string for the "OrderID" cells
                        c.TextFormatString = "Order {0}";
                    }

                    if (c.DataLevel == 2 && c.RowIndex != 1) {
                        //Set custom expression for empty data cells
                        c.ExpressionBindings.Add(new ExpressionBinding("Text", "[UnitPrice] * [Quantity]"));
                        c.Font = new Font(c.Font, FontStyle.Bold);
                        c.TextFormatString = "{0:c}";
                    }

                    if (c.DataLevel == 2 && c.RowIndex == 1) {
                        //Set text for empty header cells
                        c.Font = new Font(c.Font, FontStyle.Bold);
                        c.Text = "Total";
                    }                    
                }


                // Assign styles to cross tab
                crossTab.CrossTabStyles.GeneralStyle = new XRControlStyle() {
                    Name = "Default",
                    Borders = BorderSide.All,
                    Padding = new PaddingInfo() { All = 2 }
                };
                crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle = new XRControlStyle() {
                    Name = "Data",
                    TextAlignment = TextAlignment.TopRight
                };
                crossTab.CrossTabStyles.HeaderAreaStyle = new XRControlStyle() {
                    Name = "HeaderAndTotals",
                    BackColor = Color.WhiteSmoke
                };

                report.ShowRibbonPreviewDialog();
            }


        }
    }
}