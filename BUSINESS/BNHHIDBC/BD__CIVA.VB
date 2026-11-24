Imports System.Data
Imports NTSInformatica.CLN__STD
' WSS 439265 - verifica autoporting
<Serializable()>
Public Class CLD__CIVA
  Inherits CLD__BASE

  Public Overridable Function LeggiTabCiva(ByVal strDitta As String, ByRef dsOut As DataSet) As Boolean
    Dim strSQL As String = ""
    Try

      strSQL = "SELECT tabciva.*, tb_dessmel as xx_codtpel, tb_destacq as xx_tipacq, tabciva_1.tb_desciva As xx_mvcodiva, " &
               "       tabciva_2.tb_desciva As xx_codindetr, tabciva_3.tb_desciva As xx_coddetr, tabciva_4.tb_desciva As xx_codagr, " &
               "       tabciva_5.tb_desciva As xx_codteor, tb_desrear AS xx_reparto, " &
               "       tabcovgD.tb_descovg as xx_codcovgd, tabcovgA.tb_descovg as xx_codcovga, tabciva_rc.tb_desciva as xx_codivarc, " &
               "       case when exists (select tb_codice from tabpref where tabpref.codditt = " & CStrSQL(strDitta) &
               "       and tb_tipopref = 'CI' and tb_codice = cast(tabciva.tb_codciva as varchar) and tb_opnome = " & CStrSQL(oApp.User.Nome) & ") THEN 'S' else 'N' end as xx_preferito," &
               " tabciva_6.tb_desciva As xx_codcivaperrigaom" &
               " FROM tabciva " &
               "  LEFT JOIN tabsmel ON " & CStrSQL(strDitta) & " = tabsmel.codditt AND tabciva.tb_tipel = tabsmel.tb_codsmel AND tabciva.tb_codtpel = tabsmel.tb_numsmel " &
               "  LEFT JOIN tabrear ON " & CStrSQL(strDitta) & " = tabrear.codditt AND tb_reparto = tb_codrear " &
               "  LEFT JOIN tabtacq ON tabciva.tb_tipacq = tabtacq.tb_codtacq " &
               "  LEFT JOIN tabciva As tabciva_1 ON tabciva.tb_mvcodiva = tabciva_1.tb_codciva" &
               "  LEFT JOIN tabciva As tabciva_2 ON tabciva.tb_codindetr = tabciva_2.tb_codciva" &
               "  LEFT JOIN tabciva As tabciva_3 ON tabciva.tb_coddetr = tabciva_3.tb_codciva" &
               "  LEFT JOIN tabciva As tabciva_4 ON tabciva.tb_codagr = tabciva_4.tb_codciva" &
               "  LEFT JOIN tabciva As tabciva_5 ON tabciva.tb_codteor = tabciva_5.tb_codciva" &
               "  LEFT JOIN tabciva As tabciva_rc ON tabciva.tb_codivarc = tabciva_rc.tb_codciva" &
               "  LEFT JOIN tabcovg As tabcovgD ON tabciva.tb_codcovgd = tabcovgD.tb_codcovg" &
               "  LEFT JOIN tabcovg As tabcovgA ON tabciva.tb_codcovga = tabcovgA.tb_codcovg" &
               "  LEFT JOIN tabciva As tabciva_6 ON tabciva.tb_codcivaperrigaom = tabciva_6.tb_codciva" &
               " ORDER BY tabciva.tb_codciva"
      'Chiedo i dati al database
      dsOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "TABCIVA")

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
      strSQL = "SELECT TOP 1 codditt, ae_codartf, ae_forn, ae_codmarc FROM artest" &
        " WHERE ae_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, ae_codartf, ae_forn, ae_codmarc"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        strMsg = oApp.Tr(Me, 130930967181487611, " . 'Catalogo Fornitori' (tabella: ARTEST)" & vbCrLf &
          "     Codice Articolo Fornitore: |" & NTSCStr(dttTmp.Rows(0) !ae_codartf) & "|" & vbCrLf &
          "     Fornitore: |" & NTSCStr(dttTmp.Rows(0) !ae_forn) & "|" & vbCrLf &
          "     Marca: |" & NTSCStr(dttTmp.Rows(0) !ae_codmarc) & "|")
        If bDBMultiDitta = True Then
          strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
        End If
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, ar_codart FROM artico" &
        " WHERE ar_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, ar_codart"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        strMsg = oApp.Tr(Me, 130930970136867718, " . 'Anagrafica Articoli' (tabella: ARTICO)" & vbCrLf &
          "     Codice Articolo: |" & NTSCStr(dttTmp.Rows(0) !ar_codart) & "|")
        If bDBMultiDitta = True Then
          strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
        End If
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mc_numcom, mc_riga FROM movcom" &
        " WHERE mc_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mc_numcom, mc_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        strMsg = oApp.Tr(Me, 130930977063124952, " . 'Gestione Punti Vendita (righe)' (tabella: MOCVOM)" & vbCrLf &
          "     Comanda N°: |" & NTSCStr(dttTmp.Rows(0) !mc_numcom) & "|" & vbCrLf &
          "     Riga: |" & NTSCStr(dttTmp.Rows(0) !mc_riga) & "|")
        If bDBMultiDitta = True Then
          strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
        End If
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, pn_datreg, pn_numreg, pn_riga FROM prinot" &
        " WHERE pn_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, pn_datreg, pn_numreg, pn_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        strMsg = oApp.Tr(Me, 130931051367097066, " . 'Gestione Prima Nota' (tabella: PRINOT)" & vbCrLf &
          "     Registrazione N°: |" & NTSCStr(dttTmp.Rows(0) !pn_numreg) & "|" & vbCrLf &
          "     del: |" & NTSCDate(dttTmp.Rows(0) !pn_datreg).ToShortDateString & "|" & vbCrLf &
          "     Riga: |" & NTSCStr(dttTmp.Rows(0) !pn_riga) & "|")
        If bDBMultiDitta = True Then
          strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
        End If
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mi_datreg, mi_numreg, mi_riga FROM moviva" &
        " WHERE mi_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mi_datreg, mi_numreg, mi_riga, mi_rigareg"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        strMsg = oApp.Tr(Me, 130930980716886895, " . 'Gestione Prima Nota' (tabella: MOVIVA)" & vbCrLf &
          " --> Movimenti Iva" & vbCrLf &
          "     Registrazione N°: |" & NTSCStr(dttTmp.Rows(0) !mi_numreg) & "|" & vbCrLf &
          "     del: |" & NTSCDate(dttTmp.Rows(0) !mi_datreg).ToShortDateString & "|" & vbCrLf &
          "     Riga: |" & NTSCStr(dttTmp.Rows(0) !mi_riga) & "|")
        If bDBMultiDitta = True Then
          strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
        End If
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mm_tipork, mm_anno, mm_serie, mm_numdoc, mm_riga FROM movmag" &
        " WHERE mm_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mm_tipork, mm_anno, mm_serie, mm_numdoc, mm_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130942188273685601, " . 'Gestione Documenti di Magazzino' (tabella: MOVMAG)" & vbCrLf &
            "     |" & DecodificaDocumento(NTSCStr(!mm_tipork)) & "|" &
            " N°|" & NTSCStr(!mm_numdoc) & IIf(NTSCStr(!mm_serie).Trim <> "", "/", "").ToString & NTSCStr(!mm_serie).Trim & "|" &
            " del |" & NTSCStr(!mm_anno) & "|, riga |" & NTSCStr(!mm_riga) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mo_anno, mo_serie, mo_numord, mo_vers, mo_riga FROM movoff" &
        " WHERE mo_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mo_tipork, mo_anno, mo_serie, mo_numord, mo_vers, mo_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130930990033829328, " . 'Gestione Offerte (righe)' (tabella: MOVOFF)" & vbCrLf &
            "     N°|" & NTSCStr(!mo_numord) & IIf(NTSCStr(!mo_serie).Trim <> "", "/", "").ToString & NTSCStr(!mo_serie).Trim & "|" &
            " del |" & NTSCStr(!mo_anno) & "|, versione |" & NTSCStr(!mo_anno) & "|, riga |" & NTSCStr(!mm_riga) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mo_tipork, mo_anno, mo_serie, mo_numord, mo_riga FROM movord" &
        " WHERE mo_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mo_tipork, mo_anno, mo_serie, mo_numord, mo_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130930982117579682, " . 'Gestione Ordini/Impegni (righe)' (tabella: MOVORD)" & vbCrLf &
            "     |" & DecodificaOrdine(NTSCStr(!mo_tipork)) & "|" &
            " N°|" & NTSCStr(!mo_numord) & IIf(NTSCStr(!mo_serie).Trim <> "", "/", "").ToString & NTSCStr(!mo_serie).Trim & "|" &
            " del |" & NTSCStr(!mo_anno) & "|, riga |" & NTSCStr(!mo_riga) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mm_tipork, mm_anno, mm_serie, mm_numdoc, mm_riga FROM movpar" &
        " WHERE mm_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mm_tipork, mm_anno, mm_serie, mm_numdoc, mm_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931033371398102, " . 'Gestione Parcelle e Notule (righe)' (tabella: MOVPAR)" & vbCrLf &
            "     |" & DecodificaParcella(NTSCStr(!mm_tipork)) & "|" &
            " N°|" & NTSCStr(!mm_numdoc) & IIf(NTSCStr(!mm_serie).Trim <> "", "/", "").ToString & NTSCStr(!mm_serie).Trim & "|" &
            " del |" & NTSCStr(!mm_anno) & "|, riga |" & NTSCStr(!mm_riga) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mm_tipork, mm_anno, mm_serie, mm_numdoc, mm_riga FROM movprb" &
        " WHERE mm_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mm_tipork, mm_anno, mm_serie, mm_numdoc, mm_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931034313693956, " . 'Gestione Documenti di Magazzino (righe)' (tabella: MOVPRB)" & vbCrLf &
            "     Nota di Prelievo N°|" & NTSCStr(!mm_numdoc) & IIf(NTSCStr(!mm_serie).Trim <> "", "/", "").ToString & NTSCStr(!mm_serie).Trim & "|" &
            " del |" & NTSCStr(!mm_anno) & "|, riga |" & NTSCStr(!mm_riga) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, mu_codusat, mu_progr FROM movusat" &
        " WHERE mu_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, mu_progr"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931037118246707, " . 'Anagrafica Beni Usati' (tabella: MOVUSAT)" & vbCrLf &
            " --> Movimenti Bene Usato |" & NTSCStr(!mu_progr) & "|" & vbCrLf &
            "     per il codice Bene Usato: |" & NTSCStr(!mu_codusat) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, nnc_numcontr FROM nncontras" &
        " WHERE nnc_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, nnc_numcontr"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931041110866513, " . 'Gestione Contratti di Assistenza' (tabella: NNCONTRAS)" & vbCrLf &
            "     Contratto N°|" & NTSCStr(!nnc_numcontr) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, nnr_numcontr, nnr_rinnid, nnr_riga FROM nnmovctra" &
        " WHERE nnr_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, nnr_numcontr, nnr_rinnid, nnr_riga"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931042670552338, " . 'Gestione Contratti di Assistenza' (tabella: NNMOVCTRA)" & vbCrLf &
            "     Contratto N°|" & NTSCStr(!nnr_numcontr) & "|, Rinnovo N°|" & NTSCStr(!nnr_rinnid) & "|, riga |" & NTSCStr(!nnr_riga) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, nnp_numprep FROM nnprepag" &
        " WHERE nnp_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, nnp_numprep"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931044445190846, " . 'Gestione Prepagati Monte-Ore' (tabella: NNPREPAG)" & vbCrLf &
            "     N°|" & NTSCStr(!nnp_numprep) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, ri_codrica, ri_codchia FROM nnricambi" &
        " WHERE ri_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, ri_codrica"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931046266458208, " . 'Gestione Chiamate' (tabella: NNRICAMBI)" & vbCrLf &
            "     Chiamata N°|" & NTSCStr(!ri_codchia) & "|, Id Ricambio |" & NTSCStr(!ri_codrica) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, ol_progr, ol_tipork FROM ordlist" &
        " WHERE ol_codiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, ol_progr"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931049028395202, " . 'Gestione Proposte d'Ordine' (tabella: ORDLIST)" & vbCrLf &
            "     |" & DecodificaProposta(NTSCStr(!ol_tipork)) & "| N°|" & NTSCStr(!ol_progr) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 tb_codciva FROM tabciva" &
        " WHERE tb_mvcodiva = " & nCodciva &
        " ORDER BY tb_codciva"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931053473150946, " . 'Codici Iva' (tabella: TABCIVA)" & vbCrLf &
            "     Codice Iva per salto aliquota" & vbCrLf &
            "     utilizzato per il Codice Iva |" & NTSCStr(!tb_codciva) & "|")
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 tb_codpece FROM tabpece" &
        " WHERE tb_codivafc = " & nCodciva &
        " ORDER BY tb_codpece"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931057343988948, " . 'Personalizzazione Cespiti' (tabella: TABPECE)" & vbCrLf &
            "     Codice Iva 'Fuori campo Iva'")
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 tb_codpecg FROM tabpecg" &
        " WHERE tb_codivafc = " & nCodciva &
        " ORDER BY tb_codpecg"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931058972466264, " . 'Personalizzazione Contabilità Generale' (tabella: TABPECG)" & vbCrLf &
            "     Codice Iva 'Fuori campo'")
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, tb_tiporeg, tb_numereg, tb_anno, tb_pcodiva FROM tabpriv" &
        " WHERE tb_pcodiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, tb_tiporeg, tb_numereg, tb_anno, tb_pcodiva"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931063046558868, " . 'Progressivi Registri Iva' (tabella: TABPRIV)" & vbCrLf &
            "     |" & DecodificaTipoRegistroIva(NTSCStr(!tb_tiporeg)) & "| N°|" & NTSCStr(!tb_numereg) &
            "| del |" & NTSCStr(!tb_anno) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, tb_tipork, tb_utipreg, tb_unumreg, tb_annoes FROM tabriel" &
        " WHERE tb_pcodiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, tb_tipork, tb_utipreg, tb_unumreg, tb_annoes, tb_pcodiva"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931074009901378, " . 'Dati per Rielaborazione Bollati' (tabella: TABRIEL)" & vbCrLf &
            "     |" & DecodificaBollati(NTSCStr(!tb_tipork)) & vbCrLf &
            "|     Tipo Registro Iva |" & DecodificaTipreg(NTSCStr(!tb_utipreg)) & "| N° |" & NTSCStr(!tb_unumreg) & "|" & vbCrLf &
            "     Anno Iva |" & NTSCStr(!tb_annoes) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 tb_codtpco FROM tabtpco" &
        " WHERE (tb_codivac = " & nCodciva & " OR tb_codivaai = " & nCodciva & ")" &
        " ORDER BY tb_codtpco"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931080996991659, " . 'Contabilizzazione Ritenute' (tabella: TABPCO)" & vbCrLf &
            "     Codice Iva Compenso e/o Codice Iva Altri Importi")
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, tb_codvpar FROM tabvpar" &
        " WHERE tb_pcodiva = " & nCodciva &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, tb_codvpar"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          strMsg = oApp.Tr(Me, 130931082532623299, " . 'Voci Parcellazione' (tabella: TABVPAR)" & vbCrLf &
            "     Codice Voce Parcellazione|" & NTSCStr(!tb_codvpar) & "|")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, tc_numcom, tc_codiva_1, tc_codiva_2, tc_codiva_3, tc_codiva_4," &
        " tc_codiva_5, tc_codiva_6, tc_codiva_7, tc_codiva_8 FROM testcom" &
        " WHERE (tc_codiva_1 = " & nCodciva &
          " OR tc_codiva_2 = " & nCodciva &
          " OR tc_codiva_3 = " & nCodciva &
          " OR tc_codiva_4 = " & nCodciva &
          " OR tc_codiva_5 = " & nCodciva &
          " OR tc_codiva_6 = " & nCodciva &
          " OR tc_codiva_7 = " & nCodciva &
          " OR tc_codiva_8 = " & nCodciva & ")" &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, tc_numcom"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          i = 1
          If NTSCInt(!tc_codiva_1) = nCodciva Then
            i = 1
          ElseIf NTSCInt(!tc_codiva_2) = nCodciva Then
            i = 2
          ElseIf NTSCInt(!tc_codiva_3) = nCodciva Then
            i = 3
          ElseIf NTSCInt(!tc_codiva_4) = nCodciva Then
            i = 4
          ElseIf NTSCInt(!tc_codiva_5) = nCodciva Then
            i = 5
          ElseIf NTSCInt(!tc_codiva_6) = nCodciva Then
            i = 6
          ElseIf NTSCInt(!tc_codiva_7) = nCodciva Then
            i = 7
          ElseIf NTSCInt(!tc_codiva_8) = nCodciva Then
            i = 8
          End If
          strMsg = oApp.Tr(Me, 130931087136523006, " . 'Gestione Punti Vendita' (tabella: TESTCOM)" & vbCrLf &
            "     |" & DecodificaDocumento(NTSCStr(!tc_tipork)) & "|" & vbCrLf &
            "     Comanda N° |" & NTSCStr(!tc_numcom) & "|" & vbCrLf &
            "     (Castelletto Iva posizione |" & i.ToString & "|)")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, tm_tipork, tm_anno, tm_serie, tm_numdoc, tm_codiva_1, tm_codiva_2, tm_codiva_3," &
        " tm_codiva_4, tm_codiva_5, tm_codiva_6, tm_codiva_7, tm_codiva_8 FROM testmag" &
        " WHERE (tm_codiva_1 = " & nCodciva &
          " OR tm_codiva_2 = " & nCodciva &
          " OR tm_codiva_3 = " & nCodciva &
          " OR tm_codiva_4 = " & nCodciva &
          " OR tm_codiva_5 = " & nCodciva &
          " OR tm_codiva_6 = " & nCodciva &
          " OR tm_codiva_7 = " & nCodciva &
          " OR tm_codiva_8 = " & nCodciva & ")" &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, tm_tipork, tm_anno, tm_serie, tm_numdoc"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          i = 1
          If NTSCInt(!tm_codiva_1) = nCodciva Then
            i = 1
          ElseIf NTSCInt(!tm_codiva_2) = nCodciva Then
            i = 2
          ElseIf NTSCInt(!tm_codiva_3) = nCodciva Then
            i = 3
          ElseIf NTSCInt(!tm_codiva_4) = nCodciva Then
            i = 4
          ElseIf NTSCInt(!tm_codiva_5) = nCodciva Then
            i = 5
          ElseIf NTSCInt(!tm_codiva_6) = nCodciva Then
            i = 6
          ElseIf NTSCInt(!tm_codiva_7) = nCodciva Then
            i = 7
          ElseIf NTSCInt(!tm_codiva_8) = nCodciva Then
            i = 8
          End If
          strMsg = oApp.Tr(Me, 130931094330618884, " . 'Gestione Documenti di Magazzino' (tabella: TESTMAG)" & vbCrLf &
            "     |" & DecodificaDocumento(NTSCStr(!tm_tipork)) & "|" & vbCrLf &
            " N°|" & NTSCStr(!tm_numdoc) & IIf(NTSCStr(!tm_serie).Trim <> "", "/", "").ToString & NTSCStr(!tm_serie).Trim & "|" &
            " del |" & NTSCStr(!tm_anno) & "|" & vbCrLf &
            "     (Castelletto Iva posizione |" & i.ToString & "|)")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
        Return False
      End If
      dttTmp.Clear()
      dttTmp.Dispose()
      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 codditt, tm_tipork, tm_anno, tm_serie, tm_numdoc, tm_codiva_1, tm_codiva_2, tm_codiva_3," &
        " tm_codiva_4, tm_codiva_5, tm_codiva_6, tm_codiva_7, tm_codiva_8 FROM testpar" &
        " WHERE (tm_codiva_1 = " & nCodciva &
          " OR tm_codiva_2 = " & nCodciva &
          " OR tm_codiva_3 = " & nCodciva &
          " OR tm_codiva_4 = " & nCodciva &
          " OR tm_codiva_5 = " & nCodciva &
          " OR tm_codiva_6 = " & nCodciva &
          " OR tm_codiva_7 = " & nCodciva &
          " OR tm_codiva_8 = " & nCodciva & ")" &
        IIf(bDBMultiDitta = False, " AND codditt = " & CStrSQL(strDitta), "").ToString &
        " ORDER BY codditt, tm_tipork, tm_anno, tm_serie, tm_numdoc"
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      If dttTmp.Rows.Count > 0 Then
        With dttTmp.Rows(0)
          i = 1
          If NTSCInt(!tm_codiva_1) = nCodciva Then
            i = 1
          ElseIf NTSCInt(!tm_codiva_2) = nCodciva Then
            i = 2
          ElseIf NTSCInt(!tm_codiva_3) = nCodciva Then
            i = 3
          ElseIf NTSCInt(!tm_codiva_4) = nCodciva Then
            i = 4
          ElseIf NTSCInt(!tm_codiva_5) = nCodciva Then
            i = 5
          ElseIf NTSCInt(!tm_codiva_6) = nCodciva Then
            i = 6
          ElseIf NTSCInt(!tm_codiva_7) = nCodciva Then
            i = 7
          ElseIf NTSCInt(!tm_codiva_8) = nCodciva Then
            i = 8
          End If
          strMsg = oApp.Tr(Me, 130931096785309490, " . 'Gestione Parcelle e Notule' (tabella: TESTPAR)" & vbCrLf &
            "     |" & DecodificaParcella(NTSCStr(!tm_tipork)) & "|" & vbCrLf &
            " N°|" & NTSCStr(!tm_numdoc) & IIf(NTSCStr(!tm_serie).Trim <> "", "/", "").ToString & NTSCStr(!tm_serie).Trim & "|" &
            " del |" & NTSCStr(!tm_anno) & "|" & vbCrLf &
            "     (Castelletto Iva posizione |" & i.ToString & "|)")
          If bDBMultiDitta = True Then
            strMsg &= vbCrLf & strMessSullaDitta & NTSCStr(dttTmp.Rows(0) !codditt)
          End If
        End With
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

  Public Overridable Function DecodificaDocumento(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case "A" : Return "Fattura Immediata Emessa"
        Case "B" : Return "D.D.T. Emesso"
        Case "C" : Return "Corrispettivo Emesso"
        Case "E" : Return "Nota di Addebito Emessa"
        Case "F" : Return "Ricevuta Fiscale Emessa"
        Case "I" : Return "Riemissione Ricevuta Fiscale"
        Case "J" : Return "Nota di Accredito Ricevuta"
        Case "L" : Return "Fattura Immediata Ricevuta"
        Case "M" : Return "D.D.T. Ricevuto"
        Case "N" : Return "Nota di Accredito Emessa"
        Case "S" : Return "Fattura/Ricevuta Fiscale Emessa"
        Case "T" : Return "Carico da Produzione"
        Case "U" : Return "Scarico a P"
        Case "Z" : Return "Bolla di Movimentazione Interna"
        Case "D" : Return "Fattura Differita Emessa"
        Case "K" : Return "Fattura Differita Ricevuta"
        Case "P" : Return "Fattura/Ricevuta Fiscale Differita"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function
  Public Overridable Function DecodificaOrdine(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case "R" : Return "Impegno Cliente"
        Case "O" : Return "Ordine Fornitore"
        Case "H" : Return "Ordine di Produzione"
        Case "X" : Return "Impegno Trasferimento"
        Case "Q" : Return "Preventivo"
        Case "#" : Return "Impegno di Commessa"
        Case "V" : Return "Impegno Cliente Aperto"
        Case "$" : Return "Ordine Fornitore Aperto"
        Case "Y" : Return "Impegno di Produzione"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function
  Public Overridable Function DecodificaParcella(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case "1" : Return "Fattura"
        Case "2" : Return "Nota di Accredito/Storno Fattura"
        Case "3" : Return "Avviso di Parcella/Notula"
        Case "4" : Return "Nota di Accredito"
        Case "5" : Return "Corrispettivo"
        Case "6" : Return "Fattura di Acconto"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function
  Public Overridable Function DecodificaProposta(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case "O" : Return "Ordine a Fornitore"
        Case "H" : Return "Ordine di Produzione"
        Case "X" : Return "Impegno di Trasferimento"
        Case "Y" : Return "Impegno di Produzione"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function
  Public Overridable Function DecodificaTipoRegistroIva(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case " " : Return "(no iva)"
        Case "T" : Return "Acquisti in Sospensione"
        Case "A" : Return "Acquisti"
        Case "C" : Return "Corrispettivi"
        Case "S" : Return "Vendite in Sospensione"
        Case "V" : Return "Vendite"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function
  Public Overridable Function DecodificaBollati(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case "G" : Return "Libro Giornale"
        Case "D" : Return "Dati Ultimi Registri Iva"
        Case "P" : Return "Progressivi Registrazioni Iva"
        Case "M" : Return "Progressivi Definitivi Magazzino"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function
  Public Overridable Function DecodificaTipreg(ByVal strTipork As String) As String
    Try
      '--------------------------------------------------------------------------------------------------------------
      Select Case strTipork
        Case "A" : Return "Acquisti"
        Case "V" : Return "Vendite"
        Case "C" : Return "Corrispettivi"
        Case "S" : Return "Acquisti in Sospensione"
        Case "T" : Return "Vendite in Sospensione"
        Case "X" : Return "Tutti"
      End Select
      '--------------------------------------------------------------------------------------------------------------
      Return ""
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return ""
    End Try
  End Function

  Public Overridable Function ValorizzaTb_natura(ByRef strError As String) As Boolean
    Dim strSQL As String = ""
    Dim dttTmp As New DataTable
    Try
      '----------------------------------------
      'Per tutti i codici iva che hanno tb_natura = Null, imposto il valore della relativa natura:
      '- se aliquota <> 0 o reverse charge = N --> tb_natura = '0' (non impostata) --> è diversa da null per distinguere quelli che ho già 'verificato' ma che comunque non devono avere natura
      ' - se aliquota = 0 o reverse charge <> N, imposto la natura con le logiche standard che Business applicava in fase di estrazione dati, quindi:
      ' • tb_art15 = S --> N1
      ' • tb_art15 = 7 --> N7
      ' • tb_revcharge <> N --> N6
      ' • tb_tipiva = 2 e vp11 = S e vp12 = N e bp21 = S e vp22 = S e volaff = S --> N2
      ' • tb_tipiva = 4 e (tb_art15 = N oppure tb_art15 = 2) --> N2
      ' • tb_tipiva = 2 e tb_espror = S --> N4
      ' • tb_tipiva = 3 oppure (!tb_tipiva) = 2 e tb_art73t = N) --> N3 
      ' • (tb_tipiva = 2 e tb_art73t <> N) oppure tb_art15 = R --> N5 

      'NB: la valorizzazione della natura sui codici iva, deve avvenire SOLO SE TUTTI i
      'codici IVA hanno natura NON IMPOSTATA. In questo modo, se ho eseguito normale conversione del db
      'tramite BusAdminTool, aprendo il programma, mi valorizza comunque la natura senza doverle impostare tutte manualmente
      'Tuttavia la conversione viene fatta SOLO SE TUTTI I CODICI IVA hanno natura non impostata e SOLO SE
      'TUTTE le ditte hanno in "personalizzazione FE" l'indicazione di utilizzare il vecchio tracciato
      '(perché se anche solo per una ditta dovessi usare il nuovo tracciato, l'impostazione automatica della natura
      'non sarebbe corretta, dal momento che posso impostare solo i valori generici (N2/N3/..) e non
      'quelli specifici richiesti dal nuovo tracciato (N2.1/N2.2/N3.1/..)

      '*** ATTENZIONE: QUESTO CONTROLLO ANDRA' INSERITO QUANDO SARA' INSERITO IN PERSONALIZZAZIONE FATTURA
      'ELETTRONICA IL CAMPO PER SCEGLIERE LA VERSIONE DEL TRACCIATO (PASSAGGIO DA TRACCIATO 1.5 A 1.6
      'PREVISTO PER OTTOBRE 2020/GENNAIO 2021). AL MOMENTO QUINDI LA CONVERSIONE DEI CODICI IVA VIENE FATTA
      'SENZA CONTROLLARE TABPEFE, MA RIMANE VALIDO CHE VIENE ESEGUITA SOLO SE TUTTI I CODICI IVA HANNO
      'TB_NATURA = NULL. INFATTI AL MOMENTO E' ANCORA IN USO IL TRACCIATO 1.5 CON NATURA N1, N2, N3, ...
      'E NON SONO AMMESSE LE NATURE SPECIFICHE DEL TRACCIATO 1.6 N2.1, N3.2, N3.3, ... CHE NON POTREBBERO
      'ESSERE IMPOSTATE IN MODO AUTOMATICO
      ''strSQL = " SELECT TOP 1 * FROM TABPEFE WHERE tb_verstracciato <> '1.5'"
      ''If OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI).Rows.Count > 0 Then Return True
      '***

      strSQL = " SELECT TOP 1 * FROM TABCIVA WHERE tb_natura is not null "
      If OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI).Rows.Count > 0 Then Return True

      strSQL = " UPDATE TABCIVA SET tb_natura = " &
               " case when tb_aliq <> 0 and tb_revcharge = 'N' then ' ' " &
               " else " &
               " case when tb_art15 = 'S' then 'N1' " &
               " when tb_art15 = '7' then 'N7' " &
               " when tb_revcharge <> 'N' then 'N6' " &
               " when tb_tipiva = 2 and tb_vp11 = 'S' and tb_vp12 = 'N' and tb_vp21 = 'S' and tb_vp22 = 'S' and tb_volaff = 'S' then 'N2' " &
               " when tb_tipiva = 4 and (tb_art15 = 'N' or tb_art15 = '2') then 'N2' " &
               " when tb_tipiva = 2 and tb_espror = 'S' then 'N4' " &
               " when tb_tipiva = 3 or (tb_tipiva = 2 and tb_art73t = 'N') then 'N3' " &
               " when (tb_tipiva = 2 and tb_art73t <> 'N') or tb_art15 = 'R' then 'N5' " &
               " else ' ' end " &
               " end " &
               " WHERE tabciva.tb_natura is null"
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      strError += oApp.Tr(Me, 132315890568927869, "Su tutti i codici IVA il campo 'Natura' è stato aggiornato in modo automatico. Verificare la correttezza dei dati impostati.") & vbCrLf

      'Inoltre, se erano presenti dei codici iva nell'opzione BSFECONS\OPZIONI\CodivaN6, aggiorno la natura su tali codici e poi svuoto l'opzione
      'per evitare che al prossimo accesso si esegua nuovamente la conversione
      'NB: l'opzione viene letta normalmente, quindi con priorità per azienda/utente, ma poi viene resettata per tutte le aziende/utenti!
      'Non dovrebbe verificarsi il caso in cui un codice IVA sia indicato in opzione con natura N6 per ditta specifica, ma
      'sarebbe comunque da documentare
      Dim strCodivaN6 As String = GetSettingBus("BSFECONS", "OPZIONI", ".", "CodivaN6", "", " ", "")
      If strCodivaN6.Trim <> "" Then
        strSQL = " UPDATE TABCIVA SET tb_natura = 'N6' WHERE tb_codiva IN (" & strCodivaN6.Replace(";", ",") & ")"
        Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        'L'opzione la aggiorno direttamente sul db in modo che sia resettata per tutti gli operatori/ditte
        'dal momento che la tabella dei codici IVA non ha dipendenza per ditta
        strSQL = " UPDATE REGPROP SET rp_valprop = '' WHERE rp_liv1 = 'BSFECONS' and rp_liv2 = 'OPZIONI' and rp_liv3 = '.' and rp_nomprop = 'CodivaN6'"
        Execute(strSQL, CLE__APP.DBTIPO.DBPRC)
        strError += oApp.Tr(Me, 132315934351684084, "Era impostata l'opzione di registro BSFECONS\OPZIONI\CodivaN6: per i seguenti codici IVA, quindi, la natura è stata impostata con 'N6':") & vbCrLf
        For Each strCodiva As String In strCodivaN6.Split(";"c)
          If strCodiva.Trim = "" Then Continue For
          strError += "• " & strCodiva & vbCrLf
        Next
      End If
      '----------------------------------------
      Return True

    Catch ex As Exception
      '---------------------------
      CLN__STD.GestErr(ex, Me, "")
      '---------------------------
    Finally
      dttTmp.Clear() : dttTmp.Dispose()
    End Try
  End Function

  Public Overridable Function CodiceIvaInDichiarazioniIntento(ByVal strDitta As String, ByVal nCodciva As Integer) As Boolean
    Dim strSQL As String = ""

    Try
      '------------------------------------------------------------------------------------------------------------
      If nCodciva = 0 Then Return False
      '------------------------------------------------------------------------------------------------------------
      '--- Controlla se il Codice Iva è stato utilizzato nei documenti collegati ad una Dichiarazione d'Intento
      '------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT TOP 1 testmag.tm_tipork" &
        " FROM testmag INNER JOIN testmagc ON testmag.codditt = testmagc.codditt" &
                                        " AND testmag.tm_tipork = testmagc.tm_tipork" &
                                        " AND testmag.tm_anno = testmagc.tm_anno" &
                                        " AND testmag.tm_serie = testmagc.tm_serie" &
                                        " AND testmag.tm_numdoc = testmagc.tm_numdoc" &
        " WHERE testmag.codditt = " & CStrSQL(strDitta) &
        " AND testmagc.tm_dinumero <> 0" &
        " AND (tm_codese = " & nCodciva &
        " OR tm_codivaspeinc = " & nCodciva
      For i As Integer = 1 To 8
        strSQL += " OR tm_codiva_" & NTSCStr(i) & " = " & nCodciva & IIf(i < 8, "", ")").ToString
      Next 'i 
      If OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI).Rows.Count > 0 Then Return True
      '------------------------------------------------------------------------------------------------------------ 
      '--- Controlla se il Codice Iva è stato utilizzato nelle registrazioni di Prima Nota 
      '--- collegate ad una Dichiarazione d'Intento
      '------------------------------------------------------------------------------------------------------------ 
      strSQL = "SELECT TOP 1 pn_datreg" &
        " FROM prinot INNER JOIN moviva ON prinot.codditt = moviva.codditt" &
                                     " AND prinot.pn_datreg = moviva.mi_datreg" &
                                     " AND prinot.pn_numreg = moviva.mi_numreg" &
                                     " AND prinot.pn_riga = moviva.mi_riga" &
        " WHERE prinot.codditt = " & CStrSQL(strDitta) &
        " AND pn_dinumero <> 0" &
        " AND mi_codiva = " & nCodciva
      If OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI).Rows.Count > 0 Then Return True
      '------------------------------------------------------------------------------------------------------------ 
      Return False
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

End Class
