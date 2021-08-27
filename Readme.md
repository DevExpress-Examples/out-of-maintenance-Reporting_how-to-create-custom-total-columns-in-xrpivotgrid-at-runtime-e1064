<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128599820/19.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1064)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/PivotWithCustomColumnsAtRuntime/Form1.cs) (VB: [Form1.vb](./VB/PivotWithCustomColumnsAtRuntime/Form1.vb))
<!-- default file list end -->
# How to create custom total columns in XRCrossTab at runtime

This example demonstrates how to create an XRCrossTab control with custom total columns at runtime. You can implement calculated columns by using unbound empty columns. To accomplish this task, you need to add an unbound field to the Data area and then, after the cross tab's layout was generated, assign desired expressions to the Text property of the cells created for these empty fields.
