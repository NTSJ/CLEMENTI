Imports System.Data
Imports NTSInformatica.CLN__STD

Public Class FRM__SRPT

#Region "Variabili"

  Private components As System.ComponentModel.IContainer
  Public oPar As CLE__CLDP
  Public WithEvents opRidotta As NTSInformatica.NTSRadioButton
  Public WithEvents cmdProcedi As NTSInformatica.NTSButton
  Public WithEvents cmdAnnulla As NTSInformatica.NTSButton
  Public WithEvents opCompleta As NTSInformatica.NTSRadioButton
  Public WithEvents fmTipostampa As NTSInformatica.NTSGroupBox
  Public bControlBoxExit As Boolean

#End Region

  Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
    Try
      oMenu = Menu
      oApp = oMenu.App
      If Ditta <> "" Then
        DittaCorrente = Ditta
      Else
        DittaCorrente = oApp.Ditta
      End If

      InitializeComponent()
      Me.MinimumSize = Me.Size

      oPar = Param
      
      Return True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  Public Overridable Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM__SRPT))
    Me.fmTipostampa = New NTSInformatica.NTSGroupBox()
    Me.opCompleta = New NTSInformatica.NTSRadioButton()
    Me.opRidotta = New NTSInformatica.NTSRadioButton()
    Me.cmdProcedi = New NTSInformatica.NTSButton()
    Me.cmdAnnulla = New NTSInformatica.NTSButton()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmTipostampa, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmTipostampa.SuspendLayout()
    CType(Me.opCompleta.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.opRidotta.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'fmTipostampa
    '
    Me.fmTipostampa.AllowDrop = True
    Me.fmTipostampa.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmTipostampa.Appearance.Options.UseBackColor = True
    Me.fmTipostampa.Controls.Add(Me.opCompleta)
    Me.fmTipostampa.Controls.Add(Me.opRidotta)
    Me.fmTipostampa.Cursor = System.Windows.Forms.Cursors.Default
    Me.fmTipostampa.Location = New System.Drawing.Point(4, 4)
    Me.fmTipostampa.Name = "fmTipostampa"
    Me.fmTipostampa.Size = New System.Drawing.Size(176, 72)
    Me.fmTipostampa.TabIndex = 0
    Me.fmTipostampa.Text = "TIPO DI STAMPA"
    Me.fmTipostampa.Tile = False
    Me.fmTipostampa.TileIndex = -1
    '
    'opCompleta
    '
    Me.opCompleta.Cursor = System.Windows.Forms.Cursors.Default
    Me.opCompleta.Location = New System.Drawing.Point(4, 48)
    Me.opCompleta.Name = "opCompleta"
    Me.opCompleta.NTSBordeStyle = NTSInformatica.NTSRadioButton.NTSBorderStyle.NotSet
    Me.opCompleta.NTSPictureChecked = ""
    Me.opCompleta.NTSPictureUnchecked = ""
    Me.opCompleta.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.opCompleta.Properties.Appearance.Options.UseBackColor = True
    Me.opCompleta.Properties.Caption = "&Completa"
    Me.opCompleta.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined
    Me.opCompleta.Properties.PictureChecked = CType(resources.GetObject("opCompleta.Properties.PictureChecked"), System.Drawing.Image)
    Me.opCompleta.Properties.PictureUnchecked = CType(resources.GetObject("opCompleta.Properties.PictureUnchecked"), System.Drawing.Image)
    Me.opCompleta.Size = New System.Drawing.Size(108, 24)
    Me.opCompleta.TabIndex = 1
    '
    'opRidotta
    '
    Me.opRidotta.Cursor = System.Windows.Forms.Cursors.Default
    Me.opRidotta.EditValue = True
    Me.opRidotta.Location = New System.Drawing.Point(4, 24)
    Me.opRidotta.Name = "opRidotta"
    Me.opRidotta.NTSBordeStyle = NTSInformatica.NTSRadioButton.NTSBorderStyle.NotSet
    Me.opRidotta.NTSPictureChecked = ""
    Me.opRidotta.NTSPictureUnchecked = ""
    Me.opRidotta.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.opRidotta.Properties.Appearance.Options.UseBackColor = True
    Me.opRidotta.Properties.Caption = "&Ridotta"
    Me.opRidotta.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined
    Me.opRidotta.Properties.PictureChecked = CType(resources.GetObject("opRidotta.Properties.PictureChecked"), System.Drawing.Image)
    Me.opRidotta.Properties.PictureUnchecked = CType(resources.GetObject("opRidotta.Properties.PictureUnchecked"), System.Drawing.Image)
    Me.opRidotta.Size = New System.Drawing.Size(108, 24)
    Me.opRidotta.TabIndex = 0
    '
    'cmdProcedi
    '
    Me.cmdProcedi.ImageText = ""
    Me.cmdProcedi.Location = New System.Drawing.Point(184, 4)
    Me.cmdProcedi.Name = "cmdProcedi"
    Me.cmdProcedi.Size = New System.Drawing.Size(100, 26)
    Me.cmdProcedi.TabIndex = 1
    Me.cmdProcedi.Text = "&Procedi"
    '
    'cmdAnnulla
    '
    Me.cmdAnnulla.ImageText = ""
    Me.cmdAnnulla.Location = New System.Drawing.Point(184, 36)
    Me.cmdAnnulla.Name = "cmdAnnulla"
    Me.cmdAnnulla.Size = New System.Drawing.Size(100, 26)
    Me.cmdAnnulla.TabIndex = 2
    Me.cmdAnnulla.Text = "&Annulla"
    '
    'FRM__SRPT
    '
    Me.AcceptButton = Me.cmdProcedi
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.ClientSize = New System.Drawing.Size(287, 80)
    Me.Controls.Add(Me.cmdAnnulla)
    Me.Controls.Add(Me.cmdProcedi)
    Me.Controls.Add(Me.fmTipostampa)
    Me.Cursor = System.Windows.Forms.Cursors.Default
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FRM__SRPT"
    Me.Text = "SELEZIONA TIPO DI STAMPA"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmTipostampa, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmTipostampa.ResumeLayout(False)
    CType(Me.opCompleta.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.opRidotta.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Public Overridable Sub FRM__SRPT_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    'Cerco di uscure dal Control-Box
    If bControlBoxExit Then
      e.Cancel = True
    End If
  End Sub

  Public Overridable Sub FRM__SRPT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try
      'Predispongo i controlli
      InitControls()

      'Applico le impostazioni da GCTL
      'GctlTipoDoc = " "
      GctlSetRoules()
      GctlApplicaDefaultValue()

      Me.NTSDisableEnterComeTab()

      bControlBoxExit = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub InitControls()
    Try
      opCompleta.NTSSetParam(oMenu, oApp.Tr(Me, 130995097022965023, "Stampa completa"), "C")
      opRidotta.NTSSetParam(oMenu, oApp.Tr(Me, 130995097234900339, "Stampa ridotta"), "R")

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub cmdProcedi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcedi.Click
    oPar.bPar1 = False
    If opRidotta.Checked Then
      oPar.dPar1 = 1
    Else
      oPar.dPar1 = 2
    End If

    bControlBoxExit = False
    Me.Close()
  End Sub

  Public Overridable Sub cmdAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnulla.Click
    oPar.bPar1 = True

    bControlBoxExit = False
    Me.Close()
  End Sub

End Class
