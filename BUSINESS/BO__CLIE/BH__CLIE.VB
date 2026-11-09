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


    ''' <summary>
    ''' Inserisce un nuovo record di storico per i dati personalizzati dell'assiscuraizone; viene sfruttata la stessa tabella dello storico fidi TABSTFI
    ''' dove sono stati aggiunti dei campi specifici.
    ''' </summary>
    ''' <param name="strDitta"></param>
    ''' <param name="lConto"></param>
    ''' <param name="nCodAssicurazione"></param>
    ''' <param name="dDataInizioAssicur"></param>
    ''' <param name="strLivelloRating"></param>
    ''' <returns></returns>
    Public Overridable Function HHAggiornaStoricoAssicurazioni(strDitta As String, lConto As Integer, strBlocco As String, dFido As Decimal,
                                                               nCodAssicurazione As Integer, dDataInizioAssicur As Date,
                                                               strLivelloRating As String) As Boolean
        '----------------------------------------
        'Inserisco un record nella tabella di storico FIDO (tabstfi) per memorizzare la variazione del blocco anagrafico
        ' Nota! memorizzo anche i valori dei campi standard blocco e fido altrimenti interrogandoi dati sembra che siano stati
        ' modificati anche questi campi.
        Try
            '----------------------------------------
            Dim strSQL As String = $"INSERT INTO TABSTFI(codditt, tb_conto, tb_progr, tb_ultagg, tb_opnome, tb_blocco, tb_importo , tb_fido,   
                                                        tb_hhcodassicurazione, tb_hhdatainizioassicur, tb_hhlivellorating  )
                               VALUES ({CStrSQL(strDitta)}, {lConto}, 
                               (ISNULL((SELECT MAX(tb_progr) FROM TABSTFI where codditt = {CStrSQL(strDitta)} and tb_conto = {lConto}), 0) +1),
                               {CDataOraSQL(Now)}, {CStrSQL(oApp.User.Nome)}, {CStrSQL(strBlocco)}, {CDblSQL("0")}, {CDblSQL(dFido)}, {CDblSQL(nCodAssicurazione)}, 
                               {CDataSQL(dDataInizioAssicur)}, {CStrSQL(strLivelloRating)})"

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            '----------------------------------------
            Return True

        Catch ex As Exception
            '---------------------------
            CLN__STD.GestErr(ex, Me, "")
            '---------------------------
        End Try
    End Function


End Class