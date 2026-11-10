Imports System.Data
Imports NTSInformatica.CLN__STD

<Serializable()>
Public Class CLEHHTASS
    Inherits CLE__BASE

    Public oCldTass As CLDHHTASS  'oggetto DAL


    Public Overrides Function Init(ByRef App As CLE__APP,
                              ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                              ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                              ByVal strFiller2 As String) As Boolean
        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHTASS"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
        oCldTass = CType(MyBase.ocldBase, CLDHHTASS)
        oCldTass.Init(oApp)



        LeggiRegistro()
        Return True
    End Function

    Public Overridable Sub LeggiRegistro()
        Try

            'Leggo le Opzioni di Registro globali
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overrides Function Apri(ByVal strDitta As String, ByRef ds As System.Data.DataSet) As Boolean
        Dim dReturn As Boolean = False
        Try
            '--------------------------------------
            'Tengo un puntatore al datatable che verrà usato nell'UI, quindi chiedo i dati al DAL
            strDittaCorrente = strDitta
            dReturn = oCldTass.LeggiTabella(strDittaCorrente, ds)
            If dReturn = False Then Return False

            '--------------------------------------------------------------
            'Imposto i valori di default della tabella: con la riga che segue prendo prima i valori dal database
            oCldTass.SetTableDefaultValueFromDB(strNomeTabella, ds)

            SetDefaultValue(ds)

            dsShared = ds

            '--------------------------------------
            'Creo gli eventi per la gestione del Datatable dentro l'Entity
            AddHandler dsShared.Tables(strNomeTabella).ColumnChanging, AddressOf BeforeColUpdate
            AddHandler dsShared.Tables(strNomeTabella).ColumnChanged, AddressOf AfterColUpdate

            bHasChanges = False

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Sub SetDefaultValue(ByRef ds As DataSet)
        Try

            'Imposto i valori di default diversi da quelli impostati nel database
            ds.Tables("HH_TIPIASSICURAZIONE").Columns("tb_codtipoassicur").DefaultValue = 0
            ds.Tables("HH_TIPIASSICURAZIONE").Columns("tb_destipoassicur").DefaultValue = " "
            ds.Tables("HH_TIPIASSICURAZIONE").Columns("tb_flscadenza").DefaultValue = "N"
            ds.Tables("HH_TIPIASSICURAZIONE").Columns("tb_duratamesi").DefaultValue = 0

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overrides Sub BeforeColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
        Dim strErr As String = ""
        Try
            'memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
            'solo se il dato è uguale a quello precedentemente contenuto nella cella
            If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString, e) Then
                strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
                Return
            End If
            '-------------------------------------------------------------
            'controllo che in una cella short non venga inserito un numero troppo grande
            If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)
            '-------------------------------------------------------------
            'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
            Dim strFunction As String = "BeforeColUpdate_" & e.Column.ColumnName.ToLower
            Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
            If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub


    Public Overrides Sub AfterColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
        Try
            'non valido la colonna se il dato non è cambiato
            If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
                strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
                Return
            End If

            bHasChanges = True

            'comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
            'vengano fatte le routine di validazione del caso
            ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

            e.Row.EndEdit()
            e.Row.EndEdit()

            '-------------------------------------------------------------
            'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
            Dim strFunction As String = "AfterColUpdate_" & e.Column.ColumnName.ToLower
            Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
            If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})
        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub

    Public Overrides Function TestPreSalva() As Boolean
        'Eseguo i controlli necessari prima di fare il salvataggio della riga

        Try
            For Each dtrRow As DataRow In dsShared.Tables("HH_TIPIASSICURAZIONE").Select(Nothing, Nothing, DataViewRowState.Added Or DataViewRowState.ModifiedCurrent)
                If NTSCInt(dtrRow!tb_codtipoassicur) <= 0 Then
                    ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128182577065937500, "Il Codice deve essere maggiore di 0")))
                    Return False
                End If


                If NTSCStr(dtrRow!tb_destipoassicur).Trim = "" Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Descrizione obbligatoria."))
                    Return False
                End If


                If NTSCStr(dtrRow!tb_flscadenza).Trim <> "N" And NTSCInt(dtrRow!tb_duratamesi) = 0 Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Se l'assicurazione è soggetta a scadenza la durata non può essere 0."))
                    Return False
                End If


            Next

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
            Return False
        End Try
    End Function

    Public Overrides Function Salva(ByVal bDelete As Boolean) As Boolean

        Dim bResult As Boolean = False
        Try
            '----------------------------------------
            'controlli pre-salvataggio (solo se non è una delete)
            If Not bDelete Then
                If Not TestPreSalva() Then Return False
            End If

            '----------------------------------------
            'chiamo il dal per salvare
            If strActLog <> "-1" Then
                bResult = oCldTass.ScriviTabellaSemplice(strDittaCorrente, strNomeTabella, dsShared.Tables(strNomeTabella), "", "", "")
            Else
                bResult = oCldTass.ScriviTabellaSemplice(strDittaCorrente, strNomeTabella, dsShared.Tables(strNomeTabella),
                  strActLogProg, strActLogNomOggLog, strActLogDesLog)
            End If

            If bResult Then
                bHasChanges = False
            End If

            Return bResult

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function TestPreCancella(ByRef dtrOld As DataRow) As Boolean
        Dim strMsg As String = ""

        Try

            '--------------------------------------------------------------------------------------------------------------
            If oCldTass.TestPreCancella(strDittaCorrente, NTSCInt(dtrOld!tb_codtipoassicur), strMsg) = False Then
                ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 130931098895495651, "Attenzione!" & vbCrLf &
                          "tipo assicurazione non cancellabile, perché presente in:") & vbCrLf & strMsg))
                Return False
            End If
            '--------------------------------------------------------------------------------------------------------------
            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Restiutisce i dati, sotto forma di DTO, dell'assicurazione abbinata nell'anagrafica del cliente indicato
    ''' </summary>
    ''' <param name="nConto"></param>
    ''' <param name="oOut">oggetto di tipo HHDTO_Assicurazione</param>
    ''' <returns></returns>
    Public Overridable Function GetDTOAssicurazione(nConto As Integer, ByRef oOut As HHDTO_Assicurazione) As Boolean

        Dim dsAssicur As New DataSet
        Dim dttConto As New DataTable
        Try
            oOut = New HHDTO_Assicurazione

            oCldTass.ValCodiceDb(nConto.ToString, strDittaCorrente, "ANAGRAC", "N", "", dttConto)

            If dttConto.Rows.Count = 0 Then
                ThrowRemoteEvent(New NTSEventArgs("", "Errore leggendo l'anagrafica del cliente"))
                Return False
            End If

            If Not oCldTass.LeggiTabella(strDittaCorrente, dsAssicur, NTSCInt(dttConto.Rows(0)!an_hhcodtipoassicur)) Then
                ThrowRemoteEvent(New NTSEventArgs("", "Errore aprendo l'anagrafica dell'assicurazione"))
                Return False
            End If

            With dsAssicur.Tables("HH_TIPIASSICURAZIONE").Rows(0)
                oOut.nConto = nConto
                oOut.nCodice = NTSCInt(!tb_codtipoassicur)
                oOut.strDescrizione = !tb_destipoassicur.ToString
                oOut.nTipo = NTSCInt(!tb_tipo)
                oOut.bGestita_a_scadenza = CType(IIf(!tb_flscadenza.ToString = "S", 1, 0), Boolean)
                oOut.nMesiValidita = NTSCInt(!tb_duratamesi)
                oOut.dDataInizioVal = NTSCDate(dttConto.Rows(0)!an_hhdatainizioassicur)
                ' la data fine viene calcolata in automatico
            End With


            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        Finally
            If dsAssicur.Tables.Count > 0 Then
                Dim dtt As DataTable = dsAssicur.Tables(0)
                dsAssicur.Tables.Remove(dtt)
                dtt.Dispose() : dtt = Nothing
            End If
            dsAssicur.Dispose() : dsAssicur = Nothing
        End Try
    End Function
End Class
