Imports System.Data
Imports System.Linq
Imports NTSInformatica.CLN__STD

<Serializable()>
Public Class CLEHHIDBC
    Inherits CLE__BASE

#Region "variabili"


    Public oCldIdbc As CLDHHIDBC  'oggetto DAL

#End Region

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

    Public Overrides Function Init(ByRef App As CLE__APP,
                              ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                              ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                              ByVal strFiller2 As String) As Boolean
        '-----------------------------------------------------------------------------------------------------------
        '-----------------------------------------------------------------------------------------------------------
        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHIDBC"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
        oCldIdbc = CType(MyBase.ocldBase, CLDHHIDBC)
        oCldIdbc.Init(oApp)

        LeggiRegistro()
        Return True
    End Function

    Public Overridable Sub LeggiRegistro()
        Try


        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Function GetData_from_DBCent(ByRef ds As DataSet) As Boolean

        Dim dttLink As DataTable = Nothing ' BOMDB_LINK
        Dim dttFigli As DataTable = Nothing
        Dim dttArticoli As DataTable = Nothing ' BOMDB_DOC
        Dim oDistinteBase As New List(Of HHDTO_distbas) ' Elenco distitne base lette
        Try
            ' leggo l'intera tabella BOMDB_LINK
            If Not oCldIdbc.GetDataDBCent(ds) Then Return False
            dttLink = ds.Tables("BOMDB_LINK")
            dttArticoli = ds.Tables("BOMDB_DOC")

            ' Creo l'indice delle anagrafiche articoli
            Dim dizArticoli As Dictionary(Of String, HHDTO_articolo) = CreaIndiceArticoli(dttArticoli)

            ' ricavo solo i codici padre
            Dim dttPadre As DataTable = dttLink.Select("BOM_ORDER=BOM_PARENT and BOM_ORDER='BCLM0000005967'").
                                            CopyToDataTable().DefaultView.ToTable(True, {"BOM_ORDER", "BOM_PARENT", "CODE", "DESC1"})


            For Each dtr As DataRow In dttPadre.Rows

                Dim strCodicePF As String = dtr!BOM_ORDER.ToString

                dttFigli = Nothing
                Dim righe() As DataRow = dttLink.Select($"BOM_ORDER={CStrSQL(strCodicePF)}")

                If righe.Length > 0 Then
                    dttFigli = righe.CopyToDataTable()
                End If

                ' Creo la distinta del prodotto finito. HHDTO_distbas contiene l'oggetto movdis
                ' di tipo List (of HHDTO_movdis_row)
                Dim oDistinta As New HHDTO_distbas With {.strCoddb = strCodicePF}

                ' Avvio la lettura ricorsiva dalla radice
                LeggiRicorsivanenteDistinte(
                    dttFigli,
                    strCodicePF,
                    oDistinta.oMovdis,
                    dizArticoli,
                    1)

                oDistinteBase.Add(oDistinta)

            Next

            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function


    Private Sub LeggiRicorsivanenteDistinte(
                    ByVal dttBom As DataTable,
                    ByVal strCodicePadre As String,
                    ByRef lstRelazioni As List(Of HHDTO_movdis_row),
                    ByVal dizArticoli As Dictionary(Of String, HHDTO_articolo),
                    ByVal intLivello As Integer)

        ' Limite massimo di profondità
        If intLivello > 20 Then
            Throw New Exception("Superato il limite massimo di 20 livelli nella distinta.")
        End If

        ' Cerco tutti i figli del padre corrente
        Dim righe() As DataRow =
            dttBom.Select($"BOM_PARENT = {CStrSQL(strCodicePadre)}")

        ' Elaboro ogni figlio
        For Each dr As DataRow In righe

            Dim strCodiceFiglio As String = CStr(dr!BOM_CHILD)

            Dim oArticoloPadre As HHDTO_articolo = Nothing
            Dim oArticoloFiglio As HHDTO_articolo = Nothing

            If Not dizArticoli.TryGetValue(strCodicePadre, oArticoloPadre) Then
                Throw New Exception($"Anagrafica padre non trovata: {strCodicePadre}")
            End If

            If Not dizArticoli.TryGetValue(strCodiceFiglio, oArticoloFiglio) Then
                Throw New Exception($"Anagrafica figlio non trovata: {strCodiceFiglio}")
            End If



            ' verifico se l'articolo corrente è l'ultimo livello della distinta base.
            ' mi serve saperlo perché, nel caso fosse l'ultima foglia, dovrei controllare in 
            ' anagrafica articoli DBCENT ovvero BOMDB_DOC per leggere alcuni campi dove
            ' dobbiamo leggere la VERA materia prima da aggiungere sotto perché in SolidWork
            ' non viene gestita la MP vera e propria.
            Dim blnUltimoLivello As Boolean =
                        dttBom.Select(
                            $"BOM_PARENT={CStrSQL(strCodiceFiglio)}"
                        ).Length = 0


            ' Creo la relazione padre-figlio
            Dim objRelazione As New HHDTO_movdis_row With {
                .UID_Padre = strCodicePadre,
                .UID_Figlio = strCodiceFiglio,
                .oCodicePadre = oArticoloPadre,
                .oCodiceFiglio = oArticoloFiglio,
                .Quantita = Convert.ToDecimal(dr!BOM_QTY),
                .Posizione = CStr(dr!BOM_POS),
                .UltimoLivello = blnUltimoLivello
            }

            lstRelazioni.Add(objRelazione)

            ' Scendo al livello successivo
            LeggiRicorsivanenteDistinte(
                dttBom,
                strCodiceFiglio,
                lstRelazioni,
                dizArticoli,
                intLivello + 1)

        Next

    End Sub



    ''' <summary>
    ''' Crea l'indice per l'anagrafica articoli di DBCent
    ''' </summary>
    ''' <param name="dttArticoli"></param>
    ''' <returns></returns>
    Public Overridable Function CreaIndiceArticoli(ByVal dttArticoli As DataTable) As Dictionary(Of String, HHDTO_articolo)

        Dim dizArticoli As New Dictionary(Of String, HHDTO_articolo)(StringComparer.OrdinalIgnoreCase)

        For Each dr As DataRow In dttArticoli.Rows

            Dim strUID As String = Convert.ToString(dr!BOM_UID).Trim()

            If String.IsNullOrWhiteSpace(strUID) Then Continue For

            ' Verifico che il codice non sia già presente
            If dizArticoli.ContainsKey(strUID) Then
                Throw New Exception(
                        $"Codice BOM_UID duplicato in BOMDB_DOC: {strUID}")
            End If

            Dim oArticolo As New HHDTO_articolo With {
                    .BOM_UID = strUID,
                    .CODE = Convert.ToString(dr!CODE),
                    .DESC1 = Convert.ToString(dr!DESC1)
                }

            dizArticoli.Add(strUID, oArticolo)

        Next

        Return dizArticoli

    End Function


End Class
