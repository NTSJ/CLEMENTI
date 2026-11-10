Imports System
Imports System.Data
Imports System.Globalization
Imports DevExpress.Utils.Internal
Imports NTSInformatica.CLN__STD
Public Class CLF__CLIE
    Inherits CLE__CLIE

#Region "proprietà e variabili"


    Public _oClhClie As CLH__CLIE
    Public Property oClhClie() As CLH__CLIE
        Get
            If _oClhClie Is Nothing Then _oClhClie = CType(oCldClie, CLH__CLIE)
            Return _oClhClie
        End Get
        Set(ByVal value As CLH__CLIE)
            _oClhClie = value
        End Set
    End Property

    Private _nHHdimCodAssic As Integer
    ''' <summary>
    ''' all'avvio della procedura registra il codice dell'assicurazione impostata sul cliente.
    ''' Serve in fase di salvataggio per capire se l'assicurazione è stata modificata
    ''' </summary>
    ''' <returns></returns>
    Public Property nHHdimCodAssic() As Integer
        Get
            Return _nHHdimCodAssic
        End Get
        Set(ByVal value As Integer)
            _nHHdimCodAssic = value
        End Set
    End Property

    Private _dHHDataInizioAssicur As Date
    ''' <summary>
    ''' all'avvio della procedura registra la data di inizio assicurazione impostata sul cliente.
    ''' Serve in fase di salvataggio per capire se l'assicurazione è stata modificata
    ''' </summary>
    ''' <returns></returns>
    Public Property dHHDataInizioAssicur() As Date
        Get
            Return _dHHDataInizioAssicur
        End Get
        Set(ByVal value As Date)
            _dHHDataInizioAssicur = value
        End Set
    End Property

    Private _strHHLivelloRating As String
    ''' <summary>
    ''' all'avvio della procedura registra il livello di rating dell'assicurazione impostata sul cliente.
    ''' Serve in fase di salvataggio per capire se l'assicurazione è stata modificata
    ''' </summary>
    ''' <returns></returns>
    Public Property strHHLivelloRating() As String
        Get
            Return _strHHLivelloRating
        End Get
        Set(ByVal value As String)
            _strHHLivelloRating = value
        End Set
    End Property

#End Region


#Region "routine standard"



    Public Overrides Function Apri(strDitta As String, bNuovo As Boolean, lContoCF As Integer, strApriWhere As String, ByRef dsClie As DataSet) As Boolean
        Dim bResult As Boolean = MyBase.Apri(strDitta, bNuovo, lContoCF, strApriWhere, dsClie)
        Dim HHbHasChangesMem As Boolean = bHasChanges
        Try
            If Not bResult Then Return False
            HHAddColumnsPers()
            If dsClie.Tables("ANAGRA").Rows.Count > 0 Then
                HHDecodDatiPersInApertura()
                HHASS_MemorizzaValoriIniziali()
            End If


            bHasChanges = HHbHasChangesMem

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try

        Return bResult
    End Function

    Public Overrides Function Salva(bDelete As Boolean) As Boolean
        Dim bResult As Boolean = MyBase.Salva(bDelete)

        Try
            If Not bResult Then Return False

            If HHASS_DatiAssicurModificati() Then
                HHASS_StoricizzaDatiAssicurazione()
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try

        Return bResult
    End Function



    Public Overridable Sub BeforeColUpdate_an_hhcodtipoassicur(sender As Object, e As DataColumnChangeEventArgs)

        Dim strDescr As String = ""
        Dim dtt As New DataTable

        Try
            If NTSCInt(e.ProposedValue) = 0 Then
                strDescr = ""
            Else

                If oClhClie.HHGetDataTipoAssicurazione(strDittaCorrente, NTSCInt(e.ProposedValue), dtt) Then

                    If dtt.Rows.Count > 0 Then
                        strDescr = NTSCStr(dtt.Rows(0)!tb_destipoassicur)
                    Else
                        ' errore, codice non valido
                        ThrowRemoteEvent(New NTSEventArgs("", "codice non valido"))
                        e.ProposedValue = e.Row(e.Column.ColumnName)
                        Return
                    End If
                End If
            End If

            e.Row!xx_hhdestipoassicur = strDescr

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        Finally
            dtt.Dispose() : dtt = Nothing
        End Try
    End Sub


    Public Overridable Sub AfterColUpdate_an_hhcodtipoassicur(sender As Object, e As DataColumnChangeEventArgs)
        Dim dtt As New DataTable
        Dim strTipoEv As String = "HHASSICURAZIONE_CAMPI_DATA_ENABLE"
        Dim strMsg As String = ""

        Try
            If NTSCInt(e.ProposedValue) = 0 Then
                ' devo richiamare il metodo per riaggiornare la data di scadenza
                e.Row!an_hhdatainizioassicur = NTSCDate("01/01/1900")
                e.Row!xx_hhdatafineassicur = NTSCDate("31/12/2099")
                ' disabilito i controlli grafici delle date
                strMsg = "0"
                ThrowRemoteEvent(New NTSEventArgs(strTipoEv, strMsg))
            Else

                If oClhClie.HHGetDataTipoAssicurazione(strDittaCorrente, NTSCInt(e.ProposedValue), dtt) Then

                    If dtt.Rows.Count > 0 Then
                        ' -------------------------------------------------------------
                        ' controllo se l'assicurazione ha la gestione scadenza e abilito\disabilito di conseguenza i campi data inizio e fine validità
                        If dtt.Rows(0)!tb_flscadenza.ToString = "S" Then
                            strMsg = "1"
                            ' devo richiamare il metodo per riaggiornare la data di scadenza
                            Dim eArg As New DataColumnChangeEventArgs(e.Row, dsShared.Tables("ANAGRA").Columns("an_hhdatainizioassicur"), e.Row!an_hhdatainizioassicur)
                            AfterColUpdate_an_hhdatainizioassicur(Me, eArg)
                        Else
                            strMsg = "0"
                        End If
                        ThrowRemoteEvent(New NTSEventArgs(strTipoEv, strMsg))
                        ' -------------------------------------------------------------
                    Else
                        ' errore, codice non valido
                        ThrowRemoteEvent(New NTSEventArgs("", "codice non valido"))
                        e.ProposedValue = e.Row(e.Column.ColumnName)
                        Return
                    End If
                End If
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Sub


    Public Overridable Sub AfterColUpdate_an_hhdatainizioassicur(sender As Object, e As DataColumnChangeEventArgs)

        Dim dtt As New DataTable
        Try
            If e.Row!an_hhcodtipoassicur.ToString = "0" Then
                e.Row!an_hhdatainizioassicur = NTSCDate("01/01/1900")
                e.Row!xx_hhdatafineassicur = NTSCDate("31/12/2099")
            Else
                If oClhClie.HHGetDataTipoAssicurazione(strDittaCorrente, NTSCInt(e.Row!an_hhcodtipoassicur), dtt) Then
                    If dtt.Rows(0)!tb_flscadenza.ToString = "S" Then
                        ' se l'assicurazione è gestita a scadenza allora calcola la data fine in base ai mesi inseriti in anagrafica
                        If NTSCDate(e.ProposedValue) = NTSCDate("01/01/1900") Then
                            e.Row!xx_hhdatafineassicur = NTSCDate("31/12/2099")
                        Else
                            e.Row!xx_hhdatafineassicur = DateAdd(DateInterval.Day, -1, (DateAdd(DateInterval.Month, NTSCInt(dtt.Rows(0)!tb_duratamesi), NTSCDate(e.ProposedValue))))
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        Finally
            dtt.Dispose() : dtt = Nothing
        End Try
    End Sub

#End Region

#Region "Routine personalizzate"



    ''' <summary>
    ''' Memorizza i valori iniziali dell'assicurazione in modo da poter capire se l'assicurazione è stata modificata in fase di salvataggio
    ''' </summary>
    Public Overridable Sub HHASS_MemorizzaValoriIniziali()
        Try
            nHHdimCodAssic = NTSCInt(dsShared.Tables("ANAGRA").Rows(0)!an_hhcodtipoassicur)
            dHHDataInizioAssicur = NTSCDate(dsShared.Tables("ANAGRA").Rows(0)!an_hhdatainizioassicur)
            strHHLivelloRating = dsShared.Tables("ANAGRA").Rows(0)!an_hhlivellorating.ToString
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Sub

    Public Overridable Sub HHDecodDatiPersInApertura()
        Try
            Dim e As New DataColumnChangeEventArgs(dsShared.Tables("ANAGRA").Rows(0),
                                                   dsShared.Tables("ANAGRA").Columns("an_hhcodtipoassicur"),
                                                   dsShared.Tables("ANAGRA").Rows(0)!an_hhcodtipoassicur)
            BeforeColUpdate_an_hhcodtipoassicur(Me, e)

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Sub

    ''' <summary>
    ''' Aggiungi colonne personalizzate a tabella di dsshared
    ''' </summary>
    Public Overridable Sub HHAddColumnsPers()
        Try
            If Not dsShared.Tables("ANAGRA").Columns.Contains("xx_hhdestipoassicur") Then
                dsShared.Tables("ANAGRA").Columns.Add(New DataColumn("xx_hhdestipoassicur", GetType(String)))
                dsShared.Tables("ANAGRA").Columns("xx_hhdestipoassicur").DefaultValue = ""
            End If
            If Not dsShared.Tables("ANAGRA").Columns.Contains("xx_hhdatafineassicur") Then
                dsShared.Tables("ANAGRA").Columns.Add(New DataColumn("xx_hhdatafineassicur", GetType(Date)))
                dsShared.Tables("ANAGRA").Columns("xx_hhdatafineassicur").DefaultValue = NTSCDate("01/01/1900")
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Sub


    ''' <summary>
    ''' Controlla se i campi relativi alla gestione delle assicurazioni sono stati modificati.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function HHASS_DatiAssicurModificati() As Boolean
        Try
            With dsShared.Tables("ANAGRA").Rows(0)
                If NTSCInt(dsShared.Tables("ANAGRA").Rows(0)!an_hhcodtipoassicur) <> nHHdimCodAssic Then Return True
                If NTSCDate(dsShared.Tables("ANAGRA").Rows(0)!an_hhdatainizioassicur) <> NTSCDate(dHHDataInizioAssicur) Then Return True
                If Not String.Equals(dsShared.Tables("ANAGRA").Rows(0)!an_hhlivellorating.ToString, strHHLivelloRating, StringComparison.CurrentCultureIgnoreCase) Then Return True
            End With

            Return False
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Function


    ''' <summary>
    ''' Storicizza le modifiche fatte ai dati dell'assicurazione (custom) nella tabella storico dei fidi. Li inserisco come record aggiuntivi.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function HHASS_StoricizzaDatiAssicurazione() As Boolean
        Try
            If oClhClie.HHAggiornaStoricoAssicurazioni(strDittaCorrente,
                                                       NTSCInt(dsShared.Tables("ANAGRA").Rows(nCurRow)!an_conto),
                                                       NTSCStr(dsShared.Tables("ANAGRA").Rows(nCurRow)!an_blocco),
                                                       NTSCDec(dsShared.Tables("ANAGRA").Rows(nCurRow)!an_fido),
                                                       NTSCInt(dsShared.Tables("ANAGRA").Rows(nCurRow)!an_hhcodtipoassicur),
                                                       NTSCDate(dsShared.Tables("ANAGRA").Rows(nCurRow)!an_hhdatainizioassicur),
                                                       NTSCStr(dsShared.Tables("ANAGRA").Rows(nCurRow)!an_hhlivellorating)) Then


                ' se il salvataggio è andato a buon fine allora aggiorno i valori iniziali in modo da non storicizzare più volte lo stesso record
                HHASS_MemorizzaValoriIniziali()

            End If


            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Function

#End Region

End Class