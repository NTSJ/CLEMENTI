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
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnIdbc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnIdbc.SuspendLayout()
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
        Me.pnIdbc.Controls.Add(Me.trDiba)
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
        Me.trDiba.Dock = System.Windows.Forms.DockStyle.Fill
        Me.trDiba.Location = New System.Drawing.Point(0, 0)
        Me.trDiba.Name = "trDiba"
        Me.trDiba.Size = New System.Drawing.Size(684, 380)
        Me.trDiba.TabIndex = 0
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

    Public Overridable Sub VisualizzaDistinte()

        trDiba.BeginUpdate()

        Try

            trDiba.Nodes.Clear()

            For Each oDistinta As HHDTO_distbas In oCleIdbc.olsDistinteBase

                If oDistinta.oCodiceDistinta Is Nothing Then
                    Continue For
                End If

                ' Creo il nodo radice.
                Dim oNodoRadice As New TreeNode(
                    CreaTestoNodo(oDistinta.oCodiceDistinta)
                )

                oNodoRadice.Tag = oDistinta.oCodiceDistinta

                trDiba.Nodes.Add(oNodoRadice)

                ' Raggruppo le relazioni per UID padre.
                Dim dizFigli As New Dictionary(
                    Of String, List(Of HHDTO_movdis_row)
                )(StringComparer.OrdinalIgnoreCase)

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
            Dim oNodoFiglio As New TreeNode(
                CreaTestoNodo(oRelazione.oCodiceFiglio)
            )

            ' Conservo la relazione associata a questa specifica occorrenza.
            oNodoFiglio.Tag = oRelazione

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

    Public Overridable Function CreaTestoNodo(
    ByVal oArticolo As HHDTO_articolo) As String

        If oArticolo Is Nothing Then
            Return "(Articolo non disponibile)"
        End If

        Return oArticolo.CODE & " - " & oArticolo.DESC1

    End Function


#End Region

End Class
