<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.PC_Name = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IP_Address = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.GroupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveConnectionsToFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RetryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QuickActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.PC_Name, Me.IP_Address})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.LargeImageList = Me.ImageList1
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(910, 291)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Tile
        '
        'PC_Name
        '
        Me.PC_Name.Text = "Name"
        '
        'IP_Address
        '
        Me.IP_Address.Text = "IP Address"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "laptop_off.png")
        Me.ImageList1.Images.SetKeyName(1, "laptop_on.png")
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GroupToolStripMenuItem, Me.SearchToolStripMenuItem, Me.SaveConnectionsToFileToolStripMenuItem, Me.RetryToolStripMenuItem, Me.QuickActionsToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(200, 114)
        '
        'GroupToolStripMenuItem
        '
        Me.GroupToolStripMenuItem.Image = Global.Master.My.Resources.Resources.category
        Me.GroupToolStripMenuItem.Name = "GroupToolStripMenuItem"
        Me.GroupToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.GroupToolStripMenuItem.Text = "Group"
        '
        'SearchToolStripMenuItem
        '
        Me.SearchToolStripMenuItem.Image = Global.Master.My.Resources.Resources.computer_add
        Me.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        Me.SearchToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.SearchToolStripMenuItem.Text = "Search"
        '
        'SaveConnectionsToFileToolStripMenuItem
        '
        Me.SaveConnectionsToFileToolStripMenuItem.Image = Global.Master.My.Resources.Resources.save_as
        Me.SaveConnectionsToFileToolStripMenuItem.Name = "SaveConnectionsToFileToolStripMenuItem"
        Me.SaveConnectionsToFileToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.SaveConnectionsToFileToolStripMenuItem.Text = "Save connections to file"
        '
        'RetryToolStripMenuItem
        '
        Me.RetryToolStripMenuItem.Image = Global.Master.My.Resources.Resources.update
        Me.RetryToolStripMenuItem.Name = "RetryToolStripMenuItem"
        Me.RetryToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.RetryToolStripMenuItem.Text = "Retry"
        '
        'QuickActionsToolStripMenuItem
        '
        Me.QuickActionsToolStripMenuItem.Image = Global.Master.My.Resources.Resources.bulb
        Me.QuickActionsToolStripMenuItem.Name = "QuickActionsToolStripMenuItem"
        Me.QuickActionsToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.QuickActionsToolStripMenuItem.Text = "Quick Actions"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListView1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListView2)
        Me.SplitContainer1.Size = New System.Drawing.Size(910, 402)
        Me.SplitContainer1.SplitterDistance = 291
        Me.SplitContainer1.TabIndex = 1
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView2.LargeImageList = Me.ImageList2
        Me.ListView2.Location = New System.Drawing.Point(0, 0)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(910, 107)
        Me.ListView2.SmallImageList = Me.ImageList2
        Me.ListView2.TabIndex = 0
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Machine"
        Me.ColumnHeader1.Width = 120
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "IP Address"
        Me.ColumnHeader2.Width = 120
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Message"
        Me.ColumnHeader3.Width = 500
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList2.Images.SetKeyName(0, "bullet_black.png")
        Me.ImageList2.Images.SetKeyName(1, "bullet_blue.png")
        Me.ImageList2.Images.SetKeyName(2, "bullet_red.png")
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(910, 402)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListView1 As ListView
    Friend WithEvents PC_Name As ColumnHeader
    Friend WithEvents IP_Address As ColumnHeader
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents GroupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveConnectionsToFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RetryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ListView2 As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents QuickActionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents ImageList2 As ImageList
End Class
