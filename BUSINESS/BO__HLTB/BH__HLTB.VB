Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Imports System
Public Class CLH__HLTB
    Inherits CLD__HLTB


    Public Overrides Function GetData(strNomeZoom As String, ByRef dsHltb As DataSet, strDitta As String, strDescr As String, strCodice As String, nAnno As Integer, nEscomp As Integer, lConto As Integer, lCommessa As Integer, strTipork As String, bIgnoraEsclusioni As Boolean, bObsoleti As Boolean, bModuloCRM As Boolean, bIsCRMUser As Boolean, bMaxRecord As Boolean, strDesEst As String, dtoGestStabilimenti As DTO__STD_GestStabilimenti, dtoGestSezionaliCG As DTO__STD_GestSezionaliCG, bConsideraPref As Boolean) As Boolean


        Dim bTabellaInesistente As Boolean = False
        Dim strSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = ""
        Dim strTabella As String = ""
        Dim bPrc As Boolean = False
        Dim strT() As String = Nothing
        Dim dttTmp As New DataTable

        Try

            If strNomeZoom.EndsWith("HH_TIPIASSICURAZIONE") Then

                If strDescr.Length > 0 Then strDescr = strDescr & "*"
                strDescr = strDescr.Replace("**", "*")
                strDescr = strDescr.Replace("*", "%")
                If strDesEst.Length > 0 Then strDesEst = strDesEst & "*"
                strDesEst = strDesEst.Replace("**", "*")
                strDesEst = strDesEst.Replace("*", "%")


                strTabella = "HH_TIPIASSICURAZIONE"

                '----------------------------------------------------------
                strSQL = " SELECT tb_codtipoassicur as tb_codice, tb_destipoassicur as tb_descr "
                '----------------------------------------------------------
                strSQL += " , * "
                strSQL += " FROM " & strTabella
                '----------------------------------------------------------

                'COMPONGO LA WHERE STANDARD
                If strDescr <> "" Then
                    strWhere = " WHERE tb_destipoassicur Like " & CStrSQL(strDescr)
                End If
                'se serve aggiungo il filtro ditta
                If IsPerDitta(True, strTabella) Then
                    If strWhere = "" Then
                        strWhere = " WHERE codditt = " & CStrSQL(strDitta)
                    Else
                        strWhere += " And codditt = " & CStrSQL(strDitta)
                    End If
                End If



                '----------------------------------------------------------
                'COMPONGO LA ORDER BY
                strOrder = " ORDER BY tb_codtipoassicur"

                '----------------------------------------------------------
                'MONTO TUTTA LA QUERY
                strSQL = strSQL & strWhere & strOrder


                '-------------------------------------------------------------------------------------------------------------
                '--- Chiedo i dati al database
                '-------------------------------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------------------------------
                Try

                    dsHltb = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, strNomeZoom)
                Catch ex As Exception
                    Dim lPosErr As Integer = ex.Message.LastIndexOf("SqlException: ") + "SqlException: ".Length
                    Dim strError() As String = (ex.Message.Substring(lPosErr)).Split(CChar("".PadLeft(1)))
                    If strError(0) = "208" Then
                        '---------------------------------------------------------------------------------------------------------
                        '--- Invalid object name (tabella non esistente)
                        '--- Se ha cercato la tabella in database Azienda ora la cerca in archivio Procedura,
                        '--- se l'ha cercata in archivio Procedura ora la cerca in database Azienda
                        '---------------------------------------------------------------------------------------------------------
                        bTabellaInesistente = True
                    Else
                        '---------------------------------------------------------------------------------------------------------
                        '--- Se non è Invalid object name (tabella non esistente), scateno l'errore ed esco
                        '---------------------------------------------------------------------------------------------------------
                        CLN__STD.GestErr(ex, Me, "")
                        Return False
                    End If
                End Try
                '-------------------------------------------------------------------------------------------------------------
                '--- Ho intercettato l'errore "208" (Invalid object name --> tabella non esistente),
                '--- in questo caso la cerco nell'atro database
                '-------------------------------------------------------------------------------------------------------------
                If bTabellaInesistente = True Then
                    If bPrc = True Then
                        dsHltb = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, strNomeZoom)
                    Else
                        dsHltb = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBPRC, strNomeZoom)
                    End If
                End If
                '-------------------------------------------------------------------------------------------------------------
                'devo togliere i record con descrizione che inzia con il prefisso indicato nella opzione di regitro
                If bObsoleti = False And oApp.oGvar.strZoomScartaDescrPrefix <> "" Then
                    For Each dtrT As DataRow In dsHltb.Tables(strNomeZoom).Rows
                        If NTSCStr(dtrT!tb_descr).StartsWith(oApp.oGvar.strZoomScartaDescrPrefix, StringComparison.CurrentCultureIgnoreCase) Then dtrT.Delete()
                    Next
                    dsHltb.AcceptChanges()
                End If

                AddColonnaSeleziona(dsHltb.Tables(0))
                '-----------------------------
                'se impostate delle esclusioni, devo aggiungere alla where quanto serve per non far vedere i codici da escludere
                If strNomeZoom.StartsWith("ZOOMTAB", StringComparison.CurrentCultureIgnoreCase) AndAlso bIgnoraEsclusioni = False Then
                    strSQL = "SELECT TOP 1 * FROM eszoom " &
                                             " WHERE es_nome = 'HH_TIPIASSICURAZIONE' " &
                                             "   AND (es_ditta = '*' OR es_ditta = " & CStrSQL(strDitta) & ") " &
                                             "   AND (es_opnome = '*' OR es_opnome = " & CStrSQL(oApp.User.Nome) & ") " &
                                             "   AND (es_ruolo = '*' OR es_ruolo = " & CStrSQL(oApp.User.Gruppo) & ") " &
                                             " ORDER BY es_ditta DESC, es_opnome DESC, es_ruolo DESC, es_escludi ASC"
                    dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                    If dttTmp.Rows.Count > 0 Then
                        If NTSCStr(dttTmp.Rows(0)!es_cods).Trim <> "" Then
                            For Each dtrT As DataRow In dsHltb.Tables(strNomeZoom).Select("NOT tb_codice " & IIf(dttTmp.Rows(0)!es_escludi.ToString = "N", " IN ", " NOT IN ").ToString &
                                                                                                          "(" & NTSCStr(dttTmp.Rows(0)!es_cods) & ")")
                                dtrT.Delete()
                            Next
                            dsHltb.AcceptChanges()
                        End If
                    End If
                End If    'If strTabNamees <> "" Then

                Return True

            Else
                Return MyBase.GetData(strNomeZoom, dsHltb, strDitta, strDescr, strCodice, nAnno, nEscomp, lConto, lCommessa, strTipork, bIgnoraEsclusioni, bObsoleti, bModuloCRM, bIsCRMUser, bMaxRecord, strDesEst, dtoGestStabilimenti, dtoGestSezionaliCG, bConsideraPref)
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try


    End Function

End Class