Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UI.PivotGrid
Imports DevExpress.XtraPivotGrid
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.XtraReports.UI.CrossTab
Imports DevExpress.XtraPrinting

Namespace PivotWithCustomColumnsAtRuntime
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Using report As New XtraReport1()

				Dim crossTab As New XRCrossTab()
				report.Detail.Controls.Add(crossTab)

				Dim ds As New SqlDataSource(New Access97ConnectionParameters("|DataDirectory|\nwind.mdb", String.Empty, String.Empty))
				ds.Queries.Add(SelectQueryFluentBuilder.AddTable("Order Details").SelectColumns("UnitPrice", "Quantity", "OrderID").Join("Products", "ProductID").SelectColumns("ProductName").Filter("[Order Details].[OrderID] < 10250").Build("SalesInfo"))

				crossTab.DataSource = ds
				crossTab.DataMember = "SalesInfo"

				crossTab.RowFields.Add(New CrossTabRowField() With {.FieldName = "ProductName"})
				crossTab.ColumnFields.Add(New CrossTabColumnField() With {.FieldName = "OrderID"})
				crossTab.DataFields.Add(New CrossTabDataField() With {.FieldName = "UnitPrice"})
				crossTab.DataFields.Add(New CrossTabDataField() With {.FieldName = "Quantity"})
				crossTab.DataFields.Add(New CrossTabDataField())
				crossTab.GenerateLayout()
'                
'                +---------------------+---------------------------------------+---------------------------------------+
'                | ProductName         | [OrderID]                             | Grand total                           |
'                |                     +-------------+------------+------------+-------------+------------+------------+
'                |                     | Unit Price  | Quantity   | Empty cell | UnitPrice   | Quantity   | Empty cell |      
'                +---------------------+-------------+------------+------------+-------------+------------+------------+
'                | [ProductName]       | [UnitPrice] | [Quantity] | Empty cell | [UnitPrice] | [Quantity] | Empty cell |     
'                +---------------------+-------------+------------+------------+-------------+------------+------------+                
'                | Grand Total         |             |            |            |             |            |            |
'                +---------------------+-------------+------------+------------+-------------+------------+------------+
'                

				'Adjust generated cells
				For Each c In crossTab.ColumnDefinitions
					'Enable auto-width for all columns
					c.AutoWidthMode = DevExpress.XtraReports.UI.AutoSizeMode.ShrinkAndGrow
				Next c

				For Each c As XRCrossTabCell In crossTab.Cells
					If c.DataLevel = 0 AndAlso c.RowIndex <> 1 Then
						'Adjust format string for the "UnitPrice" cells
						c.TextFormatString = "{0:c}"
					End If

					If c.RowIndex = 0 AndAlso c.ColumnLevel = 0 Then
						'Adjust format string for the "OrderID" cells
						c.TextFormatString = "Order {0}"
					End If

					If c.DataLevel = 2 AndAlso c.RowIndex <> 1 Then
						'Set custom expression for empty data cells
						c.ExpressionBindings.Add(New ExpressionBinding("Text", "[UnitPrice] * [Quantity]"))
						c.Font = New Font(c.Font, FontStyle.Bold)
						c.TextFormatString = "{0:c}"
					End If

					If c.DataLevel = 2 AndAlso c.RowIndex = 1 Then
						'Set text for empty header cells
						c.Font = New Font(c.Font, FontStyle.Bold)
						c.Text = "Total"
					End If
				Next c


				' Assign styles to cross tab
				crossTab.CrossTabStyles.GeneralStyle = New XRControlStyle() With {
					.Name = "Default",
					.Borders = BorderSide.All,
					.Padding = New PaddingInfo() With {.All = 2}
				}
'INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
'ORIGINAL LINE: crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle = new XRControlStyle() { Name = "Data", TextAlignment = TextAlignment.TopRight };
				crossTab.CrossTabStyles.TotalAreaStyle = New XRControlStyle() With {
					.Name = "Data",
					.TextAlignment = TextAlignment.TopRight
				}
				crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle
				crossTab.CrossTabStyles.HeaderAreaStyle = New XRControlStyle() With {
					.Name = "HeaderAndTotals",
					.BackColor = Color.WhiteSmoke
				}

				report.ShowRibbonPreviewDialog()
			End Using


		End Sub
	End Class
End Namespace