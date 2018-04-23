Imports Microsoft.VisualBasic
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

Namespace PivotWithCustomColumnsAtRuntime
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim report As New XtraReport1()
			Dim pivot As New XRPivotGrid()
			Dim Products As New DataSet()
			Using connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\nwind.mdb")
				Dim adapter As New OleDbDataAdapter("SELECT [Order Details].UnitPrice, [Order Details].Quantity, [Order Details].OrderID, Products.ProductName FROM ([Order Details] INNER JOIN Products ON [Order Details].ProductID = Products.ProductID) WHERE [Order Details].OrderID < 10250", connection)
				adapter.Fill(Products, "SalesInfo")
			End Using
			pivot.DataSource = Products
			pivot.DataMember = "SalesInfo"
			Dim fieldProductName As New XRPivotGridField("ProductName", PivotArea.RowArea)
			Dim fieldOrderID As New XRPivotGridField("OrderID", PivotArea.ColumnArea)
			Dim fieldUnitPrice As New XRPivotGridField("UnitPrice", PivotArea.DataArea)
			Dim fieldQuantity As New XRPivotGridField("Quantity", PivotArea.DataArea)
			Dim fieldTotalPrice As New XRPivotGridField()
			fieldTotalPrice.Area = PivotArea.DataArea
			fieldTotalPrice.FieldName = "TotalPrice"
			pivot.OptionsView.ShowColumnGrandTotals = False
			pivot.Fields.AddRange(New PivotGridField() { fieldProductName, fieldUnitPrice, fieldOrderID, fieldQuantity, fieldTotalPrice })
			AddHandler pivot.CustomCellDisplayText, AddressOf pivot_CustomCellDisplayText
			report.Detail.Controls.Add(pivot)
			report.ShowPreview()

		End Sub

		Private Sub pivot_CustomCellDisplayText(ByVal sender As Object, ByVal e As PivotCellDisplayTextEventArgs)
			If e.DataField.FieldName <> "TotalPrice" Then
				Return
			End If
			Dim result As Decimal = Convert.ToDecimal(e.GetFieldValue((CType(sender, XRPivotGrid)).Fields("UnitPrice"))) * Convert.ToDecimal(e.GetFieldValue((CType(sender, XRPivotGrid)).Fields("Quantity")))
			e.DisplayText = String.Format("${0:n2}", result)
		End Sub
	End Class
End Namespace