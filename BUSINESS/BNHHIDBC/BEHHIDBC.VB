Imports System.Data
Imports NTSInformatica.CLN__STD

<Serializable()>
Public Class CLEHHIDBC
    Inherits CLE__BASE

#Region "variabili"


    Public oCldIdbc As CLDHHIDBC  'oggetto DAL

#End Region

#Region "moduli"

    Private Moduli_P As Integer = CLN__STD.bsModAll
    Private ModuliExt_P As Integer = CLN__STD.bsModExtAll
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

    Public ReadOnly Property Moduli() As Integer
        Get
            Return Moduli_P
        End Get
    End Property
    Public ReadOnly Property ModuliExt() As Integer
        Get
            Return ModuliExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliSup() As Integer
        Get
            Return ModuliSup_P
        End Get
    End Property
    Public ReadOnly Property ModuliSupExt() As Integer
        Get
            Return ModuliSupExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtn() As Integer
        Get
            Return ModuliPtn_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtnExt() As Integer
        Get
            Return ModuliPtnExt_P
        End Get
    End Property
#End Region

    Public Overrides Function Init(ByRef App As CLE__APP,
                              ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                              ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                              ByVal strFiller2 As String) As Boolean
        '-----------------------------------------------------------------------------------------------------------
        '-----------------------------------------------------------------------------------------------------------
        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHIDBC"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
        oCldIdbc = CType(MyBase.ocldBase, CLDHHIDBC)
        oCldIdbc.Init(oApp)

        LeggiRegistro()
        Return True
    End Function

    Public Overridable Sub LeggiRegistro()
        Try


        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub




End Class
