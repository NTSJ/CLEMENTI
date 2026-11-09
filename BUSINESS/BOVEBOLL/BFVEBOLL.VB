Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.Globalization


Imports System
Imports System.Linq
Public Class CLFVEBOLL
    Inherits CLEVEBOLL

    Public _oClhBoll As CLHVEBOLL
    Public Property oClhBoll() As CLHVEBOLL
        Get
            If _oClhBoll Is Nothing Then _oClhBoll = CType(oCldBoll, CLHVEBOLL)
            Return _oClhBoll
        End Get
        Set(ByVal value As CLHVEBOLL)
            _oClhBoll = value
        End Set
    End Property

    Private _HHoCleRaee As CLEHHRAEE
    ''' <summary>
    ''' Gestione Ecocontributo RAEE
    ''' </summary>
    ''' <returns></returns>
    Public Property HHoCleRaee() As CLEHHRAEE
        Get
            If _HHoCleRaee Is Nothing Then _HHoCleRaee = NTSIstanziaEInitDll(Of CLEHHRAEE)("HHCODARTRAEE")
            Return _HHoCleRaee
        End Get
        Set(ByVal value As CLEHHRAEE)
            _HHoCleRaee = value
        End Set
    End Property

    Private _bHHGestioneRAEE As Boolean
    ''' <summary>
    ''' Opzioni di registro personalizzata per attivazione della gestione RAEE
    ''' </summary>
    ''' <returns></returns>
    Public Property bHHGestioneRAEE() As Boolean
        Get
            Return _bHHGestioneRAEE
        End Get
        Set(ByVal value As Boolean)
            _bHHGestioneRAEE = value
        End Set
    End Property


    Private _bHHXmlEstratto As Boolean
    ''' <summary>
    ''' Viene attivato in FROORGSOR.GestisciEventiEntity se all'apertura del documento
    ''' viene rilevato il messaggio standard "Il documento è collegato alla fattura elettronica...."
    ''' </summary>
    ''' <returns></returns>
    Public Property bHHXmlEstratto() As Boolean
        Get
            Return _bHHXmlEstratto
        End Get
        Set(ByVal value As Boolean)
            _bHHXmlEstratto = value
        End Set
    End Property


    Public Overrides Function Init(ByRef App As CLE__APP, ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, strTabella As String, bFiller1 As Boolean, strFiller1 As String, strFiller2 As String) As Boolean
        Dim bResult As Boolean = MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, bFiller1, strFiller1, strFiller2)

        Try
            If Not bResult Then Return False

            HHLeggiRegistro()
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try

        Return bResult
    End Function

    ''' <summary>
    ''' Lettura delle opzioni di registro personalizzate
    ''' </summary>
    Public Overridable Sub HHLeggiRegistro()
        Try
            ' attiva\disattiva la gestione del RAEE
            bHHGestioneRAEE = CType(oClhBoll.GetSettingBusDitt(strDittaCorrente, "BSVEBOLL", "OPZIONI", ".", "HHAttivaGestioneRAEE", "", ".", "0"), Boolean)
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub

    Public Overrides Function SalvaDocumento(strState As String) As Boolean

        Try
            If bHHGestioneRAEE And strState <> "D" And IsDocumentoEmesso(dttET.Rows(0)!et_tipork.ToString, False) Then
                If HHRAEE_TestPreElabora() Then
                    HHRAEE_GestioneCodiceEcocontributoRaee()
                End If
            End If
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
        Return MyBase.SalvaDocumento(strState)

    End Function

    ''' <summary>
    ''' Controlli pre avvio elaborazione RAEE
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function HHRAEE_TestPreElabora() As Boolean

        Dim bFatturato As Boolean = False
        Try
            If Not bDocEmesso Then Return False

            ' se il documento è collegato alla fattura elettronica non devo fare l'elaborazione RAEE
            If bHHXmlEstratto Then Return False

            ' se il ddt è fatturato non devo fare l'elaborazione RAEE
            If dttET.Rows(0)!et_tipork.ToString = "B" And dttET.Rows(0)!et_flfatt.ToString = "S" Then
                Return False
            End If


            Return True
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Function

    ''' <summary>
    ''' controlla la presenza di articoli con la gestione raee attiva a provvede ad aggiungere l'ecocontributo nel corpo del documento
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function HHRAEE_GestioneCodiceEcocontributoRaee() As Boolean
        Dim dttCodartRaee As New DataTable

        Dim bEcoRaeePresente As Boolean = False

        Try
            dttCodartRaee.Columns.Add(New DataColumn("xx_codart", GetType(String)))
            dttCodartRaee.Columns.Add(New DataColumn("xx_quant", GetType(Decimal)))

            For Each dtr As DataRow In dttEC.Rows

                ' controllo se l'articolo del corpo ha la gestione RAEE
                Dim dtrProdRaee() As DataRow = HHoCleRaee.dttRaeeLink.Select($"ar_codart={CStrSQL(dtr!ec_codart)}")
                If dtrProdRaee.Length > 0 Then
                    Dim dtrRaeeToInsert() As DataRow = dttCodartRaee.Select($"xx_codart = {CStrSQL(dtrProdRaee(0)!ar_hhcodartraee)}")
                    If dtrRaeeToInsert.Length = 0 Then
                        ' inserisco nuovo articolo raee
                        dttCodartRaee.Rows.Add()
                        dttCodartRaee.Rows(dttCodartRaee.Rows.Count - 1)!xx_codart = dtrProdRaee(0)!ar_hhcodartraee.ToString
                        dttCodartRaee.Rows(dttCodartRaee.Rows.Count - 1)!xx_quant = NTSCDec(dtr!ec_quant)
                    Else
                        ' aumetno quantità articolo raee già esistente
                        dtrRaeeToInsert(0)!xx_quant = NTSCDec(dtrRaeeToInsert(0)!xx_quant) + NTSCDec(dtr!ec_quant)
                    End If
                Else
                    ' controllo se l'articolo del corpo è un ecocontributo RAEE; mi serve per capire se devo proporre
                    ' l'inserimento dell'ecocontributo nel corpo del documento
                    If HHoCleRaee.dttRaeeEcocontr.Select($"ar_hhcodartraee={CStrSQL(dtr!ec_codart)}").Length > 0 Then
                        bEcoRaeePresente = True
                    End If
                End If
            Next

            ' routine inserimento nuove righe d'ordine.
            ' viene richiesto solo se nel corpo ci sono articoli con gestione RAEE e non è presente l'ecocontributo RAEE nel corpo del documento
            If dttCodartRaee.Rows.Count > 0 Then
                Dim e As NTSEventArgs = Nothing

                If Not bEcoRaeePresente Then
                    e = New NTSEventArgs(ThMsg.MSG_NOYES, "Sono presenti articoli con la gestione RAEE. Aggiungere l'ecocontributo nel corpo?")
                Else
                    e = New NTSEventArgs(ThMsg.MSG_NOYES, "Ricalcolare l'ecocontributo RAEE?")
                End If

                ThrowRemoteEvent(e)
                If e.RetValue = ThMsg.RETVALUE_NO Then Return True

                ' cancello tutti gli ecocontributi eventualmente presenti nel corpo 
                ' Creo l'elenco degli articoli da eliminare
                Dim articoli As New HashSet(Of String)(
                    HHoCleRaee.dttRaeeEcocontr.AsEnumerable() _
                    .Select(Function(dtr) dtr.Field(Of String)("ar_hhcodartraee"))
                )

                ' Individuo le righe dell'ordine da eliminare
                Dim righeDaEliminare As List(Of DataRow) = dttEC.AsEnumerable().
                                                                 Where(Function(dtr) articoli.Contains(dtr.Field(Of String)("ec_codart"))).
                                                                 ToList()

                For Each dtr As DataRow In righeDaEliminare
                    dtr.Delete()
                Next
                dttEC.AcceptChanges()

                For Each dtrRaee As DataRow In dttCodartRaee.Rows
                    AggiungiRigaCorpo(False, dtrRaee!xx_codart.ToString, 0, 0)
                    dttEC.Rows(dttEC.Rows.Count - 1)!ec_quant = NTSCDec(dtrRaee!xx_quant)
                    dttEC.Rows(dttEC.Rows.Count - 1)!backcolor_row = Color.GreenYellow.ToArgb
                    RecordSalva(dttEC.Rows.Count - 1, False, Nothing)
                Next

            End If

            Return True
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        Finally
            dttCodartRaee.Dispose() : dttCodartRaee = Nothing
        End Try
    End Function
End Class