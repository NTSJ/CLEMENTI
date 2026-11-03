Imports System.Data
Imports NTSInformatica.CLN__STD

''' <summary>
''' Gestione RAEE: classe di business per la gestione degli articoli con codice ecocontributo RAEE
''' </summary>
<Serializable()>
Public Class CLEHHRAEE
    Inherits CLE__BASE

    Public oCldRaee As CLDHHRAEE  'oggetto DAL

    ''' <summary>
    ''' Tabella contenente tutti gli articoli reali con gestione RAEE e relativo codice ecocontributo RAEE
    ''' </summary>
    Public dttRaeeLink As DataTable
    ''' <summary>
    ''' elenco articoli eccocontributo raee
    ''' </summary>
    Public dttRaeeEcocontr As DataTable

    Public dsRaee As DataSet

    Public Overrides Function Init(ByRef App As CLE__APP,
                              ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                              ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                              ByVal strFiller2 As String) As Boolean

        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHRAEE"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
        oCldRaee = CType(MyBase.ocldBase, CLDHHRAEE)
        oCldRaee.Init(oApp)
        LeggiRegistro()

        If Not Apri(oApp.Ditta) Then Return False

        Return True
    End Function


    Public Overridable Sub LeggiRegistro()
        Try


        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overloads Function Apri(ByVal strDitta As String) As Boolean
        Dim dReturn As Boolean = False
        Dim ds As New DataSet
        Try
            '--------------------------------------
            ' Ottengo la lista degli articoli con gestione RAEE e relativo codice ecoocontributo
            strDittaCorrente = strDitta
            dReturn = oCldRaee.GetElencoEcocontributi(strDittaCorrente, ds)
            If dReturn = False Then Return False

            ' aggingo anche un ulteriore dtt con la lista dei soli articoli ecocontributo
            Dim dttRaee As DataTable = ds.Tables("HHRAEELINK").DefaultView().ToTable(True, {"ar_hhcodartraee"})
            dttRaee.TableName = "HHRAEECOD"
            ds.Tables.Add(dttRaee)
            ds.AcceptChanges()

            '--------------------------------------------------------------
            SetDefaultValue(ds)
            dsShared = ds

            '--------------------------------------------------------------
            ' tengo un collegamento diretto ai 2 datatable
            dttRaeeLink = ds.Tables("HHRAEELINK")
            dttRaeeEcocontr = ds.Tables("HHRAEECOD")
            '--------------------------------------------------------------

            bHasChanges = False

            dsRaee = ds

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Sub SetDefaultValue(ByRef ds As DataSet)
        Try

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub


End Class
