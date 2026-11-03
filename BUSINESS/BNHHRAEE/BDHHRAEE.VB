Imports System.Data
Imports DevExpress.Utils.Internal
Imports NTSInformatica.CLN__STD
' WSS 439265 - verifica autoporting
<Serializable()>
Public Class CLDHHRAEE
    Inherits CLD__BASE

    Public Overridable Function GetElencoEcocontributi(strDitta As String, ByRef ds As DataSet) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "select distinct ar_codart, ar_hhcodartraee from artico where ar_hhflgestraee = 'S'"
            OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "HHRAEELINK", ds)

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, oApp.InfoError, oApp.ErrorLogFile)
        End Try
    End Function
End Class
