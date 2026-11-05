Imports System.Data
Imports NTSInformatica.CLN__STD

Public Class FRMHHTASS



#Region "Variabili"
    Public oCleTass As CLEHHTASS
    Public dsTass As DataSet
    Public dcTass As BindingSource = New BindingSource()
    Public oCallParams As CLE__CLDP
    Private components As System.ComponentModel.IContainer
    Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
    Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Public WithEvents tlbMain As NTSInformatica.NTSBar
    Public WithEvents tlbNuovo As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbCancella As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbRipristina As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbZoom As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbStrumenti As NTSInformatica.NTSBarSubItem
    Public WithEvents tlbImpostaStampante As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
    Public WithEvents pnTass As NTSInformatica.NTSPanel
    Public WithEvents grTass As NTSInformatica.NTSGrid
    Public WithEvents tb_codtipoassicur As NTSGridColumn
    Public WithEvents tb_destipoassicur As NTSGridColumn
    Public WithEvents tb_flscadenza As NTSGridColumn
    Public WithEvents tb_duratamesi As NTSGridColumn
    Friend WithEvents tb_obsoleto As NTSGridColumn
    Public WithEvents grvTass As NTSInformatica.NTSGridView

#End Region

    Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
        oMenu = Menu
        oApp = oMenu.App
        oCallParams = Param
        If Ditta <> "" Then
            DittaCorrente = Ditta
        Else
            DittaCorrente = oApp.Ditta
        End If

        InitializeComponent()
        '------------------------------------------------
        'Creo e attivo l'entity, in più inizializzo la funzione che dovrà rilevare gli eventi dall'ENTITY stesso
        Dim strErr As String = ""
        Dim objTmp As Object = Nothing
        If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHTASS", "BEHHTASS", objTmp, strErr, False, "", "") = False Then
            oApp.MsgBoxErr(oApp.Tr(Me, 128182574024687500, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
            Return False
        End If
        oCleTass = CType(objTmp, CLEHHTASS)
        '------------------------------------------------

        AddHandler oCleTass.RemoteEvent, AddressOf GestisciEventiEntity
        If oCleTass.Init(oApp, NTSScript, oMenu.oCleComm, "HH_TIPIASSICURAZIONE", False, "", "") = False Then Return False

        Return True
    End Function

    Public Overridable Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHTASS))
        Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
        Me.tlbMain = New NTSInformatica.NTSBar()
        Me.tlbNuovo = New NTSInformatica.NTSBarButtonItem()
        Me.tlbRipristina = New NTSInformatica.NTSBarButtonItem()
        Me.tlbCancella = New NTSInformatica.NTSBarButtonItem()
        Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
        Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
        Me.tlbImpostaStampante = New NTSInformatica.NTSBarButtonItem()
        Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
        Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.pnTass = New NTSInformatica.NTSPanel()
        Me.grTass = New NTSInformatica.NTSGrid()
        Me.grvTass = New NTSInformatica.NTSGridView()
        Me.tb_codtipoassicur = New NTSInformatica.NTSGridColumn()
        Me.tb_destipoassicur = New NTSInformatica.NTSGridColumn()
        Me.tb_flscadenza = New NTSInformatica.NTSGridColumn()
        Me.tb_duratamesi = New NTSInformatica.NTSGridColumn()
        Me.tb_obsoleto = New NTSInformatica.NTSGridColumn()
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnTass, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnTass.SuspendLayout()
        CType(Me.grTass, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grvTass, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NtsBarManager1
        '
        Me.NtsBarManager1.AllowCustomization = False
        Me.NtsBarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.tlbMain})
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlTop)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlRight)
        Me.NtsBarManager1.Form = Me
        Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbNuovo, Me.tlbCancella, Me.tlbRipristina, Me.tlbZoom, Me.tlbStrumenti, Me.tlbImpostaStampante, Me.tlbGuida, Me.tlbEsci})
        Me.NtsBarManager1.MaxItemId = 16
        '
        'tlbMain
        '
        Me.tlbMain.BarName = "tlbMain"
        Me.tlbMain.DockCol = 0
        Me.tlbMain.DockRow = 0
        Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbNuovo), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristina), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancella), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbStrumenti, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbGuida, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
        Me.tlbMain.OptionsBar.AllowQuickCustomization = False
        Me.tlbMain.OptionsBar.DisableClose = True
        Me.tlbMain.OptionsBar.DrawDragBorder = False
        Me.tlbMain.OptionsBar.UseWholeRow = True
        Me.tlbMain.Text = "tlbMain"
        '
        'tlbNuovo
        '
        Me.tlbNuovo.Caption = "Nuovo"
        Me.tlbNuovo.Glyph = CType(resources.GetObject("tlbNuovo.Glyph"), System.Drawing.Image)
        Me.tlbNuovo.Id = 0
        Me.tlbNuovo.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.tlbNuovo.Name = "tlbNuovo"
        Me.tlbNuovo.Visible = True
        '
        'tlbRipristina
        '
        Me.tlbRipristina.Caption = "Ripristina"
        Me.tlbRipristina.Glyph = CType(resources.GetObject("tlbRipristina.Glyph"), System.Drawing.Image)
        Me.tlbRipristina.Id = 3
        Me.tlbRipristina.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.tlbRipristina.Name = "tlbRipristina"
        Me.tlbRipristina.Visible = True
        '
        'tlbCancella
        '
        Me.tlbCancella.Caption = "Cancella"
        Me.tlbCancella.Glyph = CType(resources.GetObject("tlbCancella.Glyph"), System.Drawing.Image)
        Me.tlbCancella.Id = 2
        Me.tlbCancella.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.tlbCancella.Name = "tlbCancella"
        Me.tlbCancella.Visible = True
        '
        'tlbZoom
        '
        Me.tlbZoom.Caption = "Zoom"
        Me.tlbZoom.Glyph = CType(resources.GetObject("tlbZoom.Glyph"), System.Drawing.Image)
        Me.tlbZoom.Id = 4
        Me.tlbZoom.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.tlbZoom.Name = "tlbZoom"
        Me.tlbZoom.Visible = True
        '
        'tlbStrumenti
        '
        Me.tlbStrumenti.Caption = "Strumenti"
        Me.tlbStrumenti.Glyph = CType(resources.GetObject("tlbStrumenti.Glyph"), System.Drawing.Image)
        Me.tlbStrumenti.Id = 9
        Me.tlbStrumenti.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbImpostaStampante)})
        Me.tlbStrumenti.Name = "tlbStrumenti"
        Me.tlbStrumenti.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
        Me.tlbStrumenti.Visible = True
        '
        'tlbImpostaStampante
        '
        Me.tlbImpostaStampante.Caption = "Imposta stampante"
        Me.tlbImpostaStampante.Id = 10
        Me.tlbImpostaStampante.Name = "tlbImpostaStampante"
        Me.tlbImpostaStampante.Visible = True
        '
        'tlbGuida
        '
        Me.tlbGuida.Caption = "Guida"
        Me.tlbGuida.Glyph = CType(resources.GetObject("tlbGuida.Glyph"), System.Drawing.Image)
        Me.tlbGuida.Id = 13
        Me.tlbGuida.Name = "tlbGuida"
        Me.tlbGuida.Visible = True
        '
        'tlbEsci
        '
        Me.tlbEsci.Caption = "Esci"
        Me.tlbEsci.Glyph = CType(resources.GetObject("tlbEsci.Glyph"), System.Drawing.Image)
        Me.tlbEsci.Id = 14
        Me.tlbEsci.Name = "tlbEsci"
        Me.tlbEsci.Visible = True
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.NtsBarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(684, 24)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 404)
        Me.barDockControlBottom.Manager = Me.NtsBarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(684, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 24)
        Me.barDockControlLeft.Manager = Me.NtsBarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 380)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(684, 24)
        Me.barDockControlRight.Manager = Me.NtsBarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 380)
        '
        'pnTass
        '
        Me.pnTass.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.pnTass.Appearance.Options.UseBackColor = True
        Me.pnTass.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pnTass.Controls.Add(Me.grTass)
        Me.pnTass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTass.Location = New System.Drawing.Point(0, 24)
        Me.pnTass.Name = "pnTass"
        Me.pnTass.Size = New System.Drawing.Size(684, 380)
        '
        'grTass
        '
        Me.grTass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grTass.Location = New System.Drawing.Point(0, 0)
        Me.grTass.MainView = Me.grvTass
        Me.grTass.Name = "grTass"
        Me.grTass.NTSNoRidimensionamentoUltimaColonna = False
        Me.grTass.Size = New System.Drawing.Size(684, 380)
        Me.grTass.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvTass})
        '
        'grvTass
        '
        Me.grvTass.ActiveFilterEnabled = False
        Me.grvTass.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.tb_codtipoassicur, Me.tb_destipoassicur, Me.tb_flscadenza, Me.tb_duratamesi, Me.tb_obsoleto})
        Me.grvTass.CustomizationFormBounds = New System.Drawing.Rectangle(680, 326, 208, 170)
        Me.grvTass.Enabled = True
        Me.grvTass.GridControl = Me.grTass
        Me.grvTass.Name = "grvTass"
        Me.grvTass.OptionsCustomization.AllowRowSizing = True
        Me.grvTass.OptionsFilter.AllowFilterEditor = False
        Me.grvTass.OptionsNavigation.EnterMoveNextColumn = True
        Me.grvTass.OptionsNavigation.UseTabKey = False
        Me.grvTass.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.grvTass.OptionsView.ColumnAutoWidth = False
        Me.grvTass.OptionsView.EnableAppearanceEvenRow = True
        Me.grvTass.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden
        Me.grvTass.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.grvTass.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        Me.grvTass.OptionsView.ShowGroupPanel = False
        '
        'tb_codtipoassicur
        '
        Me.tb_codtipoassicur.AppearanceCell.Options.UseBackColor = True
        Me.tb_codtipoassicur.AppearanceCell.Options.UseTextOptions = True
        Me.tb_codtipoassicur.Caption = "Codice"
        Me.tb_codtipoassicur.Enabled = True
        Me.tb_codtipoassicur.FieldName = "tb_codtipoassicur"
        Me.tb_codtipoassicur.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.tb_codtipoassicur.Name = "tb_codtipoassicur"
        Me.tb_codtipoassicur.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.tb_codtipoassicur.OptionsFilter.AllowFilter = False
        Me.tb_codtipoassicur.Visible = True
        Me.tb_codtipoassicur.VisibleIndex = 0
        '
        'tb_destipoassicur
        '
        Me.tb_destipoassicur.AppearanceCell.Options.UseBackColor = True
        Me.tb_destipoassicur.AppearanceCell.Options.UseTextOptions = True
        Me.tb_destipoassicur.Caption = "Descrizione"
        Me.tb_destipoassicur.Enabled = True
        Me.tb_destipoassicur.FieldName = "tb_destipoassicur"
        Me.tb_destipoassicur.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.tb_destipoassicur.Name = "tb_destipoassicur"
        Me.tb_destipoassicur.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.tb_destipoassicur.OptionsFilter.AllowFilter = False
        Me.tb_destipoassicur.Visible = True
        Me.tb_destipoassicur.VisibleIndex = 1
        '
        'tb_flscadenza
        '
        Me.tb_flscadenza.AppearanceCell.Options.UseBackColor = True
        Me.tb_flscadenza.AppearanceCell.Options.UseTextOptions = True
        Me.tb_flscadenza.Caption = "Gestione scadenza"
        Me.tb_flscadenza.Enabled = True
        Me.tb_flscadenza.FieldName = "tb_flscadenza"
        Me.tb_flscadenza.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.tb_flscadenza.Name = "tb_flscadenza"
        Me.tb_flscadenza.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.tb_flscadenza.OptionsFilter.AllowFilter = False
        Me.tb_flscadenza.Visible = True
        Me.tb_flscadenza.VisibleIndex = 2
        '
        'tb_duratamesi
        '
        Me.tb_duratamesi.AppearanceCell.Options.UseBackColor = True
        Me.tb_duratamesi.AppearanceCell.Options.UseTextOptions = True
        Me.tb_duratamesi.Caption = "Durata (mesi)"
        Me.tb_duratamesi.Enabled = True
        Me.tb_duratamesi.FieldName = "tb_duratamesi"
        Me.tb_duratamesi.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.tb_duratamesi.Name = "tb_duratamesi"
        Me.tb_duratamesi.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.tb_duratamesi.OptionsFilter.AllowFilter = False
        Me.tb_duratamesi.Visible = True
        Me.tb_duratamesi.VisibleIndex = 3
        '
        'tb_obsoleto
        '
        Me.tb_obsoleto.AppearanceCell.Options.UseBackColor = True
        Me.tb_obsoleto.AppearanceCell.Options.UseTextOptions = True
        Me.tb_obsoleto.Caption = "Obolseto"
        Me.tb_obsoleto.Enabled = True
        Me.tb_obsoleto.FieldName = "tb_obsoleto"
        Me.tb_obsoleto.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.tb_obsoleto.Name = "tb_obsoleto"
        Me.tb_obsoleto.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.tb_obsoleto.OptionsFilter.AllowFilter = False
        Me.tb_obsoleto.Visible = True
        Me.tb_obsoleto.VisibleIndex = 4
        '
        'FRMHHTASS
        '
        Me.ClientSize = New System.Drawing.Size(684, 404)
        Me.Controls.Add(Me.pnTass)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.HelpButton = True
        Me.Name = "FRMHHTASS"
        Me.Text = "CODICI IVA"
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnTass, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnTass.ResumeLayout(False)
        CType(Me.grTass, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grvTass, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#Region "Form"

    Public Overridable Sub FRMHHTASS_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            dcTass.Dispose()
            If Not dsTass Is Nothing Then dsTass.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Public Overridable Sub FRMHHTASS_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Not Salva() Then e.Cancel = True
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub FRMHHTASS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            'Predispongo i controlli
            InitControls()

            '-------------------------------------------------
            'Leggo dal database i dati e collego l'NTSBinding
            If oCleTass.Apri(DittaCorrente, dsTass) Then
                dcTass.DataSource = dsTass.Tables("HH_TIPIASSICURAZIONE")
                dsTass.AcceptChanges()

                grTass.DataSource = dcTass

                '-------------------------------------------------
                'Applico le impostazioni personalizzate della form (Ex: CTRL+ALT+F2)
                'questa funzione può essere chiamata quando si vuole,
                'ad esempio dopo aver impostato la ditta, cambiato il tipo documento, ecc ...
                '(Ex: CTRL+ALT+F4)
                'GctlTipoDoc = ""
                GctlSetRoules()

                '--------------------------------------------
                'sono stato chiamato da un altro child: mi posiziono sul record passatomi, se lo trovo
                If Not oCallParams Is Nothing Then
                    If Microsoft.VisualBasic.Left(oCallParams.strParam, 5) = "NUOV;" Then
                        If grvTass.NTSAllowInsert Then
                            grvTass.NTSNuovo()
                        End If
                    ElseIf Microsoft.VisualBasic.Mid(oCallParams.strParam, 6) <> "" Then
                        For i As Integer = 0 To dcTass.List.Count - 1
                            If CType(dcTass.Item(i), DataRowView)!tb_codtipoassicur.ToString = Microsoft.VisualBasic.Mid(oCallParams.strParam, 6) Then
                                dcTass.Position = i
                                Exit For
                            End If
                        Next
                    End If
                End If  'If Not oCallParams Is Nothing Then
            Else
                Me.Close()
            End If
            '--------------------------------------------------------------------------------------------------------------

            '--------------------------------------------------------------------------------------------------------------
            'Questa condizione serviva solo in CUBE1 (la colonna sottotipo iva doveva essere visibile se la versione era almeno
            '7, ma in cube 2 deve essere sempre visibile!)
            ''If oCleCiva.nVersionMinor <= 7 Then grvCiva.Columns.Remove(grvCiva.Columns("tb_art15"))
            '--------------------------------------------------------------------------------------------------------------


        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub
#End Region

#Region "Griglia"


    Public Overridable Sub grvCiva_NTSBeforeRowUpdate(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvTass.NTSBeforeRowUpdate
        Try
            If Not Salva() Then
                'Rimango sulla riga su cui sono
                e.Allow = False
            End If

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub


#End Region

    Public Overridable Sub InitControls()
        InitControlsBeginEndInit(Me, False)

        Try
            'Carico le immagini della Toolbar
            Try
                tlbNuovo.GlyphPath = (oApp.ChildImageDir & "\new.png")
                tlbCancella.GlyphPath = (oApp.ChildImageDir & "\delete.png")
                tlbRipristina.GlyphPath = (oApp.ChildImageDir & "\restore.png")
                tlbZoom.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
                tlbStrumenti.GlyphPath = (oApp.ChildImageDir & "\options.png")
                tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
                tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
            Catch ex As Exception
                'Non gestisco l'errore: 
                'se non c'è una immagine prendo quella a design
            End Try
            tlbMain.NTSSetToolTip()


            grvTass.NTSSetParam(oMenu, oApp.Tr(Me, 128230023372468841, "Griglia Codici IVA"))

            tb_codtipoassicur.NTSSetParamNUM(oMenu, "Codice", "#0", 0)
            tb_destipoassicur.NTSSetParamSTR(oMenu, "Descrizione", 500, True)
            tb_flscadenza.NTSSetParamCHK(oMenu, "Gestione scadenza", "S", "N")
            tb_duratamesi.NTSSetParamNUM(oMenu, "Durata (mesi)", "#0", 0)
            tb_obsoleto.NTSSetParamCHK(oMenu, "Obsoleto", "S", "N")


            'Chiamo lo script per inizializzare i controlli caricati con Source Extender
            NTSScriptExec("InitControls", Me, Nothing)

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
        InitControlsBeginEndInit(Me, True)
    End Sub

    Public Overridable Function Salva() As Boolean
        Try
            '----------------------------------------------------------------------------------------------------------
            Me.ValidaLastControl()
            '----------------------------------------------------------------------------------------------------------
            Select Case Me.NTSChiediSalva(grvTass, oCleTass.RecordIsChanged)
                Case DialogResult.Yes : If Not oCleTass.Salva(False) Then Return False
                Case DialogResult.No : oCleTass.Ripristina(dcTass.Position, dcTass.Filter)
                Case DialogResult.Cancel : Return False
            End Select
            '----------------------------------------------------------------------------------------------------------
            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function




#Region "Toolbar"

    Public Overridable Sub tlbNuovo_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbNuovo.ItemClick
        Try
            grvTass.NTSNuovo()
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub



    Public Overridable Sub tlbCancella_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancella.ItemClick
        Try
            '---------------------------------------------------------------------------------------------------------
            If grvTass.NTSGetCurrentDataRow Is Nothing Then Return
            '---------------------------------------------------------------------------------------------------------
            If oCleTass.TestPreCancella(grvTass.NTSGetCurrentDataRow) Then
                If Me.NTSChiediCancella(grvTass) = DialogResult.Yes Then oCleTass.Salva(True)
            End If
            '---------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub tlbRipristina_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbRipristina.ItemClick
        Try
            If grvTass.NTSRipristinaRigaCorrenteBefore(dcTass, CType(IIf(sender Is Nothing, False, True), Boolean)) Then
                oCleTass.Ripristina(dcTass.Position, dcTass.Filter)
                grvTass.NTSRipristinaRigaCorrenteAfter()
            End If
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick
        Dim oParam As New CLE__CLDP

        Try

            NTSCallStandardZoom()

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub



    Public Overridable Sub tlbGuida_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbGuida.ItemClick
    SendKeys.Send("{F1}")
  End Sub

  Public Overridable Sub tlbEsci_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
    Try
      If Not Salva() Then Return
      Me.Close()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbImpostaStampante_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbImpostaStampante.ItemClick
    Try
      oMenu.ReportImposta(Me)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

End Class
