Imports System.Data
Imports NTSInformatica.CLN__STD

Public Class FRM__DUAR
#Region "Variabili"
  Public oCallParams As CLE__CLDP
  Public oCleCiva As CLE__CIVA
  Public bOk As Boolean
  Public strCodDupl As String = ""
#End Region

#Region "Controlli"
  Public WithEvents cmdAnnulla As NTSInformatica.NTSButton
  Public WithEvents lbCod As NTSInformatica.NTSLabel
  Public WithEvents edCod As NTSInformatica.NTSTextBoxNum
  Public WithEvents fmNuovo As NTSGroupBox
  Public WithEvents cmdOk As NTSInformatica.NTSButton
#End Region

#Region "Init"
  Public Overridable Sub InitializeComponent()
    Me.edCod = New NTSInformatica.NTSTextBoxNum()
    Me.lbCod = New NTSInformatica.NTSLabel()
    Me.cmdAnnulla = New NTSInformatica.NTSButton()
    Me.cmdOk = New NTSInformatica.NTSButton()
    Me.fmNuovo = New NTSInformatica.NTSGroupBox()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edCod.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmNuovo, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmNuovo.SuspendLayout()
    Me.SuspendLayout()
    '
    'frmPopup
    '
    
    
    
    
    '
    'frmAuto
    '
    
    
    
    
    '
    'edCod
    '
    Me.edCod.Location = New System.Drawing.Point(108, 28)
    Me.edCod.Name = "edCod"
    Me.edCod.Properties.Appearance.Options.UseTextOptions = True
    Me.edCod.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edCod.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edCod.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edCod.Properties.AutoHeight = False
    Me.edCod.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edCod.Properties.MaxLength = 65536
    Me.edCod.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edCod.Size = New System.Drawing.Size(68, 20)
    Me.edCod.TabIndex = 583
    '
    'lbCod
    '
    Me.lbCod.BackColor = System.Drawing.Color.Transparent
    Me.lbCod.Location = New System.Drawing.Point(4, 28)
    Me.lbCod.Name = "lbCod"
    Me.lbCod.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbCod.Size = New System.Drawing.Size(100, 20)
    Me.lbCod.TabIndex = 582
    Me.lbCod.Text = "Codice Iva nuovo"
    Me.lbCod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.lbCod.Tooltip = ""
    Me.lbCod.UseMnemonic = False
    '
    'cmdAnnulla
    '
    Me.cmdAnnulla.ImageText = ""
    Me.cmdAnnulla.Location = New System.Drawing.Point(204, 36)
    Me.cmdAnnulla.Name = "cmdAnnulla"
    Me.cmdAnnulla.Size = New System.Drawing.Size(136, 26)
    Me.cmdAnnulla.TabIndex = 10
    Me.cmdAnnulla.Text = "&Annulla"
    '
    'cmdOk
    '
    Me.cmdOk.ImageText = ""
    Me.cmdOk.Location = New System.Drawing.Point(204, 4)
    Me.cmdOk.Name = "cmdOk"
    Me.cmdOk.Size = New System.Drawing.Size(136, 26)
    Me.cmdOk.TabIndex = 11
    Me.cmdOk.Text = "&Conferma"
    '
    'fmNuovo
    '
    Me.fmNuovo.AllowDrop = True
    Me.fmNuovo.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmNuovo.Appearance.Options.UseBackColor = True
    Me.fmNuovo.Controls.Add(Me.edCod)
    Me.fmNuovo.Controls.Add(Me.lbCod)
    Me.fmNuovo.Location = New System.Drawing.Point(4, 4)
    Me.fmNuovo.Name = "fmNuovo"
    Me.fmNuovo.ShowCaption = False
    Me.fmNuovo.Size = New System.Drawing.Size(196, 72)
    Me.fmNuovo.TabIndex = 14
    Me.fmNuovo.Tile = False
    Me.fmNuovo.TileIndex = -1
    '
    'FRM__DUAR
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.ClientSize = New System.Drawing.Size(344, 80)
    Me.Controls.Add(Me.fmNuovo)
    Me.Controls.Add(Me.cmdAnnulla)
    Me.Controls.Add(Me.cmdOk)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.HelpContext = ""
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FRM__DUAR"
    Me.Text = "DUPLICAZIONE"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edCod.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmNuovo, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmNuovo.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

  Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
    oMenu = Menu
    oApp = oMenu.App
    oCallParams = Param
    If Ditta <> "" Then
      DittaCorrente = Ditta
    Else
      DittaCorrente = oApp.Ditta
    End If
    Me.GctlTipoDoc = ""

    InitializeComponent()
    Me.MinimumSize = Me.Size

    Return True
  End Function

  Public Overridable Sub InitControls()
    InitControlsBeginEndInit(Me, False)
    Try
      edCod.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130722698207846286, "Codice iva"), tabciva)

      '-------------------------------------------------
      'chiamo lo script per inizializzare i controlli caricati con source ext
      NTSScriptExec("InitControls", Me, Nothing)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
    InitControlsBeginEndInit(Me, True)
  End Sub
#End Region

#Region "Eventi di Form"
  Public Overridable Sub FRM__DUAR_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try
      AddHandler oCleCiva.RemoteEvent, AddressOf GestisciEventiEntity

      edCod.Text = "0"

      '-------------------------------------------------
      'predispongo i controlli
      InitControls()

      '--------------------------------------------
      'sempre alla fine di questa funzione: applico le regole della gctl
      GctlSetRoules()
      GctlApplicaDefaultValue()

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub FRM__DUAR_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    Dim ctrlTmp As Control = Nothing
    Try
      '--------------------------------------------
      'gestione dello zoom:
      'eseguo la Zoom, tanto se per il controllo non deve venir eseguito uno zoom particolare, la routine non fa nulla e viene processato lo zoom standard del controllo, settato con la NTSSetParamZoom
      If e.KeyCode = Keys.F5 And e.Control = False And e.Alt = False And e.Shift = False Then
        Zoom()
        e.Handled = True    'altrimenti anche il controllo riceve l'F5 e la routine ZOOM viene eseguita 2 volte!!!
      End If

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Zoom / ALT+F3 / ALT+F3"
  Public Overridable Sub Zoom()
    Dim ctlLastControl As Control
    Dim ctrlTmp As Control = Nothing
    Dim oParam As New CLE__CLDP

    Try
      ctlLastControl = NTSFindControlFocused(Me)
      If ctlLastControl Is Nothing Then Return

      '------------------------------------
      'zoom standard di textbox e griglia
      'SendKeys.SendWait("{F5}") 'se faccio questa riga va in un loop infinito....
      NTSCallStandardZoom()

      '------------------------------------
      'riporto il focus sul controllo che ha chiamato lo zoom
      ctlLastControl.Focus()

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Controlii"
  Public Overridable Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
    Try
      If Not oCleCiva.CheckCod(edCod.Text) Then
        Exit Sub
      Else
        strCodDupl = edCod.Text
        bOk = True
        Me.Close()
      End If

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub cmdAnnulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAnnulla.Click
    Try
      bOk = False
      Me.Close()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub
#End Region

End Class