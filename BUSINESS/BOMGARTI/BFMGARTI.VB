Imports System
Imports System.Data
Imports System.Globalization
Imports DevExpress.Utils.Internal
Imports NTSInformatica.CLN__STD
Public Class CLFMGARTI
    Inherits CLEMGARTI

    Public _oClhArti As CLHMGARTI
    Public Property oClhArti() As CLHMGARTI
        Get
            If _oClhArti Is Nothing Then _oClhArti = CType(oCldArti, CLHMGARTI)
            Return _oClhArti
        End Get
        Set(ByVal value As CLHMGARTI)
            _oClhArti = value
        End Set
    End Property


    Public Overrides Function Apri(strDitta As String, ByRef ds As DataSet) As Boolean
        Dim bResult As Boolean = MyBase.Apri(strDitta, ds)

        Try
            If Not bResult Then Return False

            ' aggiungo colonne personalizzate al dataset
            HHAddColumnsPers(ds)




            ' gestione visibilità controlli in avvio form

            HHRAEE_VisControlliInApertura(ds)




            Return bResult
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try

    End Function

    ''' <summary>
    ''' Inizializzo i controlli personalizzati all'apertura di una anagrafica articolo, in base ai valori dei campi personalizzati
    ''' </summary>
    ''' <param name="ds"></param>
    Public Overridable Sub HHRAEE_VisControlliInApertura(ByRef ds As DataSet)
        Try
            If ds.Tables("ARTICO").Rows.Count > 0 Then
                Dim dtr As DataRow = ds.Tables("ARTICO").Rows(0)
                Dim e As New DataColumnChangeEventArgs(dtr, dtr.Table.Columns("ar_hhflgestraee"), NTSCStr(dtr!ar_hhflgestraee))
                AfterColUpdate_ar_hhflgestraee(Me, e)

                e = Nothing
                e = New DataColumnChangeEventArgs(dtr, dtr.Table.Columns("ar_hhcodartraee"), NTSCStr(dtr!ar_hhcodartraee))
                AfterColUpdate_ar_hhcodartraee(Me, e)
            End If
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub

    Public Overridable Sub HHAddColumnsPers(ByRef ds As DataSet)
        Try
            If ds Is Nothing Then Return
            If Not ds.Tables.Contains("ARTICO") Then Return
            ' -------------------------------

            If ds.Tables("ARTICO").Columns.Contains("xx_hhdesartraee") = False Then
                ds.Tables("ARTICO").Columns.Add("xx_hhdesartraee", GetType(String))
            End If
            ds.Tables("ARTICO").Columns("xx_hhdesartraee").DefaultValue = ""

        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub

    Public Overrides Function TestPreSalva() As Boolean
        Dim bResult As Boolean = MyBase.TestPreSalva()

        Try
            If Not bResult Then Return False

            ' RAEE
            If Not HHRAEE_TestPreSalva() Then
                Return False
            End If

            Return bResult
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try

    End Function


    ''' <summary>
    ''' Gestione RAEE: test presalvataggio
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function HHRAEE_TestPreSalva() As Boolean
        Try
            If dsShared.Tables("ARTICO").Rows(0)!ar_hhflgestraee.ToString = "S" And dsShared.Tables("ARTICO").Rows(0)!ar_hhcodartraee.ToString.Trim = "" Then
                ' è stata attivata la gestione raee ma non è stato indicato il codice articolo da importare nei documenti
                ThrowRemoteEvent(New NTSEventArgs(ThMsg.MSG_ERROR, "Se la gestione RAEE è attiva è obbligatorio indicare il codice articolo relativo all'Ecocontributo da usare in ordini e documenti."))
                Return False
            End If

            Return True
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Function

    Public Overridable Sub AfterColUpdate_ar_hhflgestraee(sender As Object, e As DataColumnChangeEventArgs)
        Try
            If e.ProposedValue.ToString = "S" Then
                ' gestione raee attivata
                ThrowRemoteEvent(New NTSEventArgs("HHGEST_RAEE_ABILITATA", ""))
            Else
                ' gestione raee disattivata
                ThrowRemoteEvent(New NTSEventArgs("HHGEST_RAEE_DISABILITATA", ""))
            End If
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub

    Public Overridable Sub AfterColUpdate_ar_hhcodartraee(sender As Object, e As DataColumnChangeEventArgs)

        Dim strDescr As String = ""
        Try
            If e.ProposedValue.ToString.Trim <> "" Then
                If Not oCldArti.ValCodiceDb(e.ProposedValue.ToString, strDittaCorrente, "ARTICO", "S", strDescr) Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Codice articolo non valido"))
                    e.ProposedValue = e.Row(e.Column.ColumnName)
                End If
            End If

            e.Row!xx_hhdesartraee = strDescr
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub

End Class