Imports System
Imports System.Data
Imports System.Globalization
Imports DevExpress.Utils.Internal
Imports NTSInformatica.CLN__STD
Public Class CLF__CLIE
    Inherits CLE__CLIE

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

    Public Overrides Function Apri(strDitta As String, bNuovo As Boolean, lContoCF As Integer, strApriWhere As String, ByRef dsClie As DataSet) As Boolean
        Dim bResult As Boolean = MyBase.Apri(strDitta, bNuovo, lContoCF, strApriWhere, dsClie)
        Dim HHbHasChangesMem As Boolean = bHasChanges
        Try
            If Not bResult Then Return False

            HHAddColumnsPers()
            HHDecodDatiPersInApertura()

            bHasChanges = HHbHasChangesMem

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try

        Return bResult
    End Function

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

                        ' -------------------------------------------------------------
                        ' controllo se l'assicurazione ha la gestione scadenza e abilito\disabilito di conseguenza i campi data inizio e fine validità
                        Dim strTipoEv As String = "HHASSICURAZIONE_CAMPI_DATA_ENABLE"
                        Dim strMsg As String = ""
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

            e.Row!xx_hhdestipoassicur = strDescr



        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        Finally
            dtt.Dispose() : dtt = Nothing
        End Try
    End Sub


    Public Overridable Sub AfterColUpdate_an_hhdatainizioassicur(sender As Object, e As DataColumnChangeEventArgs)

        Dim dtt As New DataTable
        Try
            If oClhClie.HHGetDataTipoAssicurazione(strDittaCorrente, NTSCInt(e.Row!an_hhcodtipoassicur), dtt) Then
                If dtt.Rows(0)!tb_flscadenza.ToString = "S" Then
                    ' se l'assicurazione è gestita a scadenza allora calcola la data fine in base ai mesi inseriti in anagrafica
                    e.Row!xx_hhdatafineassicur = DateAdd(DateInterval.Day, -1, (DateAdd(DateInterval.Month, NTSCInt(dtt.Rows(0)!tb_duratamesi), NTSCDate(e.ProposedValue))))
                End If
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        Finally
            dtt.Dispose() : dtt = Nothing
        End Try
    End Sub

End Class