Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Imports System
Public Class CLH__STFI
  Inherits CLD__STFI


    Public Overrides Function ApriStoricoFido(strDitta As String, lConto As Integer, ByRef dsOut As DataSet) As Boolean
        Dim bResult As Boolean = MyBase.ApriStoricoFido(strDitta, lConto, dsOut)

        Dim strSQL As String = ""
        Try

            ' leggo la tabella personalizzata dei tipi assicurazione per decodificare i codici 
            strSQL = $"select HH_TIPIASSICURAZIONE.* from HH_TIPIASSICURAZIONE where HH_TIPIASSICURAZIONE.codditt = {CStrSQL(strDitta)}"
            OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "HH_TIPIASSICURAZIONE", dsOut)

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try


        Return bResult
    End Function

End Class