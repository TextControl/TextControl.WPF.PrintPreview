# TextControl.WPF.PrintPreview
## Building a Print Preview for TX Text Control .NET for WPF
A print preview control, that can be compared to the Windows Forms print preview control, is not part of the standard WPF control set.

In order to preview what is actually printed, a WPF DocumentViewer can be used.

Generally, this document viewer renders WPF FixedDocuments. Due to TX Text Control's special WYSIWYG rendering, TextControl can't be connected to this viewer directly.

Anyway, it is quite easy to utilize this out-of-the-box viewer effectively as a print preview control using TX Text Control's Page Rendering Engine.

Each page is rendered on a FixedPage of the FixedDocument and displayed in the standard viewer.
