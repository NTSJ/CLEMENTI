Imports System.Data
Imports NTSInformatica.CLN__STD
' WSS 439265 - verifica autoporting
<Serializable()>
Public Class CLDHHTASS
    Inherits CLD__BASE

    Public Overridable Function LeggiTabella(ByVal strDitta As String, ByRef dsOut As DataSet, Optional ByVal nCodTipoAssicur As Integer = 0) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = $"select HH_TIPIASSICURAZIONE.* from HH_TIPIASSICURAZIONE where HH_TIPIASSICURAZIONE.codditt = {CStrSQL(strDitta)}"
            If nCodTipoAssicur <> 0 Then
                strSQL += $"{vbCrLf}and HH_TIPIASSICURAZIONE.tb_codtipoassicur = {CDblSQL(nCodTipoAssicur)}"
            End If
            'Chiedo i dati al database
            dsOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "HH_TIPIASSICURAZIONE")

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            'Non eseguo la gestione errori standard ma giro l'errore 
            'direttamente al componente entity che mi ha chiamato
            CLN__STD.GestErr(ex, Me, "")
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function TestPreCancella(ByVal strDitta As String, ByVal nCodciva As Integer,
        ByRef strMsg As String) As Boolean
        Dim bDBMultiDitta As Boolean = False
        Dim i As Integer = 0
        Dim strSQL As String = ""
        Dim dttTmp As New DataTable
        Dim strMessSullaDitta As String = ""
        Try
            strMessSullaDitta = oApp.Tr(Me, 131189354499264850, "     sulla Ditta: ")
            '--------------------------------------------------------------------------------------------------------------
            bDBMultiDitta = False
            strSQL = "SELECT codditt FROM tabanaz"
            If OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI).Rows.Count > 1 Then bDBMultiDitta = True
            '--------------------------------------------------------------------------------------------------------------
            strSQL = "SELECT TOP 1 codditt, an_conto, an_descr1 FROM anagra" &
            $" WHERE an_hhcodtipoassicur = {CDblSQL(nCodciva)} and an_tipo in ('C','F') " &
            IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
            " ORDER BY codditt, an_conto"
            dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            If dttTmp.Rows.Count > 0 Then
                strMsg = " . 'Anagrafica clienti\fornitori' (tabella: ANAGRA)" & vbCrLf &
              "     Codice Clienti\fornitore: |" & NTSCStr(dttTmp.Rows(0)!an_conto) & "|" & vbCrLf &
              "     Rag. Sociale: |" & NTSCStr(dttTmp.Rows(0)!an_descr1) & "|"
                If bDBMultiDitta = True Then
                    strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0)!codditt)
                End If
                Return False
            End If
            dttTmp.Clear()
            dttTmp.Dispose()
            '--------------------------------------------------------------------------------------------------------------

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function


End Class
