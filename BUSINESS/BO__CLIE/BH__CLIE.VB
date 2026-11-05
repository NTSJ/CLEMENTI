Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Imports System
Public Class CLH__CLIE
  Inherits CLD__CLIE

    Public Overridable Function HHGetDataTipoAssicurazione(strDitta As String, nCodTipoAssicur As Integer, ByRef dtt As DataTable) As Boolean

        Dim strSQL As String = ""
        Try

            strSQL = "select HH_TIPIASSICURAZIONE.* " &
                     "from HH_TIPIASSICURAZIONE " &
                     $"Where HH_TIPIASSICURAZIONE.codditt = {CStrSQL(strDitta)} and tb_codtipoassicur = {CDblSQL(nCodTipoAssicur)}"

            dtt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, strSQL)

        End Try
    End Function
End Class