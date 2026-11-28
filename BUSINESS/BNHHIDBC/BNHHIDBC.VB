Imports System.Data
Imports NTSInformatica.CLN__STD
' Imports System.Windows.Forms
Public Class FRMHHIDBC

#Region "moduli"

    Private Moduli_P As Integer = CLN__STD.bsModAll
    Private ModuliExt_P As Integer = CLN__STD.bsModExtAll
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

    Public ReadOnly Property Moduli() As Integer
        Get
            Return Moduli_P
        End Get
    End Property
    Public ReadOnly Property ModuliExt() As Integer
        Get
            Return ModuliExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliSup() As Integer
        Get
            Return ModuliSup_P
        End Get
    End Property
    Public ReadOnly Property ModuliSupExt() As Integer
        Get
            Return ModuliSupExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtn() As Integer
        Get
            Return ModuliPtn_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtnExt() As Integer
        Get
            Return ModuliPtnExt_P
        End Get
    End Property

#End Region

#Region "Variabili"
    Public oCleIdbc As CLEHHIDBC
    Public dsIdbc As DataSet
    Public dcIdbc As BindingSource = New BindingSource()
    Public oCallParams As CLE__CLDP
    Private components As System.ComponentModel.IContainer
    Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
    Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Public WithEvents tlbMain As NTSInformatica.NTSBar
    Public WithEvents tlbZoom As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbStrumenti As NTSInformatica.NTSBarSubItem
    Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
    Friend WithEvents tlbElabora As NTSBarButtonItem
    Friend WithEvents trDiba As NTSTreeView
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents fmRisultati As NTSGroupBox
    Friend WithEvents NtsSplitter1 As NTSSplitter
    Public WithEvents pnIdbc As NTSInformatica.NTSPanel

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
        If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHIDBC", "BEHHIDBC", objTmp, strErr, False, "", "") = False Then
            oApp.MsgBoxErr(oApp.Tr(Me, 128182574024687500, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
            Return False
        End If
        oCleIdbc = CType(objTmp, CLEHHIDBC)
        '------------------------------------------------

        AddHandler oCleIdbc.RemoteEvent, AddressOf GestisciEventiEntity
        If oCleIdbc.Init(oApp, NTSScript, oMenu.oCleComm, "", False, "", "") = False Then Return False

        Return True
    End Function

    Public Overridable Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHIDBC))
        Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
        Me.tlbMain = New NTSInformatica.NTSBar()
        Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
        Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
        Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
        Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.pnIdbc = New NTSInformatica.NTSPanel()
        Me.tlbElabora = New NTSInformatica.NTSBarButtonItem()
        Me.trDiba = New NTSInformatica.NTSTreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.fmRisultati = New NTSInformatica.NTSGroupBox()
        Me.NtsSplitter1 = New NTSInformatica.NTSSplitter()
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnIdbc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnIdbc.SuspendLayout()
        CType(Me.fmRisultati, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fmRisultati.SuspendLayout()
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
        Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbZoom, Me.tlbStrumenti, Me.tlbGuida, Me.tlbEsci, Me.tlbElabora})
        Me.NtsBarManager1.MaxItemId = 17
        '
        'tlbMain
        '
        Me.tlbMain.BarName = "tlbMain"
        Me.tlbMain.DockCol = 0
        Me.tlbMain.DockRow = 0
        Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbElabora), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbStrumenti, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbGuida, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
        Me.tlbMain.OptionsBar.AllowQuickCustomization = False
        Me.tlbMain.OptionsBar.DisableClose = True
        Me.tlbMain.OptionsBar.DrawDragBorder = False
        Me.tlbMain.OptionsBar.UseWholeRow = True
        Me.tlbMain.Text = "tlbMain"
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
        Me.tlbStrumenti.Name = "tlbStrumenti"
        Me.tlbStrumenti.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
        Me.tlbStrumenti.Visible = True
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
        'pnIdbc
        '
        Me.pnIdbc.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.pnIdbc.Appearance.Options.UseBackColor = True
        Me.pnIdbc.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pnIdbc.Controls.Add(Me.fmRisultati)
        Me.pnIdbc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnIdbc.Location = New System.Drawing.Point(0, 24)
        Me.pnIdbc.Name = "pnIdbc"
        Me.pnIdbc.Size = New System.Drawing.Size(684, 380)
        '
        'tlbElabora
        '
        Me.tlbElabora.Caption = "Elabora"
        Me.tlbElabora.Id = 16
        Me.tlbElabora.Name = "tlbElabora"
        Me.tlbElabora.Visible = True
        '
        'trDiba
        '
        Me.trDiba.Dock = System.Windows.Forms.DockStyle.Left
        Me.trDiba.Location = New System.Drawing.Point(2, 23)
        Me.trDiba.Name = "trDiba"
        Me.trDiba.Size = New System.Drawing.Size(481, 355)
        Me.trDiba.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'fmRisultati
        '
        Me.fmRisultati.AllowDrop = True
        Me.fmRisultati.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.fmRisultati.Appearance.Options.UseBackColor = True
        Me.fmRisultati.Controls.Add(Me.NtsSplitter1)
        Me.fmRisultati.Controls.Add(Me.trDiba)
        Me.fmRisultati.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fmRisultati.Location = New System.Drawing.Point(0, 0)
        Me.fmRisultati.Name = "fmRisultati"
        Me.fmRisultati.Size = New System.Drawing.Size(684, 380)
        Me.fmRisultati.Text = "RISULTATO IMPORTAZIONE DISTINTE BASE"
        '
        'NtsSplitter1
        '
        Me.NtsSplitter1.Location = New System.Drawing.Point(483, 23)
        Me.NtsSplitter1.Name = "NtsSplitter1"
        Me.NtsSplitter1.Size = New System.Drawing.Size(3, 355)
        Me.NtsSplitter1.TabIndex = 1
        Me.NtsSplitter1.TabStop = False
        '
        'FRMHHIDBC
        '
        Me.ClientSize = New System.Drawing.Size(684, 404)
        Me.Controls.Add(Me.pnIdbc)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.HelpButton = True
        Me.Name = "FRMHHIDBC"
        Me.Text = "IMPORT DISTINTE BASE DA DBCENT"
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnIdbc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnIdbc.ResumeLayout(False)
        CType(Me.fmRisultati, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fmRisultati.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#Region "Form"

    Public Overridable Sub FRMHHIDBC_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            dcIdbc.Dispose()
            If Not dsIdbc Is Nothing Then dsIdbc.Dispose()
        Catch ex As Exception

        End Try
    End Sub



    Public Overridable Sub FRMHHIDBC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ds As New DataSet
        Try
            'Predispongo i controlli
            InitControls()

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub
#End Region

#Region "Griglia"



#End Region


#Region "Eventi"
    Public Overridable Sub trDiba_AfterExpand(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles trDiba.AfterExpand

        Try
            AggiornaImmagineNodo(e.Node)
            trDiba.SelectedNode = e.Node
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try


    End Sub

    Public Overridable Sub trDiba_AfterCollapse(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles trDiba.AfterCollapse
        Try
            AggiornaImmagineNodo(e.Node)
            trDiba.SelectedNode = e.Node
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub trDiba_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles trDiba.AfterSelect
        AggiornaImmagineNodo(e.Node)
    End Sub

#End Region


    Public Overridable Sub InitControls()
        InitControlsBeginEndInit(Me, False)

        Try
            'Carico le immagini della Toolbar
            Try
                tlbZoom.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
                tlbStrumenti.GlyphPath = (oApp.ChildImageDir & "\options.png")
                tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
                tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
                tlbElabora.GlyphPath = (oApp.ChildImageDir & "\elabora.png")
            Catch ex As Exception
                'Non gestisco l'errore: 
                'se non c'è una immagine prendo quella a design
            End Try
            tlbMain.NTSSetToolTip()

            oApp.NTSSetImageListSize("TREEVIEW", ImageList1)
            ImageList1.Images.Add("prima", oApp.NTSBitmapFromFile("TREEVIEW", oApp.ChildImageDir & "\materia_prima.png")) ' prima materia prima
            ImageList1.Images.Add("semi", oApp.NTSBitmapFromFile("TREEVIEW", oApp.ChildImageDir & "\tr_closed.png")) 'semi semilavorato
            ImageList1.Images.Add("semi-ap", oApp.NTSBitmapFromFile("TREEVIEW", oApp.ChildImageDir & "\tr_open.png")) 'semi-ap semilavorato corrente
            ImageList1.Images.Add("lavo", oApp.NTSBitmapFromFile("TREEVIEW", oApp.ChildImageDir & "\chiaveinglese.png")) 'lavorazionr corrente
            trDiba.ImageList = ImageList1


            'Chiamo lo script per inizializzare i controlli caricati con Source Extender
            NTSScriptExec("InitControls", Me, Nothing)

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
        InitControlsBeginEndInit(Me, True)
    End Sub



#Region "Toolbar"

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
            Me.Close()
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbElabora_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbElabora.ItemClick
        Try
            Elabora()
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub
#End Region


#Region "Logica applicativa"

    Public Overridable Function Elabora() As Boolean
        Try
            trDiba.Nodes.Clear()

            If Not oCleIdbc.ElaboraDati() Then
                Return False
            End If

            VisualizzaDistinte()

            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Costruzione della rappresentazione TreeView
    ''' </summary>
    Public Overridable Sub VisualizzaDistinte()

        trDiba.BeginUpdate()

        Try

            trDiba.Nodes.Clear()

            For Each oDistinta As HHDTO_distbas In oCleIdbc.olsDistinteBase

                If oDistinta.oCodiceDistinta Is Nothing Then
                    Continue For
                End If

                ' Creo il nodo radice.
                Dim oNodoRadice As New TreeNode(CreaTestoNodo(oDistinta.oCodiceDistinta))
                ' imposta immagine
                ImpostaImmagineNodo(oNodoRadice, oDistinta.oCodiceDistinta.BOM_UID)

                oNodoRadice.Tag = oDistinta.oCodiceDistinta

                trDiba.Nodes.Add(oNodoRadice)

                ' Raggruppo le relazioni per UID padre.
                Dim dizFigli As New Dictionary(Of String, List(Of HHDTO_movdis_row))(StringComparer.OrdinalIgnoreCase)

                For Each oRelazione As HHDTO_movdis_row In oDistinta.oMovdis

                    If Not dizFigli.ContainsKey(oRelazione.UID_Padre) Then

                        dizFigli.Add(
                            oRelazione.UID_Padre,
                            New List(Of HHDTO_movdis_row)
                        )

                    End If

                    dizFigli(oRelazione.UID_Padre).Add(oRelazione)

                Next

                ' Creo l'insieme degli UID presenti nel percorso corrente.
                Dim hsPercorso As New HashSet(Of String)(
                    StringComparer.OrdinalIgnoreCase
                )

                hsPercorso.Add(oDistinta.oCodiceDistinta.BOM_UID)

                ' Costruisco tutti i livelli.
                AggiungiNodiFigli(
                    oNodoRadice,
                    oDistinta.oCodiceDistinta.BOM_UID,
                    dizFigli,
                    hsPercorso
                )

            Next

        Finally
            trDiba.EndUpdate()
        End Try

    End Sub

    Public Overridable Sub AggiungiNodiFigli(
                                                ByVal oNodoPadre As TreeNode,
                                                ByVal strUidPadre As String,
                                                ByVal dizFigli As Dictionary(Of String, List(Of HHDTO_movdis_row)),
                                                ByVal hsPercorso As HashSet(Of String)
                                            )

        If Not dizFigli.ContainsKey(strUidPadre) Then Return

        For Each oRelazione As HHDTO_movdis_row In dizFigli(strUidPadre)

            Dim strUidFiglio As String = oRelazione.UID_Figlio

            If hsPercorso.Contains(strUidFiglio) Then
                Throw New Exception(
                    "Ciclo rilevato durante la costruzione del TreeView: " &
                    strUidFiglio
                )
            End If

            ' Creo il nodo del componente.
            Dim oNodoFiglio As New TreeNode(CreaTestoNodo(oRelazione.oCodiceFiglio))

            ' Conservo la relazione associata a questa specifica occorrenza.
            oNodoFiglio.Tag = oRelazione

            ' imposta immagine nodo
            ImpostaImmagineNodo(oNodoFiglio, oRelazione.UID_Figlio)

            oNodoPadre.Nodes.Add(oNodoFiglio)

            ' Scendo ulteriormente soltanto se non è l'ultimo livello.
            If Not oRelazione.UltimoLivello Then

                hsPercorso.Add(strUidFiglio)

                AggiungiNodiFigli(
                    oNodoFiglio,
                    strUidFiglio,
                    dizFigli,
                    hsPercorso
                )

                hsPercorso.Remove(strUidFiglio)

            End If

        Next

    End Sub

    Public Overridable Function CreaTestoNodo(ByVal oArticolo As HHDTO_articolo) As String

        If oArticolo Is Nothing Then
            Return "(Articolo non disponibile)"
        End If

        Return oArticolo.CODE & " - " & oArticolo.DESC1

    End Function


    ''' <summary>
    '''  Assegna l'immagine al nodo in base alla tipologia del componente (PF,SL,MP)
    ''' </summary>
    ''' <param name="oNodo"></param>
    ''' <param name="strUID"></param>
    Public Overridable Sub ImpostaImmagineNodo(ByVal oNodo As TreeNode, ByVal strUID As String)

        Dim tipo As HH_TipoComponente
        If Not oCleIdbc.TryGetTipoComponente(strUID, tipo) Then
            Throw New Exception("Tipologia non determinata per l'articolo UID: " & strUID)
        End If

        Select Case tipo
            Case HH_TipoComponente.PF, HH_TipoComponente.SL
                oNodo.ImageIndex = 1
                oNodo.SelectedImageIndex = 2
            Case HH_TipoComponente.MP
                oNodo.ImageIndex = 0
                oNodo.SelectedImageIndex = 0
            Case Else
                Throw New Exception("Tipologia componente non gestita per UID: " & strUID)
        End Select
    End Sub


    ''' <summary>
    ''' Aggiornamento immagine dei nodi 
    ''' </summary>
    ''' <param name="oNodo"></param>
    Public Overridable Sub AggiornaImmagineNodo(ByVal oNodo As TreeNode)

        Try

            If oNodo Is Nothing Then Return
            Dim strUID As String = ""
            If TypeOf oNodo.Tag Is HHDTO_articolo Then
                strUID = DirectCast(oNodo.Tag, HHDTO_articolo).BOM_UID
            ElseIf TypeOf oNodo.Tag Is HHDTO_movdis_row Then
                strUID = DirectCast(oNodo.Tag, HHDTO_movdis_row).UID_Figlio

            End If

            Dim tipo As HH_TipoComponente
            If Not oCleIdbc.TryGetTipoComponente(strUID, tipo) Then Return

            Select Case tipo
                Case HH_TipoComponente.PF, HH_TipoComponente.SL
                    If oNodo.IsExpanded Then
                        oNodo.ImageIndex = 2
                        oNodo.SelectedImageIndex = 2
                    Else
                        oNodo.ImageIndex = 1
                        oNodo.SelectedImageIndex = 1
                    End If
                Case HH_TipoComponente.MP
                    oNodo.ImageIndex = 0
                    oNodo.SelectedImageIndex = 0
            End Select

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Sub
#End Region


End Class
