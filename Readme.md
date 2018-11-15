<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/PivotWithCustomColumnsAtRuntime/Form1.cs) (VB: [Form1.vb](./VB/PivotWithCustomColumnsAtRuntime/Form1.vb))
* [Program.cs](./CS/PivotWithCustomColumnsAtRuntime/Program.cs) (VB: [Program.vb](./VB/PivotWithCustomColumnsAtRuntime/Program.vb))
* [XtraReport1.cs](./CS/PivotWithCustomColumnsAtRuntime/XtraReport1.cs) (VB: [XtraReport1.vb](./VB/PivotWithCustomColumnsAtRuntime/XtraReport1.vb))
<!-- default file list end -->
# How to create custom total columns in xrPivotGrid at runtime


<p>This example illustrates how to create the xrPivotGrid control with custom total columns at runtime. You can implement calculated columns by using unbound columns. To accomplish this task you need to add an unbound field to the Data area and then handle the PivotGridControl.CustomCellDisplayText event to calculate the summary from the columns that have already been calculated.</p>

<br/>


