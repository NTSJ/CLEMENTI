Imports System.Data
Imports NTSInformatica.CLN__STD

Imports System
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*__STFI
Public Class FRO__STFI
  Inherits FRM__STFI

#Region "proprietà e variabili"


    Public _oClfStfi As CLF__STFI
    Public Property oClfStfi() As CLF__STFI
        Get
            If _oClfStfi Is Nothing Then _oClfStfi = CType(oCleStfi, CLF__STFI)
            Return _oClfStfi
        End Get
        Set(ByVal value As CLF__STFI)
            _oClfStfi = value
        End Set
    End Property




#End Region
    Public Overrides Sub FRM__STFI_Load(sender As Object, e As EventArgs)
        MyBase.FRM__STFI_Load(sender, e)

        HHSetVisibColonneGrigliaPers()


    End Sub

    Public Overridable Sub HHSetVisibColonneGrigliaPers()
        Try

            grvStfi.NTSGetColumnByName("tb_hhcodassicurazione").Enabled = False
            grvStfi.NTSGetColumnByName("tb_hhdatainizioassicur").Enabled = False
            grvStfi.NTSGetColumnByName("tb_hhlivellorating").Enabled = False
            grvStfi.NTSGetColumnByName("xx_hhdestipoassicur").Enabled = False

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub
End Class

