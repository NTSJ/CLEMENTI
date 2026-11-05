Imports System
Imports System.Data
Imports DevExpress.XtraBars
Imports Microsoft
Imports NTSInformatica.CLN__STD
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*__HLTB
Public Class FRO__HLTB
    Inherits FRM__HLTB

    Public _oClfHltb As CLF__HLTB
    Public Property oClfHltb() As CLF__HLTB
        Get
            If _oClfHltb Is Nothing Then _oClfHltb = CType(oCleHltb, CLF__HLTB)
            Return _oClfHltb
        End Get
        Set(ByVal value As CLF__HLTB)
            _oClfHltb = value
        End Set
    End Property


    'Public Overrides Sub tlbGestione_ItemClick(sender As Object, e As ItemClickEventArgs)

    '    Dim strServer As String = ""
    '    Dim strNomeDll As String = ""
    '    Dim strPrgGest As String = ""
    '    Dim strParam As String = ""
    '    Dim dttTmp As New DataTable
    '    Try

    '        If strNomeZoom = "ZOOMHH_TIPIASSICURAZIONE" Then
    '            '---------------------------------
    '            'ottengo il nome del programma per la gestione
    '            strPrgGest = NTSZOOM.GetNomeProgForGest(strNomeZoom)
    '            If strPrgGest = "" Then
    '                oApp.MsgBoxErr(oApp.Tr(Me, 127791222145156250, "Funzione GESTIONE non abilitata per il programma '|" & strNomeZoom & "|'"))
    '                Return
    '            End If


    '            If grvZoom.RowCount = 0 Then
    '                strParam = "NUOV;" & "0"
    '            Else
    '                strParam = "APRI;" & grvZoom.GetFocusedRowCellDisplayText("tb_codice")
    '            End If


    '            '--------------------------
    '            'verifico se programma child è COM o NET
    '            If strPrgGest.Substring(0, 3).ToUpper <> "CLS" Then
    '                'QUI PASSA SOLO PER I Child NET MAI SCTRITTI IN vb6
    '                strNomeDll = strPrgGest
    '                strPrgGest = "FRM" & strPrgGest.Substring(2)
    '                strServer = "NTSInformatica"

    '                oMenu.RunChild(strServer, strPrgGest, "", DittaCorrente, "", strNomeDll, Nothing, strParam, True, True)
    '            Else
    '                'Child COM
    '                strServer = strPrgGest.Replace("CLS", "BS")
    '                oMenu.RunChild(strServer, strPrgGest, "", DittaCorrente, "", "", Nothing, strParam, True, True)
    '            End If

    '            If edDescr.Text.Trim <> "" Or grvZoom.RowCount > 0 Then cmdRicerca_Click(cmdRicerca, Nothing)

    '        Else
    '            ' standard
    '            MyBase.tlbGestione_ItemClick(sender, e)
    '        End If

    '    Catch ex As Exception
    '        CLN__STD.GestErr(ex, Me, "")
    '    End Try



    'End Sub


End Class

